using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBLibrary.Configuration;
using DBLibrary.Session;
using DBLibrary;
using log4net;

namespace DBLibrary.Repository
{
    public delegate void Execute(DbSession aSession);
    public delegate void ExecuteRepo(DBRepository Inside);
    public delegate void OnException(Exception anException);


    public interface DBHelper
    {
        void Execute(Execute anExecute, OnException onException);
        void Execute(ExecuteRepo anExecute, OnException onException);
        void Execute(Execute anExecute);
    }

    class DBHelperImpl : DBHelper
    {
        private ILog logger = LogManager.GetLogger(typeof(DatabaseLogger));
        private SqlFactory factory;

        public DBHelperImpl(SqlFactory aFactory)
        {
            factory = aFactory;
        }

        public void Execute(Execute anExecute, OnException onException)
        {
            using (var session = factory.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        logger.Debug("Starting transaction ");
                        anExecute(session);
                        transaction.Commit();
                        logger.Debug("Commit Transaction ");
                    }
                    catch (Exception anExc)
                    {
                        transaction.RollBack();
                        logger.Debug("Roll back Operation; Exception occured: ");
                        if (onException != null)
                            onException(anExc);
                    }
                }
            }
        }

        public void Execute(Execute anExecute)
        {
            using (var session = factory.CreateSession())
            {
                anExecute(session);
            }
        }


        public void Execute(ExecuteRepo anExecute, OnException onException)
        {
            Execute(
               delegate(DbSession aSession)
               {
                   anExecute(new DBRepository(aSession));
               }, onException);
        }
    }


}