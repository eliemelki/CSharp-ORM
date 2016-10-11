using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DBLibrary.Session.Executor
{
    class DefaultExecutor : IExecutor
    {
        public void Execute<R>(QueryInfo<R> aQueryInfo) where R : class 
        {
            aQueryInfo.Connection.execute(
               aQueryInfo.Query,
               delegate(SqlDataReader aReader)
               {
                   aQueryInfo.Binder.Bind(aReader);
               },
               delegate(SqlCommand aCommand) { },
               delegate() { });
        }
    }
}
