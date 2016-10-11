using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;
using System.Data.SqlClient;

namespace DBLibrary.QueryEngine.Expressions
{
    public class ExpressionSimple : ExpressionParameter
    {
        protected String Op;

        public override String Property { get; protected set; }
        public override Object[] Value { get; protected set; }

        public ExpressionSimple(String anOp, String aProperty, Object aValue)
        {
            Op = anOp;
            Property = aProperty;
            Value = new Object[] { aValue };
        }

        public override void AppendSqlString(StringBuilder aBuilder, List<SqlParameter> aParameters)
        {
            foreach (Object value in Value)
            {
                AppendSql(aBuilder, aParameters, Op, value);
            }
        }
    }
}
