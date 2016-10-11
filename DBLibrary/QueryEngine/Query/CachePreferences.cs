using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBLibrary.QueryEngine.Query
{
    public interface CachePreference
    {
        bool Cachable { get; set; }
        int CacheDuration { get; set; }
        bool IsCacheDependency { get; set; }
    }


   public class CachePreferenceImpl : CachePreference
    {
       public CachePreferenceImpl()
       { 
       }
       
       public CachePreferenceImpl(CachePreference aPreference)
       {
           IsCacheDependency = aPreference.IsCacheDependency;
           Cachable = aPreference.Cachable;
           CacheDuration = aPreference.CacheDuration;
       }

       public CachePreferenceImpl(bool isCachable, int aCacheDuration, bool isCacheDependency)
       {
           Cachable = isCachable;
           CacheDuration = aCacheDuration;
           IsCacheDependency = isCacheDependency;
       }

       public virtual bool IsCacheDependency { get; set; }

       public virtual bool Cachable { get; set; }

       public virtual int CacheDuration { get; set; }

    }
}
