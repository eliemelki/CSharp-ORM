using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using DBLibrary.Mapper;
using DBLibrary.Session;
using DBLibrary.QueryEngine.Criteria;
using DBLibrary.QueryEngine.Query.GenericSqlQuery;
using DBLibrary.QueryEngine.Query.Queries;
using DBLibrary.QueryEngine.Query;
using DBLibrary.Session.Cache;
using DBLibrary.QueryEngine.Statements;
using DBLibrary.Session.Executor;
using DBLibrary.QueryEngine;
using DBLibrary.QueryEngine.Expressions;
using DBLibrary.Repository;
namespace DBLibrary.Configuration
{
    public class SqlModules : NinjectModule
    {
        public override void Load()
        {
            //Singleton

            Bind<SqlFactory>().To<FactoryImpl>().InSingletonScope().WithConstructorArgument("aKernel", Kernel);
            Bind<Config>().To<ConfigImpl>().InSingletonScope();
            Bind<ClassMapLoader>().To<ClassMapLoaderImpl>().InSingletonScope();
            Bind<Initialise>().To<InitialiseImpl>().InSingletonScope();
            Bind<DBHelper>().To<DBHelperImpl>().InSingletonScope();

            Bind<ValueExtractor>().To<ValueExtractorImpl>().InSingletonScope();
            Bind<PropertyBinder>().To<PropertyBinderImpl>().InSingletonScope();
            Bind<ModelCriteria>().To<ModelCriteriaImpl>().InSingletonScope();
            Bind<QueryTracker>().To<QueryTrackerImpl>().InThreadScope();
            Bind<ValueBinder>().To<ValueBinderImpl>().InSingletonScope();


            Bind<ResultIterator>().To<ResultIteratorImpl>().InSingletonScope();
            
            Bind<DBExpression>().To<DBExpression>().InSingletonScope();

            Bind<QueryFactory>().To<QueryFactoryImpl>().InSingletonScope();
            Bind<LitsCache>().To<AspNetCache>().InSingletonScope();
            Bind<CacheKeyGenerator>().To<CacheKeyGeneratorImpl>().InSingletonScope();

               //Transiant
      
            Bind<DBConnection>().To<DBConnectionImpl>().InTransientScope();
            Bind<DbSession>().To<SessionImpl>().InTransientScope();
            Bind<SqlCriteria>().To<CriteriaImpl>().InTransientScope();

            Bind<SqlCacheDependencyEngine>().To<SqlCacheDependencyEngineImpl>().InSingletonScope();

            Bind(typeof(TableRepository<>)).To(typeof(DefaultRepository<>));
            Bind<SchemaRepository>().To<SchemaRepositoryImpl>().InTransientScope();
            Bind<StoredProcedureRepository>().To<StoredProcedureRepositoryImpl>().InTransientScope();
            Bind<DBTypeConverter>().To<MSSQLTypeConverter>().InTransientScope();

        }
    }
}
