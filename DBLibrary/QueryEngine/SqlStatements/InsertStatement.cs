using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;
using System.Data.SqlClient;
namespace DBLibrary.QueryEngine.Statements
{
    public class InsertStatement : ParemeterStatement
    {
        private String Identity;

        public InsertStatement AddIdentity(String anIdentity)
        {
            Identity = anIdentity;
            return this;
        }
        protected override String GetSqlFromat()
        {
            return SqlSyntax.STATEMENT_INSERT;
        }

        protected override void GetData(List<Object> aData)
        {
            base.GetData(aData);
            if (!String.IsNullOrEmpty(Identity))
            {
                aData.Add(" as " + Identity);
            }
        }

        protected override String GetParemetrized()
        {
            // (Field1,Field2,...) VALUES(@Field1,@Field2,@Field3)

            //Values(

           
            StringBuilder _builder = new StringBuilder();
            StringBuilder _paremeterBuilder = new StringBuilder();
            _paremeterBuilder.Append(SqlSyntax.VALUES);
            _paremeterBuilder.Append(SqlSyntax.LEFT_PARENTHESE);

            int i = 0;
            //(
            _builder.Append(SqlSyntax.LEFT_PARENTHESE);
            Dictionary<String, Object>.Enumerator enumerator = FieldValue.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<String, Object> keyValue = enumerator.Current;

                //@Field1,
                _paremeterBuilder.Append(ParemeterHelper.GetParameterString(ParemeterCount));


                //Field
                _builder.Append(keyValue.Key);

                if (i != FieldValue.Count - 1)
                {
                    _paremeterBuilder.Append(SqlSyntax.COMMA);
                    _builder.Append(SqlSyntax.COMMA);
                }
                AddExpressionParameters(keyValue.Value);
                i++;
            }

            _paremeterBuilder.Append(SqlSyntax.RIGHT_PARENTHESE);
            _paremeterBuilder.Append(SqlSyntax.SPACE);

            _builder.Append(SqlSyntax.RIGHT_PARENTHESE);
            _builder.Append(SqlSyntax.SPACE);

            _builder.Append(_paremeterBuilder);
            return _builder.ToString();
        }
    }
}
