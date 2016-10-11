using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;
using DBLibrary.Mapper.ResultBinder;

namespace DBLibrary.Session.Executor
{
    
    class QueryInfo<R> where R : class
    {
        public DBConnection Connection;
        public ResultBinder<R> Binder;
        public SqlQuery Query;
        public QueryInfo(DBConnection aConnection, ResultBinder<R> aBinder, SqlQuery anSqlQuery)
        {
            Connection = aConnection;
            Binder = aBinder;
            Query = anSqlQuery;
        }
    }
}
