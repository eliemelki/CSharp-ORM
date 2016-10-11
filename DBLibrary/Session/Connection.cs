using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;
using System.Data.SqlClient;
using DBLibrary.Configuration;
using DBLibrary.Session.Executor;
using DBLibrary.Mapper.ResultBinder;
using log4net;
namespace DBLibrary.Session
{
    public interface DBConnection : IDisposable
    {
        Transaction BeginTransaction();
        void execute<R>(SqlQuery aQuery, ResultBinder<R> aBinder) where R : class;
        void execute(SqlQuery aQuery, OnExecute onExecute, BeforeExecute onBefore, AfterExecute onAfterExecute);
    }

    public delegate void OnExecute(SqlDataReader aReader);
    public delegate void BeforeExecute(SqlCommand aCommand);
    public delegate void AfterExecute();
    
    class DBConnectionImpl : DBConnection
    {
        private const String QUERY = "[QUERY TIME = {1} milliseconds] {0}";
        private const String EXECUTING_QUERY = "About to execute: {0}";
        private readonly ILog logger = LogManager.GetLogger(typeof(DatabaseLogger));

        private SqlConnection Connection { set; get; }
        private TransactionDetail Transaction { set; get; }
        private QueryTracker Tracker { set; get; }

        public DBConnectionImpl(Config aConfig, QueryTracker aTracker)
        {
            Tracker = aTracker;
            Connection = new SqlConnection(aConfig.DataSource);
            Transaction = Transaction_Null.Current;
            Connection.Open();
        }

        public void execute<R>(SqlQuery aQuery, ResultBinder<R> aBinder)  where R : class
        {
            try
            {
                Tracker.SaveQuery(aQuery);
                QueryExecutorFactory.Current.GetQueryExecutor(aQuery, aBinder).Execute(new QueryInfo<R>(this, aBinder, aQuery));
            }
            catch (Exception anExc)
            {
                logger.Error(anExc.Message, anExc);
                throw anExc;
            }
        }

        public void execute(SqlQuery aQuery, OnExecute onExecute, BeforeExecute onBefore, AfterExecute onAfterExecute)
        {
            
            using (SqlCommand Command = new SqlCommand(aQuery.Query, Connection))
            {
                Command.Parameters.AddRange(aQuery.Parameters.ToArray());
                Command.Transaction = Transaction.SqlTransaction;
                onBefore(Command);

                DateTime beforequery = DateTime.Now; ;
                logger.Debug(String.Format(EXECUTING_QUERY, aQuery));
                using (SqlDataReader aReader = Command.ExecuteReader())
                {
                    DateTime afterquery = DateTime.Now;
                    logger.Debug(String.Format(QUERY,aQuery, (afterquery - beforequery).Milliseconds));
                
                    while (aReader.Read())
                    {
                        onExecute(aReader);
                    }
                }
            }
            onAfterExecute();

        }

        public void Dispose()
        {
            Transaction.Dispose();
            
            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
            }
        }

        public Transaction BeginTransaction()
        {
            if (Transaction == Transaction_Null.Current)
            {
                Transaction = new TransactionImpl(Connection);
                return Transaction;
            }

            return Transaction;

        }

     
    }
}

