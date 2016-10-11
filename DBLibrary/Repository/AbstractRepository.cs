/* Code Explanations: 
 * 09-07-2010       Added GetSingle With boolean isIdIncluded                                               GAB
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using DBLibrary.Configuration;
using DBLibrary;
using DBLibrary.QueryEngine.Query;
using DBLibrary.Session;
using DBLibrary.QueryEngine.Query.GenericSqlQuery;
using DBLibrary.QueryEngine.Expressions;
using DBLibrary.QueryEngine.Criteria;
using System.Data;
using DBLibrary.QueryEngine.Query.Queries;
using DBLibrary.QueryEngine;
using DBLibrary.Mapper;
using DBLibrary.Repository.Command;

namespace DBLibrary.Repository
{
    public abstract class AbstractRepository<T> :  TableRepository<T> where T : class , new()
    {
        public ExpressionHelper<T> ExpressionHelper
        {
            get
            {
                return Database.Current.Factory.GetExpressionHelper<T>();
            }
        }

        protected readonly ILog logger = LogManager.GetLogger(typeof(DatabaseLogger));
        protected SqlFactory factory;
        protected DBHelper DBHelper;

       
        public AbstractRepository(DBHelper aDBHelper)
        {
            factory = Database.Current.Factory;
            DBHelper = aDBHelper;
        }

        public void setBDHelper(DBHelper aHelper)
        {
            DBHelper = aHelper;
        }

        #region Insert Update delete

        public virtual int Delete(SqlCriteria aCriteria)
        {
            int result = 0;
            DBHelper.Execute(
               delegate(DbSession aSession)
               {
                   result = aSession.Delete<T>(aCriteria);
               });
            CheckForResult(result, "Delete");
            return result;
        }

        public virtual int DeleteAll()
        {
            return Delete(factory.CreateCriteria());
        }

        public virtual int Delete(object anId)
        {
            int result = 0;
            DBHelper.Execute(
               delegate(DbSession aSession)
               {
                   result = aSession.Delete<T>(anId);
               });
            CheckForResult(result, "Delete");
            return result;
        }

        public virtual int Delete(params T[] aModels)
        {
            int result = 0;
            DBHelper.Execute(
               delegate(DbSession aSession)
               {
                   result = aSession.Delete<T>(aModels);
               });
            CheckForResult(result, "Delete");
            return result;
        }

        public virtual int Delete(T aModel)
        {
            int result = 0;
            DBHelper.Execute(
               delegate(DbSession aSession)
               {
                   result = aSession.Delete<T>(aModel);
               });
            CheckForResult(result, "Delete");
            return result;
        }

        public virtual void Insert(T aModel, bool aWithNullValues = false)
        {
            DBHelper.Execute(
                delegate(DbSession aSession)
                {
                    aSession.Insert(aModel, aWithNullValues);
                })
            ;
        }

        public virtual int Update(T aModel, bool aWithNullValues = false)
        {
            int result = 0;
            DBHelper.Execute(
               delegate(DbSession aSession)
               {
                   result = aSession.Update(aModel, aWithNullValues);
               });
            CheckForResult(result, "Update");
            return result;
        }

        public int Update(T aModel, T[] aData, bool aWithNullValues = false)
        {
            int result = 0;
            DBHelper.Execute(
               delegate(DbSession aSession)
               {
                   result = aSession.Update<T>(aModel,aData,aWithNullValues);
               });
            CheckForResult(result, "Delete");
            return result;
        }

        public virtual int Update(T aModel, SqlCriteria aCriteria, bool aWithNullValues = false)
        {
            int result = 0;
            DBHelper.Execute(
               delegate(DbSession aSession)
               {
                   result = aSession.Update(aModel, aCriteria, aWithNullValues);
               });
            CheckForResult(result, "Update");
            return result;
        }

        
        public virtual State Save(T aModel, bool aWithNullValues = false)
        {
            State state = State.Error;
            DBHelper.Execute(
               delegate(DbSession aSession)
               {
                   state = aSession.Save(aModel, aWithNullValues);
               });
            return state;
        }
        
        #endregion

        #region DBCommand

        public virtual List<T> SelectList(DBCommand<T> aCommand, CachePreference aCachePreferences)
        {
            var _query = GetQuery(aCachePreferences);
            var _criteria = _query.GetCriteria();
            ApplyFiltersForCriteria(_criteria, aCommand);
            ApplyOrderForQuery(_criteria, aCommand);
            if (aCommand.Pager != null && aCommand.Pager.RowCount > 0)
                ApplyPagerForQuery(_query, aCommand);
            return ExecuteSelectQuery(_query, delegate(T aT, int index) { });
        }

        public virtual DBCommandResult<T> Select(DBCommand<T> aCommand, CachePreference aCachePreferences)
        {
            int _count = Count(aCommand, aCachePreferences);
            List<T> _data = GetData(aCommand, aCachePreferences);
            return new DBCommandResultImpl<T>(_data,_count,aCommand.Pager);
        }

        public virtual int Count(DBCommand<T> aCommand, CachePreference aCachePreferences)
        {
            var _query = GetQuery(aCachePreferences);
            var _criteria = _query.GetCriteria();
            ApplyFiltersForCriteria(_criteria, aCommand);
            return GetCount(_criteria);
        }

        private List<T> GetData(DBCommand<T> aCommand, CachePreference aCachePreferences)
        {
            var _query = GetQuery(aCachePreferences);
            var _criteria = _query.GetCriteria();
            ApplyFiltersForCriteria(_criteria, aCommand);
            ApplyOrderForQuery(_criteria, aCommand);
            ApplyPagerForQuery(_query, aCommand);
            return ExecuteSelectQuery(_query, delegate(T aT, int index) { });
        }

        public void ApplyPagerForQuery(SelectQuery<T> aQuery, DBCommand<T> aCommand)
        {
            Command.DBPager _pager = aCommand.Pager;
            int _min = (_pager.PageNumber - 1) * _pager.RowCount;
            int _max = _min + 1 + _pager.RowCount;
            aQuery.SetMaximum(_max).SetMinimum(_min);
        }

        public void ApplyOrderForQuery(SqlCriteria anSqlCriteria, DBCommand<T> aCommand)
        {
            foreach (DBSort<T> _sort in aCommand.Sorts)
                anSqlCriteria.AddOrder(_sort.GetOrder());
        }

        public void ApplyFiltersForCriteria(SqlCriteria anSqlCriteria, DBCommand<T> aCommand)
        {
            foreach (DBFilterBase<T> _filter in aCommand.Filters)
                anSqlCriteria.Add(_filter.Expression());
        }
        #endregion

        #region selects

        public virtual T GetById(object anId, CachePreference aCachePreferences)
        {
            T data = null;
            DBHelper.Execute(
              delegate(DbSession aSession)
              {
                  data = aSession.GetById<T>(anId, aCachePreferences);
              });
            return data;
        }

        public virtual T GetSingle(T aCriteriaModel, bool isIdIncluded, CachePreference aCachePreference)
        {
            SqlCriteria _cri = GetDataModelCriteria(aCriteriaModel, isIdIncluded);
            return GetSingle(_cri, aCachePreference);
        }

        public virtual T GetSingle(SqlCriteria aSqlCriteria, CachePreference aCachePreference)
        {
            T data = null;
            DBHelper.Execute(
            delegate(DbSession aSession)
            {
                data = aSession.GetSingle<T>(aSqlCriteria, aCachePreference);
            });
            return data;
        }

        public virtual int GetCount(SqlCriteria aCriteria, CachePreference aCachePreferences)
        {
            return GetCount(aCriteria, null, aCachePreferences);
        }

        public virtual int GetCount(T aCriteriaModel, String sqlQuery, CachePreference aCachePreferences)
        {
            return GetCount(GetDataModelCriteria(aCriteriaModel, false), sqlQuery, aCachePreferences);
        }

        public SqlCriteria GetCriteria()
        {
            return factory.CreateCriteria();
        }

        public virtual List<T> GetTop<E>(int aTop, T aCriteriaModel = null, System.Linq.Expressions.Expression<Func<T, E>> aOrderBy = null, bool isAscending = true, String aSqlQuery = null, CachePreference aCachePreferences = null, OnEntitySelect<T> onSelect = null)
        {
            return ExecuteSelectQuery(GetModelOrderQueryWithSql(aCriteriaModel, aOrderBy, isAscending, aSqlQuery, aCachePreferences).SetMaximum(aTop), onSelect);
        }

        public virtual DataTable GetTop<E>(int aTop, T aCriteriaModel = null, System.Linq.Expressions.Expression<Func<T, E>> aOrderBy = null, bool isAscending = true, String aSqlQuery = null, CachePreference aCachePreferences = null, OnRowSelect onSelect = null)
        {
            return ExecuteSelectQuery(GetModelOrderQueryWithSql(aCriteriaModel, aOrderBy, isAscending, aSqlQuery, aCachePreferences).SetMaximum(aTop), onSelect);
        }

        public virtual List<T> Get(SqlCriteria aCriteria = null, CachePreference aCachePreferences = null, OnEntitySelect<T> onSelect = null)
        {
            if (aCriteria == null)
                aCriteria = GetCriteria();

            return ExecuteSelectQuery(GetQuery(aCachePreferences).SetCriteria(aCriteria), onSelect);
        }

        public virtual DataTable GetDByCriteria(SqlCriteria aCriteria = null, CachePreference aCachePreferences = null, OnRowSelect onSelect = null)
        {
            if (aCriteria == null)
                aCriteria = GetCriteria();
            return ExecuteSelectQuery(GetQuery(aCachePreferences).SetCriteria(aCriteria), onSelect);
        }

        public virtual List<T> Get<E>(T aCriteriaModel = null, System.Linq.Expressions.Expression<Func<T, E>> aOrderBy = null, bool isAscending = true,  CachePreference aCachePreferences = null, OnEntitySelect<T> onSelect = null)
        {
            return ExecuteSelectQuery(GetModelOrderQueryWithSql(aCriteriaModel, aOrderBy, isAscending, null, aCachePreferences), onSelect);
        }

        public virtual DataTable GetDByDataModelCriteria<E>(T aCriteriaModel = null, System.Linq.Expressions.Expression<Func<T, E>> aOrderBy = null, bool isAscending = true, String aSqlQuery = null, CachePreference aCachePreferences = null, OnRowSelect onSelect = null)
        {
            return ExecuteSelectQuery(GetModelOrderQueryWithSql(aCriteriaModel, aOrderBy, isAscending, aSqlQuery, aCachePreferences), onSelect);
        }

        public virtual List<T> GetPager<E>(int aMinimum = 0, int aMaximum = 10, T aCriteriaModel = null, System.Linq.Expressions.Expression<Func<T, E>> aOrderBy = null, bool isAscending = true, String aSqlQuery = null, CachePreference aCachePreferences = null, OnEntitySelect<T> onSelect = null)
        {
            return ExecuteSelectQuery(GetModelOrderQueryWithSql(aCriteriaModel, aOrderBy, isAscending, aSqlQuery, aCachePreferences).SetMaximum(aMaximum).SetMinimum(aMinimum), onSelect);
     
        }
        
        public virtual DataTable GetDPager<E>(int aMinimum = 0, int aMaximum = 10, T aCriteriaModel = null, System.Linq.Expressions.Expression<Func<T, E>> aOrderBy = null, bool isAscending = true, String aSqlQuery = null, CachePreference aCachePreferences = null, OnRowSelect onSelect = null)
        {
            return ExecuteSelectQuery(GetModelOrderQueryWithSql(aCriteriaModel, aOrderBy, isAscending, aSqlQuery, aCachePreferences).SetMaximum(aMaximum).SetMinimum(aMinimum), onSelect);
        }

        public virtual List<T> ExecuteSelectQuery(ISelectQuery aQuery, OnEntitySelect<T> onSelect)
        {
            List<T> list = null;
            DBHelper.Execute(
            delegate(DbSession aSession)
            {
                list = aSession.Select<T>(aQuery, onSelect);
            });
            return list;
        }
        
        public virtual DataTable ExecuteSelectQuery(ISelectQuery aQuery, OnRowSelect onSelect)
        {
            DataTable dt = null;
            DBHelper.Execute(
             delegate(DbSession aSession)
             {
                 dt = aSession.Select(aQuery, onSelect);
             });
            return dt;
        }

        public SelectQuery<T> GetQuery(CachePreference aPreference)
        {
            if (aPreference == null)
                aPreference = new CachePreferenceImpl();

            return factory.GetQueryFactory().CreateSelectQuery<T>().SetCache(aPreference);
        }
       
        #endregion

        #region private functions

        private const String INFO = "NOTICE: No row affected for the {0} occured on model {1}";
        protected void CheckForResult(int aResult, String aDisplay)
        {
            if (aResult <= 0)
                logger.Info(string.Format(INFO, aDisplay, typeof(T).Name));
        }

        private void OnException(Exception anException)
        {
            throw anException;
        }

        
        private int GetCount(SqlCriteria aCriteria = null, String aSqlQuery = null, CachePreference aCachePreferences = null)
        {
            int count = 0;
            if (aSqlQuery != null)
                aCriteria.AddSql(aSqlQuery);

            DBHelper.Execute(
              delegate(DbSession aSession)
              {
                  count = aSession.Count<T>(aCriteria, aCachePreferences);
              });

            return count;
        }

        protected SqlCriteria GetDataModelCriteria(T aCriteriaModel, bool isIdIncludedIfExists)
        {
            if (aCriteriaModel != null)
            {
                return factory.GetModelCriteria().GetCriteria(aCriteriaModel, delegate(SqlCriteria aCriteria, PropertyMap aMap, object aValue)
                {
                    aCriteria.Add(QueryEngine.Expressions.ExpressionHelper.Eq(ExpressionHelper.GetField(aMap), aValue));
                }, false, isIdIncludedIfExists);
            }
            else
                return factory.CreateCriteria();
        }

        protected SelectQuery<T> GetModelOrderQueryWithSql<E>(T aCriteriaModel, System.Linq.Expressions.Expression<Func<T, E>> aOrderBy, bool isAscending, String sqlQuery, CachePreference aCachePreferences = null)
        {
            var query = GetQuery(aCachePreferences).SetCriteria(GetDataModelCriteria(aCriteriaModel, false)); 
            var criteria = query.GetCriteria();
            if (!String.IsNullOrEmpty(sqlQuery))
            {
                criteria.AddSql(sqlQuery);
            }
            if (aOrderBy != null)
            {
                if (!isAscending)
                    criteria.AddOrder(ExpressionHelper.OrderDes(aOrderBy));
                else
                    criteria.AddOrder(ExpressionHelper.OrderAsc(aOrderBy));
            }
            return query;
            
        }
        #endregion
    }
}