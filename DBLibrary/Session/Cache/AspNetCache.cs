using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web;
using DBLibrary.QueryEngine.Query;
using log4net;
using System.Collections.Concurrent;

namespace DBLibrary.Session.Cache
{
    public delegate void OnCacheInValidate(String aKey, Object aValue);      

    class AspNetCache : LitsCache
    {
        private String INVALIDATE_DISPLAY = "Key: {0}; Value: {1} - Reason: {2}";
        private readonly ILog logger = LogManager.GetLogger(typeof(DatabaseLogger));
        private ConcurrentDictionary<String,String> CachedKeys;
        private System.Web.Caching.Cache cache;
        
        public AspNetCache()
        {
            cache = HttpRuntime.Cache;
            CachedKeys = new ConcurrentDictionary<String, String>();
            Listeners = new List<LitsCacheListener>();
        }

        public object Get(string aKey)
        {
            return cache.Get(aKey);
        }

        public void FreeCache()
        {
            logger.Debug("Free Cache");
            string[] _cachedKeys = CachedKeys.Keys.ToArray();
            foreach (String aKey in _cachedKeys)
            {
                Remove(aKey);
            }
        }

        public T Get<T>(string aKey)
        {
            return (T)cache.Get(aKey);
        }

        public void Remove(string aKey)
        {
            cache.Remove(aKey);
            String _value;
            CachedKeys.TryRemove(aKey, out _value);
        }
    
        public void Add(string aKey, object aValue, AspNetNotication aNotification)
        {
            CacheSpec cacheSpecs = aNotification.GetCacheSpec();
            CachePreference pref = cacheSpecs.GetCachePreferences();
            cache.Add(
                aKey, aValue,
                aNotification.GetDependency(),
                DateTime.Now.AddSeconds(pref.CacheDuration),
                System.Web.Caching.Cache.NoSlidingExpiration,
                CacheItemPriority.Normal,
                new CacheItemRemovedCallback(this.CacheItemRemovedCallback)
                );
            CachedKeys[aKey] = null;
        }

        
        public void CacheItemRemovedCallback(string aKey, object aValue, CacheItemRemovedReason aReason)
        {
            logger.Debug(String.Format(INVALIDATE_DISPLAY, aKey, aValue, aReason));
            NotifyListeners(aKey, aValue, aReason);
            String _value;
            CachedKeys.TryRemove(aKey, out _value);
        }

        private void NotifyListeners(string aKey, object aValue, CacheItemRemovedReason aReason)
        {
            foreach (LitsCacheListener _listener in Listeners)
            {
                _listener.onInvalidateCache(aKey, aValue, aReason);
            }
        }

        private List<LitsCacheListener> Listeners { set; get; }
    
        public void UnRegister(LitsCacheListener aListener)
        {
            Listeners.Remove(aListener);
        }

        public void Register(LitsCacheListener aListener)
        {
            Listeners.Add(aListener);
        }
    }

}
