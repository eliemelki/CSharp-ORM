using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


using DBLibrary.Configuration;
using DBLibrary.QueryEngine.Query;
using DBLibrary.QueryEngine.Query.GenericSqlQuery;
using DBLibrary.QueryEngine.Query.Queries;
using DBLibrary.QueryEngine.Expressions;
using DBLibrary.QueryEngine.Criteria;
using DBLibrary.Mapper;
using DBLibrary.Mapper.ResultBinder;


namespace DBLibrary.Session
{
    class SessionImpl : DbSession
    {
        private Config Config { set; get; }
        private ClassMapLoader Loader { set; get; }
        private SqlFactory Factory { set; get; }
        private DBConnection Connetion;
        private ValueExtractor ValueExtractor;
        private QueryFactory QueryFactory { set; get; }

        public SessionImpl(
            Config aConfig,
            ClassMapLoader aLoader,
            SqlFactory aFactory,
            DBConnection aConnection,
            ValueExtractor aValueExtactor
            )
        {
            Config = aConfig;
            Loader = aLoader;
            Factory = aFactory;
            QueryFactory = aFactory.GetQueryFactory();
            Connetion = aConnection;
            ValueExtractor = aValueExtactor;
        }


        #region private functions
        private Expression GetByIdExpression<T>(T aData) where T : class, new()
        {
            ClassMap<T> classmap = Loader.GetClassMap<T>();
            IdentityMap identity = classmap.Identity;
            return ExpressionHelper.Eq(
                    identity.GetColumn(),
                    ValueExtractor.GetValue(identity.Member.Name, aData));
        }

        private SqlCriteria GetByIdCriteria<T>(T aData) where T : class, new()
        {
            var criteria = Factory.CreateCriteria().Add(GetByIdExpression<T>(aData));
            return criteria;
        }
        #endregion

        #region IUD

        public State Save<T>(T aModel, bool isBindNull) where T : class, new()
        {
            ClassMap<T> classmap = Loader.GetClassMap<T>();
            IdentityMap identity = classmap.Identity;
            Object data = ValueExtractor.GetValue(identity.Member.Name, aModel);
            if (data == null || (data is int && (int)data <= 0))
            {
                Insert(aModel, isBindNull);
                return State.Insert;
            }
            else
            {
                Update(aModel, isBindNull);
                 return State.Update;
            }
        }

        public int Update<T>(T aModel, bool isNullBind) where T : class, new()
        {
            return Update(aModel, GetByIdCriteria(aModel), isNullBind);
        }

        public int Update<T>(T aModel, SqlCriteria aCriteria, bool isNullBind) where T : class, new()
        {
            UpdateQuery<T> query = QueryFactory.CreateUpdateQuery<T>(aModel, isNullBind).SetCriteria(aCriteria);
            int result = 0;
            IntBinder binder = new IntBinder();
            binder.OnBind =
                delegate(IntResult aResult)
                {
                    result = aResult.value;
                };
            Connetion.execute<IntResult>(query, binder);
            return result;
        }

        public int Update<T>(T aUpdatedModel, T[] aModels, bool isBindNull) where T : class, new()
        {
            int _limit = 1000;
            int _index = 0;
            int _allCount = aModels.Count();
            for (int i = 0; i <= _allCount / _limit; i++)
            {
                int _count = (_index + _limit);
                _count = (_count >= _allCount) ? _allCount : _count;
                _index = ExecuteUpdatePartial(aUpdatedModel, isBindNull, aModels, _index, _count);

            }
            return 1;
        }


        public int ExecuteUpdatePartial<T>(T aUpdatedModel, bool isBindNull, T[] aModels, int aIndex, int aCount) where T : class, new()
        {
            SqlCriteria _criteria = Factory.CreateCriteria();
            var _exp = new List<Expression>();

            for (int i = aIndex; i < aCount; i++)
            {
                _exp.Add(GetByIdExpression(aModels[i]));
            }
            _criteria.Add(ExpressionHelper.Or(_exp.ToArray()));
            Update<T>(aUpdatedModel, _criteria, isBindNull);
            return aCount;
        }

        public void Insert<T>(T aModel, bool isNullBind) where T : class, new()
        {
            InsertQuery<T> query = QueryFactory.CreateInsertQuery<T>(aModel, isNullBind);
            IdentityMap identity = Loader.GetClassMap<T>().Identity;
        
            ResultBinder<T> binder = new TemplateInsertBinder<T>();
            binder.OnBind =
                delegate(T anEntity)
                {
                    String id = identity.Member.Name;
                    ValueExtractor.SetValue(id, aModel, ValueExtractor.GetValue(id, anEntity));
                  };
            Connetion.execute(query, binder);

        }

   
        public int Delete<T>(T aModel) where T : class, new()
        {
           return Delete<T>(GetByIdCriteria(aModel));
        }

        public int Delete<T>(params T[] aModels) where T : class, new()
        {
            int _limit = 1000;
            int _index = 0;
            int _allCount = aModels.Count();
            for (int i = 0; i <= _allCount / _limit; i++)
            {  
                int _count = (_index + _limit);
                _count = (_count >= _allCount) ? _allCount : _count; 
                _index = ExecuteDeletePartial(aModels, _index, _count);

            }
            return 1;
        }

     

        private int ExecuteDeletePartial<T>(T[] aModels, int aIndex, int aCount) where T : class, new()
        {
            SqlCriteria _criteria = Factory.CreateCriteria();
            var _exp = new List<Expression>();

            for (int i = aIndex; i < aCount; i++)
            {
                _exp.Add(GetByIdExpression(aModels[i]));
            }
            _criteria.Add(ExpressionHelper.Or(_exp.ToArray()));
            Delete<T>(_criteria);
            return aCount;
        }

