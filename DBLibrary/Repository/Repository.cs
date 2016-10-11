using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBLibrary.Session;
using System.Data;
using DBLibrary.QueryEngine.Criteria;
using DBLibrary;
using DBLibrary.Configuration;
using DBLibrary.QueryEngine.Query;
using DBLibrary.QueryEngine.Query.GenericSqlQuery;
using DBLibrary.QueryEngine.Expressions;
using log4net;
using System.Linq.Expressions;
using DBLibrary.QueryEngine.Query.Queries;
using DBLibrary.Repository.Command;

namespace DBLibrary.Repository
{

    public interface Repository
    {
    }

    public interface Repository<T> : Repository where T : class, new()
    {

        List<T> Get<E>(T aCriteriaModel = null, System.Linq.Expressions.Expression<Func<T, E>> aOrderBy = null, bool isAscending = true, CachePreference aCachePreferences = null, OnEntitySelect<T> onSelect = null);
        List<T> Get(SqlCriteria aCriteria = null, CachePreference aCachePreferences = null, OnEntitySelect<T> onSelect = null);

        T GetSingle(T aCriteriaModel = null, bool isIdIncluded = false, CachePreference aCachePreference = null);
        T GetSingle(SqlCriteria aSqlCriteria = null, CachePreference aCachePreference = null);

        T GetById(Object anId, CachePreference aCachePreferences = null);

        List<T> GetPager<E>(int aMinimum = 0, int aMaximum = 10, T aCriteriaModel = null, System.Linq.Expressions.Expression<Func<T, E>> aOrderBy = null, bool isAscending = true, String sqlQuery = null, CachePreference aCachePreferences = null, OnEntitySelect<T> onSelect = null);
        List<T> GetTop<E>(int aTop, T aCriteriaModel = null, System.Linq.Expressions.Expression<Func<T, E>> aOrderBy = null, bool isAscending = true, String sqlQuery = null, CachePreference aCachePreferences = null, OnEntitySelect<T> onSelect = null);

        int GetCount(T aCriteriaModel = null, String sqlQuery = null, CachePreference aCachePreferences = null);
        int GetCount(SqlCriteria aCriteria, CachePreference aCachePreferences);

        SqlCriteria GetCriteria();
        SelectQuery<T> GetQuery(CachePreference aPreference = null);

        List<T> ExecuteSelectQuery(ISelectQuery aQuery, OnEntitySelect<T> onSelect);
        DataTable ExecuteSelectQuery(ISelectQuery aQuery, OnRowSelect onSelect);

        ExpressionHelper<T> ExpressionHelper { get; }
        
    }
}