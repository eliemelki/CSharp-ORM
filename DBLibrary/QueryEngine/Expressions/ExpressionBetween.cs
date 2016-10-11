using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DBLibrary.QueryEngine.Query;
namespace DBLibrary.QueryEngine.Expressions
{
    public class ExpressionBetween : ExpressionParameter
    {
        public override String Property { get; protected set; }
        public override object[] Value { get; protected set; }
        
        private String Op1;
        private String Op2;

        public ExpressionBetween(String aProperty, String anOp1, Object aValue1, String anOp2, Object aValue2)
        {
            Property = aProperty;
            Op1 = anOp1;
            Op2 = anOp2;
            Value = new Object[] { aValue1, aValue2 };
        }

        public override void AppendSqlString(StringBuilder aBuilder,List<SqlParameter> aParameters)
        {
            //(
            aBuilder.Append(SqlSyntax.LEFT_PARENTHESE);
            
            //({0} op @{1})
            AppendSql(aBuilder, aParameters, Op1, Value[0]);
            
            //And
            aBuilder.Append(SqlSyntax.AND);
            
            //({0} op @{1})
            AppendSql(aBuilder, aParameters, Op2, Value[1]);
            
            //)
            aBuilder.Append(SqlSyntax.RIGHT_PARENTHESE);

        }

    }
}
