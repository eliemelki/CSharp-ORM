using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;
using System.Web.Caching;

namespace DBLibrary.Session.Cache
{
    public class AspNetNotication
    {
        private SqlCacheDependency dependency;
        private CacheSpec CacheSpec;
        
        public AspNetNotication(
            CacheSpec aCacheSpec, 
            SqlCacheDependency aDependency)
        {
            CacheSpec = aCacheSpec;
            dependency = aDependency;
        }


        public CacheSpec GetCacheSpec()
        {
            return CacheSpec;
        }

        public SqlCacheDependency GetDependency()
        {
            return dependency;
        }

        
    }
}