        public int Delete<T>(SqlCriteria aCriteria) where T : class, new()
        {
            DeleteQuery<T> query = QueryFactory.CreateDeleteQuery<T>();
            query.SetCriteria(aCriteria);
            int result = 0;
            IntBinder binder = new IntBinder();
            binder.OnBind = 
                delegate(IntResult aResult)
                {
                    result = aResult.value;
                };
            Connetion.execute<IntResult>(query, binder);
            return result;
        }

        public int Delete<T>(object id) where T : class, new()
        {
          
            ClassMap<T> classmap = Loader.GetClassMap<T>();
            var criteria = Factory.CreateCriteria();
            IdentityMap identity = classmap.Identity;
            criteria.Add(ExpressionHelper.Eq(
                    identity.GetColumn(),
                    id));
            return Delete<T>(criteria);
           
        }
        public void Execute<R>(SqlQuery aQuery, ResultBinder<R> aResultBinder) where R : class
        {
            Connetion.execute(aQuery, aResultBinder);
        }
        #endregion

        #region select

        private SelectQuery<T> GetQuery<T>(CachePreference aPreferences) where T : class ,new()
        {
            if (aPreferences == null)
                aPreferences = new CachePreferenceImpl();

            return Factory.GetQueryFactory().CreateSelectQuery<T>().SetCache(aPreferences); 
        }

        public T GetById<T>(T aModel, CachePreference aPreferences = null) where T : class, new()
        {
            var query = GetQuery<T>(aPreferences).SetMaximum(1).SetCriteria(GetByIdCriteria(aModel));
            return Select<T>(query,null).Single();
        }

        public T GetById<T>(Object anID, CachePreference aPreferences = null) where T : class, new()
        {
            T data = new T();
            ClassMap<T> classmap = Loader.GetClassMap<T>();
            IdentityMap identity = classmap.Identity;
            ValueExtractor.SetValue(identity.Member.Name, data, anID);
            return Select<T>(GetQuery<T>(aPreferences).SetMaximum(1).SetCriteria(GetByIdCriteria<T>(data)), null).Single();
        }

        public T GetSingle<T>(SqlCriteria aCriteria, CachePreference aPreferences) where T : class, new()
        {
            if (aCriteria == null)
                aCriteria = Factory.CreateCriteria();
            var _results = Select<T>(GetQuery<T>(aPreferences).SetCriteria(aCriteria).SetMaximum(1), null);

            return _results.Count > 0 ? _results.First() : null;
        
        }

        public List<T> Get<T>(SqlCriteria aCriteria = null, OnEntitySelect<T> onExecute = null, CachePreference aPreferences = null) where T : class, new()
        {
            if (aCriteria == null)
                aCriteria = Factory.CreateCriteria();

            return Select<T>(GetQuery<T>(aPreferences).SetCriteria(aCriteria), new TemplateBinder<T>(), onExecute);
        }

        public List<T> Select<T>(ISelectQuery aQuery, OnEntitySelect<T> onExecute) where T : class, new()
        {
            if (aQuery == null)
                aQuery = QueryFactory.CreateSelectQuery<T>();
            return Select<T>(aQuery, new TemplateBinder<T>(), onExecute);
        }

        private List<T> Select<T>(ISelectQuery aQuery, ResultBinder<T> aBinder, OnEntitySelect<T> onExecute) where T : class , new()
        {

            List<T> dataList = new List<T>();
            int i = 0;
            aBinder.OnBind =
                 delegate(T anEntity)
                 {
                     dataList.Add(anEntity);
                     if (onExecute != null)
                     {
                         onExecute(anEntity, i);
                         i++;
                     }
                 };
            Connetion.execute(aQuery, aBinder);
            return dataList;
        }

        public DataTable Select(ISelectQuery aQuery, OnRowSelect onExecute)
        {
            if (aQuery == null)
                aQuery = QueryFactory.CreateSelectQuery();

            DataRow dr = null;
            RowBinder binder = new RowBinder();
            int i = 0;
            binder.OnBind = delegate(DataRow aRow)
            {
                dr = aRow;
                if (onExecute != null)
                {
                    onExecute(dr, i);
                    i++;
                }
            };
            Connetion.execute(aQuery, binder);
            if (dr == null)
                return null;

            return dr.Table;
        }

        #endregion

        #region dispose
        public Transaction BeginTransaction()
        {
            return Connetion.BeginTransaction();
        }

        public void Dispose()
        {
            Connetion.Dispose();
        }
        #endregion

        #region extra
        public int Count<T>(SqlCriteria aCriteria,CachePreference aPreferences) where T : class, new()
        {
            int count = 0;
            var query = QueryFactory.CreateSelectQuery();
            query.SetStatement(
                s =>
                {
                    s.AddField(Factory.GetExpressionHelper<T>().GetCountField<T>());
                    s.AddFrom(Loader.GetClassMap<T>().GetTableName());
                });

            if (aCriteria == null)
                aCriteria = Factory.CreateCriteria();

            if (aPreferences == null)
                aPreferences = new CachePreferenceImpl();

            query.SetCriteria(aCriteria).SetCache(aPreferences);

            IntBinder binder = new IntBinder();
            binder.OnBind = delegate(IntResult aResult)
            {
                count = aResult.value;
            };
            Connetion.execute(query, binder);
            return count;

        }

     
        #endregion


    }
}
