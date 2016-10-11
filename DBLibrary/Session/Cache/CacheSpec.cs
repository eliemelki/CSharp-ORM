using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;

namespace DBLibrary.Session.Cache
{
   public interface CacheSpec
    {
        CachePreference GetCachePreferences();
    }

    class CacheSpecImpl : CacheSpec
    {
        public CacheSpecImpl(CachePreference aPreferences)
        {
            CachePreferences = new CachePreferenceImpl(aPreferences);

        }
        private CachePreference CachePreferences { set; get; }

        public CachePreference GetCachePreferences()
        {
            return CachePreferences;
        }
    }
}
