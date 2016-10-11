using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query.Queries;
using DBLibrary.Mapper;
using DBLibrary.QueryEngine.Statements;
using System.Reflection;
using DBLibrary.Mapper.ResultBinder;
using DBLibrary.QueryEngine.Criteria;
namespace DBLibrary.QueryEngine.Query.GenericSqlQuery
{
    public class DeleteQuery<T> : GenericAbstractQuery<T, DeleteQuery, DeleteStatement>, HasCriteria<DeleteQuery<T>>
     where T : class, new()
    {

        internal DeleteQuery(PropertyBinder aBinder)
            : base(aBinder)
        {
        }

        public DeleteQuery<T> SetCriteria(Action<SqlCriteria> anAction)
        {
            anAction(query.GetCriteria());
            return this;
        }

        public Criteria.SqlCriteria GetCriteria()
        {
            return query.GetCriteria();
        }

        public DeleteQuery<T> SetCriteria(SqlCriteria aCriteria)
        {
            query.SetCriteria(aCriteria);
            return this;
        }
        public override string Query
        {
            get { return query.Query + SqlSyntax.ROWCOUNT; }
        }
    }

    public abstract class InsertUpdateQuery<T, Q, S> : GenericAbstractQuery<T, Q, S>
        where T : class ,new()
        where Q : AbstractQuery<Q, S>, new()
        where S : ParemeterStatement , new()
    {
        internal InsertUpdateQuery(PropertyBinder aBinder, T aData, bool isNullBind, ValueExtractor anExtractor)
            : base(aBinder)
        {
            ValueBinder valuebinder = Database.Current.Factory.GetValueBinder();
            aBinder.BindProperty<T>(

                delegate(PropertyMap aMap)
                {
                    valuebinder.BindValue(query.GetStatement(), aMap, aData,isNullBind);
                },
                delegate(IdentityMap aMap)
                {
                    setIdentity(aMap.GetColumn());
                },
                delegate(PropertyMap aMap, MemberInfo[] aParentsMembers)
                {
                    var field =  anExtractor.GetValue(aData,aParentsMembers);
                    valuebinder.BindValue(query.GetStatement(), aMap, field, isNullBind);
                });
        }

        protected abstract void setIdentity(String aColumn);
    }

    public class InsertQuery<T> : InsertUpdateQuery<T, InsertQuery, InsertStatement>
    where T : class, new()
    {
        internal InsertQuery(PropertyBinder aBinder, T aData, bool isNullBind, ValueExtractor anExtractor)
            : base(aBinder, aData, isNullBind, anExtractor)
        {

        }

        protected override void setIdentity(String aColumn)
        {
            query.SetStatement(m => m.AddIdentity(aColumn));
        }
    }

    public class UpdateQuery<T> : InsertUpdateQuery<T, UpdateQuery, UpdateStatement>, HasCriteria<UpdateQuery<T>>
    where T : class, new()
    {
        internal UpdateQuery(PropertyBinder aBinder, T aData, bool isNullBind, ValueExtractor anExtractor)
            : base(aBinder, aData, isNullBind, anExtractor)
        {

        }


        protected override void setIdentity(String aColumn)
        {

        }

        public UpdateQuery<T> SetCriteria(Action<SqlCriteria> anAction)
        {
            anAction(query.GetCriteria());
            return this;
        }


        public Criteria.SqlCriteria GetCriteria()
        {
            return query.GetCriteria();
        }


        public UpdateQuery<T> SetCriteria(SqlCriteria aCriteria)
        {
            query.SetCriteria(aCriteria);
            return this;
        }

        public override string Query
        {
            get { return query.Query + SqlSyntax.ROWCOUNT; }
        }
    }
}

