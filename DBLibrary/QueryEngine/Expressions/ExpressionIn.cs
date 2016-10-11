using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DBLibrary.QueryEngine.Query;

namespace DBLibrary.QueryEngine.Expressions
{
    public class ExpressionIn : ExpressionParameter
    {
        public override String Property { get; protected set; }
        public override Object[] Value { get; protected set; }

        public ExpressionIn(String aProperty, Object[] aValue)
        {
            Property = aProperty;
            Value = aValue;
        }

        public override void AppendSqlString(StringBuilder aBuilder, List<SqlParameter> aParameters)
        {
            //({0} IN (@{0},@{1},...))

            //({0} IN (
            aBuilder.Append(SqlSyntax.LEFT_PARENTHESE);
            aBuilder.Append(Property);
            aBuilder.Append(SqlSyntax.SPACE);         
            aBuilder.Append(SqlSyntax.IN);
            aBuilder.Append(SqlSyntax.SPACE);
            aBuilder.Append(SqlSyntax.LEFT_PARENTHESE);
            ParemetersHelper helper = ParemetersHelperFactory.GetParemeterHelper();
            //@{1},@{2},...
            for (int i = 0; i < Value.Length; i++)
            {
                aBuilder.Append(helper.GetParameterString(aParameters.Count));
                if (i != Value.Length - 1)
                {
                    aBuilder.Append(SqlSyntax.COMMA);
                }
                AddExpressionParameters(aParameters, Value[i]);
            }

            //))
            aBuilder.Append(SqlSyntax.RIGHT_PARENTHESE);
            aBuilder.Append(SqlSyntax.RIGHT_PARENTHESE);
        }
    }
}
