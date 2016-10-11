using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Statements;
using DBLibrary.QueryEngine.Criteria;
using DBLibrary.QueryEngine.Expressions;
using System.Data.SqlClient;
using DBLibrary.Mapper.ResultBinder;
namespace DBLibrary.QueryEngine.Query.Queries
{

    public abstract class AbstractQuery<Q, S> : SqlQuery, HasStatement<Q, S>
        where S : AbstractStatement, new()
        where Q : SqlQuery
    {

        protected abstract Q GetThis();
        protected S Statement;
        protected SqlCriteria Criteria;

        private StringBuilder QueryBuilder { get; set; }


        public Q SetStatement(Action<S> anAction)
        {
            anAction(Statement);
            return GetThis();

        }

        internal S GetStatement()
        {
            return Statement;
        }

        public AbstractQuery()
        {
            QueryBuilder = new StringBuilder();
            Criteria = DBLibrary.Database.Current.Factory.CreateCriteria();
            Statement = DBLibrary.Database.Current.Factory.CreateStatement<S>();
        }


        public virtual string Query
        {
            get
            {

                StringBuilder _builder = new StringBuilder();
                _builder.Append(Statement.GetSqlString(Criteria.Paremeters.Count));
                _builder.Append(Criteria.GetSqlString());
                return _builder.ToString();
            }
        }

        public virtual List<System.Data.SqlClient.SqlParameter> Parameters
        {
            get
            {
                List<SqlParameter> paremeter = new List<SqlParameter>();
                paremeter.AddRange(Criteria.Paremeters);
                paremeter.AddRange(Statement.Parameters);
                return paremeter;
            }
        }



        public override string ToString()
        {
            ParemetersHelper helper = ParemetersHelperFactory.GetParemeterHelper();
            StringBuilder result = new StringBuilder();
            result.Append("[");
            result.Append(Query);
            result.Append("]");
            result.AppendLine();
            result.Append("[");
            foreach (System.Data.SqlClient.SqlParameter paremeter in this.Parameters)
            {
                result.Append(SqlSyntax.AT);
                result.Append(paremeter.ParameterName);
                result.Append(SqlSyntax.EQ);
                result.Append(paremeter.SqlValue);
                result.Append(SqlSyntax.SPACE);
            }
            result.Append("]");
            return result.ToString();
        }


        public Q SetStatement(S aStatement)
        {
            Statement = aStatement;
            return GetThis() ;
        }
    }

    public abstract class CriteriaQuery<Q, S> : AbstractQuery<Q, S>, HasCriteria<Q>
        where S : AbstractStatement, new()
        where Q : SqlQuery
    {
        public Q SetCriteria(Action<SqlCriteria> anAction)
        {
            anAction(Criteria);
            return GetThis();
        }

        public SqlCriteria GetCriteria()
        {
            return Criteria;
        }

        public Q SetCriteria(SqlCriteria aCriteria)
        {
            Criteria = aCriteria;
            return GetThis();
        }
    }
}
