using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.Mapper;
using DBLibrary.QueryEngine.Statements;
using DBLibrary.QueryEngine.Query.Queries;
using System.Data.SqlClient;
using DBLibrary.Mapper.ResultBinder;

namespace DBLibrary.QueryEngine.Query.GenericSqlQuery
{

    public abstract class GenericAbstractQuery<T, Q, S> : SqlQuery
        where T : class ,new()
        where Q : AbstractQuery<Q,S> , new()
        where S : AbstractStatement , new()
    {
        protected PropertyBinder PropertyBinder;
        protected String tableName;
        protected Q query;


        public GenericAbstractQuery(PropertyBinder aPropertyBinder)
        {
            PropertyBinder = aPropertyBinder;
            query = new Q();
            tableName = PropertyBinder.GetTable<T>();    
            query.SetStatement( m => m.AddFrom(String.Format(SqlSyntax.DBTABLE,tableName)));
        }

        public virtual string Query
        {
            get { return query.Query; }
        }

        public List<SqlParameter> Parameters
        {
            get { return query.Parameters; }
        }

        public override string ToString()
        {
            return query.ToString();
        }
    }
}
