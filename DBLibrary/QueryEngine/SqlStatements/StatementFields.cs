using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;
using System.Data.SqlClient;

namespace DBLibrary.QueryEngine.Statements
{
    public abstract class SqlStatementFields : AbstractStatement
    {
        protected List<String> Fields { set; get; }

        public SqlStatementFields()
        {
            Fields = new List<string>();
        }

        public virtual SqlStatementFields AddField(String aField)
        {
            Fields.Add(aField);
            return this;
        }

        public virtual SqlStatementFields AddField(params String[] aFields)
        {
            Fields.AddRange(aFields);
            return this;
        }

        protected override String GetSqlFromat()
        {
            return SqlSyntax.STATEMENT_SELECT;
        }

        protected abstract String GetDefaultField();

        private String GetFields()
        {
            StringBuilder _builder = new StringBuilder();
            if (Fields.Count <= 0)
            {
                _builder.Append(GetDefaultField());
            }
            else
            {
                int i = 0;
                var _fields = Fields.Distinct();
                foreach (String field in _fields)
                {
                    _builder.Append(field);
                    if (i != _fields.Count() - 1)
                    {
                        _builder.Append(SqlSyntax.COMMA);
                    }
                    i++;
                }
            }
            return _builder.ToString();
        }


        protected override void GetData(List<Object> aData)
        {
            aData.Add(GetFields());
            aData.Add(From);
        }
    }
}
