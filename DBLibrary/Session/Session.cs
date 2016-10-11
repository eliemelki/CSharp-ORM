using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using DBLibrary.Mapper;

using DBLibrary.Configuration;
using DBLibrary.QueryEngine.Criteria;
using DBLibrary.QueryEngine.Query;
using DBLibrary.QueryEngine.Query.GenericSqlQuery;
using DBLibrary.QueryEngine.Query.Queries;
using DBLibrary.Mapper.ResultBinder;

namespace DBLibrary.Session
{
    public delegate void OnEntitySelect<T>(T anEntity, int index) where T : class, new();
    public delegate void OnRowSelect(DataRow aRow, int index);
    public delegate void OnObjectSelect(Object aRow, int index);

    public enum State { Insert, Update, Error }

    public interface DbSession : IDisposable
    {
        Transaction BeginTransaction();

        int Update<T>(T model, bool isBindNull = false) where T : class, new();
        int Update<T>(T model, SqlCriteria aCriteria, bool isBindNull = false) where T : class, new();
        int Update<T>(T model,  T[] aModels, bool isBindNull = false) where T : class, new();
        void Insert<T>(T model, bool isBindNull = false) where T : class, new();

        State Save<T>(T model, bool isBindNull = false) where T : class, new();

        int Delete<T>(T model) where T : class, new();
        int Delete<T>(SqlCriteria aCriteria) where T : class, new();
        int Delete<T>(object id) where T : class, new();
        int Delete<T>(params T[] aModels) where T : class, new();

        int Count<T>(SqlCriteria aCriteria = null, CachePreference aPreferences = null) where T : class, new();
    
        T GetById<T>(T aModel, CachePreference aPreferences = null) where T : class, new();
        T GetById<T>(Object anID, CachePreference aPreferences = null) where T : class, new();

        T GetSingle<T>(SqlCriteria aCriteria = null, CachePreference aPreferences = null) where T : class, new();
        List<T> Get<T>(SqlCriteria aCriteria = null, OnEntitySelect<T> onExecute = null, CachePreference aPreferences = null) where T : class, new();

        List<T> Select<T>(ISelectQuery aQuery = null, OnEntitySelect<T> onExecute = null) where T : class, new();
        DataTable Select(ISelectQuery aQuery = null, OnRowSelect onExecute = null);

        void Execute<R>(SqlQuery aQuery, ResultBinder<R> aResultBinder) where R : class;

    }

    
}
