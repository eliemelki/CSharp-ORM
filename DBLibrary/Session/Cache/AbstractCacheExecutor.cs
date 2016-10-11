using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using log4net;
using DBLibrary.QueryEngine.Query;
using DBLibrary.Configuration;
using DBLibrary.Session.Executor;
using DBLibrary.Mapper.ResultBinder;
namespace DBLibrary.Session.Cache
{
    abstract class AbstractCacheExecutor : IExecutor
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(DatabaseLogger));
        private const String FROM_CACHE = "Retrieving from cache result of Query: {0}\r";
        private static RowBinder ROW_BINDER = new RowBinder();
        private CacheKeyGenerator Generator;
        private String Key;
        protected LitsCache Cache;
        protected AspNetNotication Notification;
        protected DataTable CachedTable;
        protected CacheSpec CacheSpecs;

        public AbstractCacheExecutor(CachePreference aPreference)
        {
            CacheSpecs = new CacheSpecImpl(aPreference);
            Cache = Database.Current.Factory.GetCache();
            Generator = Database.Current.Factory.GetKeyGenerator();
        }
        public void Execute<R>(QueryInfo<R> aQueryInfo) where R : class
        {
            CachedTable = new DataTable();
            Key = Generator.GenerateKey(aQueryInfo.Query);
            if (GetFromCache(aQueryInfo))
            {
                logger.InfoFormat(FROM_CACHE, aQueryInfo.Query.Query);
                return;
            }
            ExecuteImpl(aQueryInfo);

        }
        protected abstract void ExecuteImpl<R>(QueryInfo<R> aQueryInfo) where R : class;


        protected void AddToCache()
        {
            Cache.Add(Key, CachedTable, Notification);
        }

        protected void Bind<R>(QueryInfo<R> aQueryInfo, SqlDataReader aReader) where R : class
        {
            R data = aQueryInfo.Binder.Bind(aReader);
            CacheBinder<R> cacheBinder = (CacheBinder<R>)aQueryInfo.Binder;
            CachedTable = cacheBinder.BindToCache(aReader).Table;

        }
        private bool GetFromCache<R>(QueryInfo<R> anExtractor) where R : class
        {
            DataTable dt = (DataTable)Cache.Get(Key);
            CacheBinder<R> cacheBinder = (CacheBinder<R>)anExtractor.Binder;
            if (dt != null)
            {
                foreach (DataRow datarow in dt.Rows)
                    anExtractor.Binder.OnBind(cacheBinder.BindFromCache(datarow));
                return true;
            }
            return false;
        }
    }
}
