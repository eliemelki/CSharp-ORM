using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query.Queries;
using DBLibrary.QueryEngine.Query;
using DBLibrary.Mapper.ResultBinder;
using DBLibrary.Session.Cache;
namespace DBLibrary.Session.Executor
{
    class QueryExecutorFactory
    {
        private static QueryExecutorFactory instance = new QueryExecutorFactory();

        public static QueryExecutorFactory Current { get { return instance; } }

        public QueryExecutorFactory()
        {

        }

        public IExecutor GetQueryExecutor<R>(SqlQuery aQuery, ResultBinder<R> aBinder) where R : class 
        {
            if (aQuery is HasCache && aBinder is CacheBinder<R>)
            {
                return GetExecuteQuery((HasCache)aQuery,(HasMaximum)aQuery);
            }
            return new DefaultExecutor();
        }

        private IExecutor GetExecuteQuery(HasCache aCache,HasMaximum aMaximum)
        {
            CachePreference pref = aCache.GetCachePreferences();
            if (!pref.Cachable)
                return new DefaultExecutor();
            if (pref.CacheDuration <= 0)
                return new DefaultExecutor();
            if (!pref.IsCacheDependency)
                return new CacheExecutor(aCache.GetCachePreferences());

            return new DependencyExecutor(aCache.GetCachePreferences(),aMaximum.GetMaximum());
        }
    }
   
}
