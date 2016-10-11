using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query.Queries;
using DBLibrary.QueryEngine.Query.GenericSqlQuery;
using DBLibrary.Configuration;
using DBLibrary.Mapper;
namespace DBLibrary.QueryEngine.Query
{

    public interface QueryFactory
    {
        SelectQuery CreateSelectQuery();
        InsertQuery CreateInsertQuery();
        UpdateQuery CreateUpdateQuery();
        DeleteQuery CreateDeleteQuery();
        NativeQuery CreateNativeQuery();
        NativeStoredProcedureQuery CreateNativeStoredProcedureQuery();
        InsertQuery<T> CreateInsertQuery<T>(T aData, bool isNullBind) where T : class, new();
        UpdateQuery<T> CreateUpdateQuery<T>(T aData, bool isNullBind) where T : class, new();
        DeleteQuery<T> CreateDeleteQuery<T>() where T : class, new();
        SelectQuery<T> CreateSelectQuery<T>() where T : class, new();
       // DependencyQuery<T> CreateDepdendencyQuery<T>() where T : class, new();
    }

    class QueryFactoryImpl : QueryFactory
    {
        private PropertyBinder Binder;
        private ValueExtractor ValueExtractor;

        public QueryFactoryImpl(PropertyBinder aBinder, ValueExtractor aValueExtrator)
        {
            Binder = aBinder;
            ValueExtractor = aValueExtrator;
        }
   

        public SelectQuery CreateSelectQuery()
        {
            return new SelectQuery();
        }

        public InsertQuery CreateInsertQuery()
        {
            return new InsertQuery();
        }

        public UpdateQuery CreateUpdateQuery()
        {
            return new UpdateQuery();
        }

        public DeleteQuery CreateDeleteQuery()
        {
            return new DeleteQuery();
        }

        public NativeQuery CreateNativeQuery()
        {
            return new NativeQuery();
        }

        public InsertQuery<T> CreateInsertQuery<T>(T aData, bool isNullBind) where T : class, new()
        {
            return new InsertQuery<T>(Binder, aData, isNullBind, ValueExtractor);
        }

        public UpdateQuery<T> CreateUpdateQuery<T>(T aData, bool isNullBind) where T : class, new()
        {
            return new UpdateQuery<T>(Binder, aData, isNullBind, ValueExtractor);
        }

        public DeleteQuery<T> CreateDeleteQuery<T>() where T : class, new()
        {
            return new DeleteQuery<T>(Binder);
        }

        public SelectQuery<T> CreateSelectQuery<T>() where T : class, new()
        {
            return new SelectQuery<T>(Binder);
        }

        //public DependencyQuery<T> CreateDepdendencyQuery<T>() where T : class, new()
        //{
        //    return new DependencyQuery<T>(Binder);
        //}


        public NativeStoredProcedureQuery CreateNativeStoredProcedureQuery()
        {
            return new NativeStoredProcedureQuery();
        }
    }
}
