using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;
using DBLibrary.Session.Executor;
using System.Data.SqlClient;

namespace DBLibrary.Session.Cache
{
    class CacheExecutor : AbstractCacheExecutor
    {
        public CacheExecutor(CachePreference aPreference)
            : base(aPreference)
        {
        }
        protected override void ExecuteImpl<R>(QueryInfo<R> aQueryInfo) 
        {
            aQueryInfo.Connection.execute(
                  aQueryInfo.Query,
                  delegate(SqlDataReader aReader)
                  {
                      Bind(aQueryInfo, aReader);
                  },
                  delegate(SqlCommand aCommand)
                  {
                      Notification = new AspNetNotication(CacheSpecs,null);
                  },
                  delegate()
                  {
                      AddToCache();
                  });
        }
    }
}