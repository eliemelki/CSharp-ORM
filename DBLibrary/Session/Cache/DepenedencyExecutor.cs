using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;
using DBLibrary.Session.Executor;
using System.Data.SqlClient;
using System.Web.Caching;
namespace DBLibrary.Session.Cache
{
    class DependencyExecutor : AbstractCacheExecutor
    {
        private int Maximum;
        public DependencyExecutor(CachePreference aPreference, int aMaximum)
            : base(aPreference)
        {
            Maximum = aMaximum;
        }
        protected override void ExecuteImpl<R>(QueryInfo<R> aQueryInfo) 
        {
            int i = 0;
            aQueryInfo.Connection.execute(
                  aQueryInfo.Query,
                  delegate(SqlDataReader aReader)
                  {
                      if (Maximum > 0)
                      {
                          if (i >= Maximum)
                          {
                              return;
                          }
                      }
                      Bind(aQueryInfo, aReader);
                      i++;
                  },
                  delegate(SqlCommand aCommand)
                  {
                      Notification = new AspNetNotication(CacheSpecs, new SqlCacheDependency(aCommand));
                  },
                  delegate()
                  {
                      AddToCache();
                  });
        }
    }
}
