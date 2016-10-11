using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DBLibrary.QueryEngine.Query;

namespace DBLibrary.QueryEngine.Expressions
{
    public abstract class ExpressionParameter : AbstractExpression
    {
        public virtual String Property { get; protected set; }
        public virtual Object[] Value { get; protected set; }

        protected void AddExpressionParameters(List<SqlParameter> aParameters, Object aValue)
        {
            ParemetersHelper helper =  ParemetersHelperFactory.GetParemeterHelper();
            aParameters.Add(helper.GetSqlParemeter(aParameters.Count,aValue));
        }

        protected void AppendSql(StringBuilder aBuilder, List<SqlParameter> aParameters, String Op, Object aValue)
        {
            //property Op @property[X]
            aBuilder.Append(SqlSyntax.LEFT_PARENTHESE);
            aBuilder.Append(Property);
            aBuilder.Append(SqlSyntax.SPACE);
            aBuilder.Append(Op);
            aBuilder.Append(SqlSyntax.SPACE);
            ParemetersHelper helper = ParemetersHelperFactory.GetParemeterHelper();
            aBuilder.Append(helper.GetParameterString(aParameters.Count));
            aBuilder.Append(SqlSyntax.RIGHT_PARENTHESE);
            
            //adding paremeters
            AddExpressionParameters(aParameters, aValue);
        }

    }
}
