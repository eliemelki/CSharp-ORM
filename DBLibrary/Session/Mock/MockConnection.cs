using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBLibrary.Session.Mock
{
    public class MockConnection : DBConnection
    {
        private QueryTracker QueryTracker;
        private Transaction MockTransaction;
        public MockConnection(QueryTracker aQueryTracker,Transaction aTransaction)
        {
            QueryTracker = aQueryTracker;
            MockTransaction = aTransaction;
        }
        public Transaction BeginTransaction()
        {
            return MockTransaction;
        }

        public void execute<R>(QueryEngine.Query.SqlQuery aQuery, Mapper.ResultBinder.ResultBinder<R> aBinder) where R : class
        {
             QueryTracker.SaveQuery(aQuery);
        }

        public void execute(QueryEngine.Query.SqlQuery aQuery, OnExecute onExecute, BeforeExecute onBefore, AfterExecute onAfterExecute)
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}
