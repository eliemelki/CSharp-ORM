using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;
using System.Data.SqlClient;

namespace DBLibrary.QueryEngine.Expressions
{
    public class ExpressionBoolean : ExpressionParameter
    {
        private String Op;
        private String FormatFunction;

        public ExpressionBoolean(String aProperty, String anOp, String aFormatFunction, params Object[] aValues)
        {
            Property = aProperty;
            Op = anOp;
            FormatFunction = aFormatFunction;
            this.Value = aValues;
        }

        public override void AppendSqlString(StringBuilder aBuilder, List<SqlParameter> aParameters)
        {
            //property Op @property[X]
            aBuilder.Append(SqlSyntax.LEFT_PARENTHESE);
            aBuilder.Append(Property);
            aBuilder.Append(SqlSyntax.SPACE);
            aBuilder.Append(Op);
            aBuilder.Append(SqlSyntax.SPACE);
            ParemetersHelper helper = ParemetersHelperFactory.GetParemeterHelper();
            aBuilder.AppendFormat(FormatFunction,
                helper.GetParameterString(aParameters.Count));
            aBuilder.Append(SqlSyntax.RIGHT_PARENTHESE);

            foreach (Object value in Value)
            {
                AddExpressionParameters(aParameters, value);
            }
        }
    }
}
