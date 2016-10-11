using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Criteria;
using DBLibrary.QueryEngine.Expressions;
using DBLibrary.Mapper.ResultBinder;

namespace DBLibrary.QueryEngine.Query
{

    interface HasStatement<Q,S>
    {
        Q SetStatement(Action<S> anAction);
        Q SetStatement(S aStatement);
    }

    public interface HasCache
    {
        CachePreference GetCachePreferences();
    }

    interface HasCache<T> : HasCache
        where T : SqlQuery
    {
        T SetCache(CachePreference aPreference);
    }

    public interface HasCriteria
    {
        SqlCriteria GetCriteria();
    }

    interface HasCriteria<Q> : HasCriteria
    {
        Q SetCriteria(Action<SqlCriteria> anAction);
        Q SetCriteria(SqlCriteria aCriteria);
    }

    public interface HasMaximum
    {
        int GetMaximum();
    }

    interface HasMaximum<Q> : HasMaximum
    {
        Q SetMaximum(int aMaximum);
        
    }

    interface HasMinimum<Q> : HasMinimum
    {
        Q SetMinimum(int aMinimum);
    }

    public interface HasMinimum
    {
        int GetMinimum();
    }

  
}
