using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Statements;
using DBLibrary.QueryEngine.Query.Queries;
using System.Reflection;
using DBLibrary.Mapper;
using DBLibrary.Mapper.ResultBinder;
using DBLibrary.QueryEngine.Criteria;

namespace DBLibrary.QueryEngine.Query.GenericSqlQuery
{
    public class SelectQuery<T> :
        GenericAbstractQuery<T, SelectQuery, SelectStatement>,
        HasMaximum<SelectQuery<T>>,
        HasMinimum<SelectQuery<T>>,
        HasCriteria<SelectQuery<T>>,
        ISelectQuery
        where T : class , new()
    {
        public SelectQuery(PropertyBinder aBinder)
            : base(aBinder)
        {
            PropertyBinder.BindProperty<T>(

             delegate(PropertyMap aMap)
             {
                 query.GetStatement().AddField(
                     String.Format(SqlSyntax.DBFIELD, tableName, aMap.GetColumn()));
             },
             delegate(IdentityMap aMap)
             {
                 query.GetStatement().AddField(
                   String.Format(SqlSyntax.DBFIELD, tableName, aMap.GetColumn()));
             },
             delegate(PropertyMap aMap,MemberInfo[] aParentsMembers)
             {
                 query.GetStatement().AddField(
                     String.Format(SqlSyntax.DBFIELD, tableName, aMap.GetColumn()));
             });
        }

        public SelectQuery<T> SetCache(CachePreference aCache)
        {
            query.SetCache(aCache);
            return this;
        }

        public CachePreference GetCachePreferences()
        {
            return query.GetCachePreferences();
        }

        public SelectQuery<T> SetMaximum(int aMaximum)
        {
            query.SetMaximum(aMaximum);
            return this;
        }


        public int GetMaximum()
        {
            return query.GetMaximum();
        }

        public SelectQuery<T> SetCriteria(Action<SqlCriteria> anAction)
        {
            query.SetCriteria(anAction);
            return this;
        }

        public SelectQuery<T> SetCriteria(SqlCriteria aCriteria)
        {
            query.SetCriteria(aCriteria);
            return this;
        }

        public SelectQuery<T> SetMinimum(int aMinimum)
        {
            query.SetMinimum(aMinimum);
            return this;
        }

        public int GetMinimum()
        {
            return query.GetMinimum();
        }

        public SqlCriteria GetCriteria()
        {
            return query.GetCriteria();
        }
    }
}
