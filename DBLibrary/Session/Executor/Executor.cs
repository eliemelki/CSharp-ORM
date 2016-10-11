using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;
using DBLibrary.QueryEngine.Query.GenericSqlQuery;
using System.Data.SqlClient;
using DBLibrary.Mapper.ResultBinder;
namespace DBLibrary.Session.Executor
{
    interface IExecutor
    {
        void Execute<R>(QueryInfo<R> aQueryInfo) where R : class;
    }

   
}
