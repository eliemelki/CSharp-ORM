using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Expressions;
using System.Data.SqlClient;
using DBLibrary.QueryEngine.Query;

namespace DBLibrary.QueryEngine.Criteria
{

    public interface SqlCriteria : SqlString
    {
        SqlCriteria Add(Expression anExpression);
        SqlCriteria AddOrder(Order anOrder);
        SqlCriteria AddSql(string anSqlString);
        SqlCriteria AddGroupBy(Group aGroupBy);
        List<SqlParameter> Paremeters { get; }
    }

    class CriteriaImpl : SqlCriteria
    {
        private Orders Orders { set; get; }
        private Group GroupBy { set; get; }
        private int count { set; get; }

        protected StringBuilder CriteriaBuilder { set; get; }

        public CriteriaImpl()
        {
            CriteriaBuilder = new StringBuilder();
            Paremeters = new List<SqlParameter>();
            Orders = new Orders();
            GroupBy = new Group();
        }

        private void CheckBeforeAdd()
        {
            if (count > 0)
            {
                CriteriaBuilder.AppendFormat(SqlSyntax.SPACE);
                CriteriaBuilder.AppendFormat(SqlSyntax.AND);
                CriteriaBuilder.AppendFormat(SqlSyntax.SPACE);
            }

            else
            {
                CriteriaBuilder.AppendFormat(SqlSyntax.SPACE);
                CriteriaBuilder.AppendFormat(SqlSyntax.WHERE);
                CriteriaBuilder.AppendFormat(SqlSyntax.SPACE);
            }
            count++;
        }

        public SqlCriteria Add(Expression anExpression)
        {
            CheckBeforeAdd();
            anExpression.AppendSqlString(this.CriteriaBuilder, this.Paremeters);
            return this;
        }

        public SqlCriteria AddOrder(Order anOrder)
        {
            Orders.Add(anOrder);
            return this;
        }

        public SqlCriteria AddSql(string anSqlString)
        {
            if (count <= 0)
            {
                CriteriaBuilder.AppendFormat(SqlSyntax.SPACE);
                CriteriaBuilder.AppendFormat(SqlSyntax.WHERE);
                CriteriaBuilder.AppendFormat(SqlSyntax.SPACE);
            }

            CriteriaBuilder.AppendFormat(SqlSyntax.SPACE);
            CriteriaBuilder.Append(anSqlString);
            CriteriaBuilder.AppendFormat(SqlSyntax.SPACE);
            return this;
        }

        public List<SqlParameter> Paremeters
        {
            get;
            private set;
        }

        public string GetSqlString()
        {
            StringBuilder _queryBuilder = new StringBuilder();
            _queryBuilder.Append(CriteriaBuilder.ToString());
            _queryBuilder.Append(GroupBy.GetSqlString());
            _queryBuilder.Append(Orders.GetSqlString());
            return _queryBuilder.ToString();
        }


        public SqlCriteria AddGroupBy(Group aGroupBy)
        {
            GroupBy = aGroupBy;
            return this;
        }
    }
}
