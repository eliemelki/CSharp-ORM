using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DBLibrary.QueryEngine.Query;

namespace DBLibrary.QueryEngine.Expressions
{
    public abstract class ExpressionLogical : AbstractExpression
    {
        private Expression[] Expressions;


        public ExpressionLogical(params Expression[] aExpressions)
        {
            Expressions = aExpressions;
        }

        public abstract String Op();

        public override void AppendSqlString(StringBuilder aBuilder, List<SqlParameter> aParameters)
        {
            //(sql op sql)

            aBuilder.Append(SqlSyntax.LEFT_PARENTHESE);
            for (int i = 0; i < Expressions.Count() - 1; i++)
            {
                Expressions[i].AppendSqlString(aBuilder, aParameters);
                aBuilder.Append(SqlSyntax.SPACE);
                aBuilder.Append(Op());
                aBuilder.Append(SqlSyntax.SPACE);
            }
            Expressions[Expressions.Count() -1].AppendSqlString(aBuilder, aParameters);
            aBuilder.Append(SqlSyntax.RIGHT_PARENTHESE);
        }
    }


    public class AndExpression : ExpressionLogical
    {
        public AndExpression(params Expression[] aExpressions)
            : base(aExpressions)
        {

        }
        public override String Op()
        {
            return SqlSyntax.AND;
        }
    }

    public class OrExpression : ExpressionLogical
    {
        public OrExpression(params Expression[] aExpressions)
            : base(aExpressions)
        {

        }
        public override String Op()
        {
            return SqlSyntax.OR;
        }
    }

    public class NotExpression : AbstractExpression
    {
        private Expression Expression1;
        public NotExpression(Expression anExpression1)
        {
            Expression1 = anExpression1;
        }

        public override void AppendSqlString(StringBuilder aBuilder, List<SqlParameter> aParameters)
        {
            //(Not (sqlstring))

            aBuilder.Append(SqlSyntax.LEFT_PARENTHESE);
            aBuilder.Append(SqlSyntax.NOT);
            aBuilder.Append(SqlSyntax.LEFT_PARENTHESE);
            Expression1.AppendSqlString(aBuilder, aParameters);
            aBuilder.Append(SqlSyntax.RIGHT_PARENTHESE);
            aBuilder.Append(SqlSyntax.RIGHT_PARENTHESE);
        }
    }
}