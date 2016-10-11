using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;

namespace DBLibrary.Session
{
    /*
     * Im not sure about this yet if it is thread safe, but i  set ninject to create an instance based on a thread context
     * Use It always after executing the query, and make sure the query is not executed in a different thread
     * To make it work. the query and GetLastExecutedQuery must be executed on the same thread.
     * example:
     * Session.execute(SqlQuery);
     * Tracker.GetLastExecutedQuery();
     */
    public interface QueryTracker
    {
        void SaveQuery(SqlQuery aQuery);
        SqlQuery GetLastExecutedQuery();
    }

    class QueryTrackerImpl : QueryTracker
    {
        private SqlQuery query;

        public void SaveQuery(SqlQuery aQuery)
        {
            query = aQuery;
        }


        public SqlQuery GetLastExecutedQuery()
        {
            return query;
        }
    }
}
