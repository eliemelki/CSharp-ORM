using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Statements;
using DBLibrary.QueryEngine.Query;
using DBLibrary.QueryEngine.Criteria;
using DBLibrary.Mapper.ResultBinder;

namespace DBLibrary.QueryEngine.Query.Queries
{
    public interface ISelectQuery : 
        SqlQuery,
        HasCriteria,
        HasCache,
        HasMaximum,
        HasMinimum
    {

    }
    public class SelectQuery : 
        CriteriaQuery<SelectQuery, SelectStatement>, 
        HasCache<SelectQuery>,
        HasMaximum<SelectQuery>,
        HasMinimum<SelectQuery>,
        ISelectQuery
    {
        private CachePreference preferences;
        private int Minimum;
        private int Maximum;

        public SelectQuery()
        {
            preferences = new CachePreferenceImpl();

        }

        protected override SelectQuery GetThis()
        {
            return this;
        }

        public override string Query
        {
            get 
            {
                if (!this.preferences.IsCacheDependency)
                    return PagerBuilder.GetSqlString(base.Query, Minimum, Maximum);
                return base.Query;
            }
        }
        protected String AppendToQuery(StringBuilder aBuilder)
        {
          
            return aBuilder.ToString();
        }

        public SelectQuery SetCache(CachePreference aCache)
        {
            preferences = aCache;
            return this;
        }

        public CachePreference GetCachePreferences()
        {
            return preferences;
        }


        public SelectQuery SetMaximum(int aMaximum)
        {
            Maximum = aMaximum;
            return this; 
        }

        public SelectQuery SetMinimum(int aMinimum)
        {
            Minimum = aMinimum;
            return GetThis();
        }


        public int GetMaximum()
        {
            return Maximum;
        }


        public int GetMinimum()
        {
            return Minimum;
        }
    }
}
