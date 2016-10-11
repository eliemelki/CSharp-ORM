using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBLibrary.Session;
using DBLibrary.QueryEngine.Criteria;

namespace DBLibrary.Repository
{
    public interface TableRepository<T> : Repository<T> where T :  class, new()
    {
        int Delete(Object anId);
        int Delete(T aModel);
        int Delete(params T[] aModels);
        int Delete(SqlCriteria aCriteria);
        int DeleteAll();


        void Insert(T aModel, bool aWithNullValues = false);
        int Update(T aModel, SqlCriteria aCriteria, bool aWithNullValues = false);
        int Update(T aModel, T[] aData, bool aWithNullValues = false);
        int Update(T aModel, bool aWithNullValues = false);
        State Save(T aModel, bool aWithNullValues = false);
    }
}