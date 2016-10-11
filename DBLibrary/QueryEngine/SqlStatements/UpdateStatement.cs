using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;
using System.Data.SqlClient;

namespace DBLibrary.QueryEngine.Statements
{
    public class UpdateStatement : ParemeterStatement
    {

        protected override String GetSqlFromat()
        {
            return SqlSyntax.STATEMENT_UPDATE;
        }

        protected override String GetParemetrized()
        {
            // Field=@Field1 ,Field2,...

           
            StringBuilder _builder = new StringBuilder();

            int i = 0;
            Dictionary<String, Object>.Enumerator enumerator = FieldValue.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<String, Object> keyValue = enumerator.Current;

                //Field=@Field1 ,
                _builder.Append(SqlSyntax.SPACE);
                _builder.Append(keyValue.Key);
                _builder.Append(SqlSyntax.EQ);
                _builder.Append(ParemeterHelper.GetParameterString(ParemeterCount));
                if (i != FieldValue.Count - 1)
                {
                    _builder.Append(SqlSyntax.COMMA);
                }
                AddExpressionParameters(keyValue.Value);
                i++;
            }

            return _builder.ToString();
        }
    }
}
