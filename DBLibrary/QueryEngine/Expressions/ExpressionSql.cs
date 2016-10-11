using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DBLibrary.QueryEngine.Query;

namespace DBLibrary.QueryEngine.Expressions
{
    public class ExpressionSql : AbstractExpression
    {
        private String SqlString;

        public ExpressionSql(String anSqlString)
        {
            SqlString = anSqlString;
        }

        public override void AppendSqlString(StringBuilder aBuilder, List<SqlParameter> aParameters)
        {
            aBuilder.Append(SqlString);
        }
    }
}