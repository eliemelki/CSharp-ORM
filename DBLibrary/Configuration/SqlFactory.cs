using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Criteria;
using DBLibrary.QueryEngine.Query;
using DBLibrary.QueryEngine.Statements;
using DBLibrary.Session;
using DBLibrary.QueryEngine.Query.GenericSqlQuery;
using Ninject;
using Ninject.Modules;
using DBLibrary.Mapper;
using DBLibrary.Session.Cache;
using DBLibrary.QueryEngine;
using DBLibrary.QueryEngine.Expressions;
using Loader;
using DBLibrary.Repository;
namespace DBLibrary.Configuration
{

    public interface SqlFactory : InjectFactory
    {
        QueryFactory GetQueryFactory();
        ValueBinder GetValueBinder();
        Config GetConfig();

        SqlCriteria CreateCriteria();
        DbSession CreateSession();
        T CreateStatement<T>() where T : AbstractStatement, new();

        ClassMapLoader GetClassMapLoader();
        LitsCache GetCache();
        CacheKeyGenerator GetKeyGenerator();

        ModelCriteria GetModelCriteria();

        ValueExtractor GetValueExtractor();

        QueryTracker GetQueryTracker();

        ResultIterator GetResultIterator();

        SqlCacheDependencyEngine GetSqlDependencyEngine();

        DBHelper GetDBHelper();
            
        ExpressionHelper<T> GetExpressionHelper<T>() where T : class, new();

    }

    class FactoryImpl : AbstractInjectFactory, SqlFactory
    {
        public FactoryImpl(IKernel aKernel)
            : base(aKernel)
        {
        }

        public QueryEngine.Criteria.SqlCriteria CreateCriteria()
        {
            return GetInstance<QueryEngine.Criteria.SqlCriteria>();
        }

        public QueryEngine.Query.QueryFactory GetQueryFactory()
        {
            return GetInstance<QueryEngine.Query.QueryFactory>();
        }


        public T CreateStatement<T>() where T : QueryEngine.Statements.AbstractStatement, new()
        {
            return new T();
        }


        public Session.DbSession CreateSession()
        {
            return GetInstance<Session.DbSession>();
        }

        public ClassMapLoader GetClassMapLoader()
        {
            return GetInstance<ClassMapLoader>();
        }


        public LitsCache GetCache()
        {
            return GetInstance<LitsCache>();
        }

        public CacheKeyGenerator GetKeyGenerator()
        {
            return GetInstance<CacheKeyGenerator>();
        }


        public ModelCriteria GetModelCriteria()
        {
            return GetInstance<ModelCriteria>();
        }


        public ValueBinder GetValueBinder()
        {
            return GetInstance<ValueBinder>();
        }


        public ValueExtractor GetValueExtractor()
        {
            return GetInstance<ValueExtractor>();
        }

        public QueryTracker GetQueryTracker()
        {
            return GetInstance<QueryTracker>();
        }


        public ResultIterator GetResultIterator()
        {
            return GetInstance<ResultIterator>();
        }

        public ExpressionHelper<T> GetExpressionHelper<T>() where T : class, new()
        {
            var expression = GetInstance<DBExpression>();
            return expression.GetHelper<T>();
        }



        public SqlCacheDependencyEngine GetSqlDependencyEngine()
        {
            return GetInstance<SqlCacheDependencyEngine>();
        }

        public Config GetConfig()
        {
            return GetInstance<Config>();
        }

        public DBHelper GetDBHelper()
        {
            return GetInstance<DBHelperImpl>();
        }
    }
}
