using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Web.Caching;

namespace DBLibrary.Session.Cache
{
    public interface LitsCacheListener
    {
        void onInvalidateCache(string aKey, object aValue, CacheItemRemovedReason aReason);
    }

    public interface LitsCache
    {
        Object Get(String aKey);
        T Get<T>(String aKey);
        void Remove(String aKey);
        void Add(String aKey, Object aValue, AspNetNotication aNotification);
        void FreeCache();
        void Register(LitsCacheListener aListener);
        void UnRegister(LitsCacheListener aListener);
    }
}
