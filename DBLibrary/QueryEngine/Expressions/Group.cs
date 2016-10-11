using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;

namespace DBLibrary.QueryEngine.Expressions
{
    public class Group : SqlString
    {
        private List<String> Fields;

        public Group()
        {
            Fields = new List<string>();
        }

        public Group(params String[] aFields)
        {
            Fields = new List<string>();
            Fields.AddRange(aFields);
        }

        public Group(List<String> aFields)
        {
            Fields = aFields;
        }

        public String GetSqlString()
        {
            int count = Fields.Count();

            if (Fields.Count() <= 0)
                return "";

            StringBuilder builder = new StringBuilder();
            builder.Append(SqlSyntax.GROUP);
            for (int i = 0; i < count; i++)
            {

                builder.Append(Fields[i]);
                if (i != count - 1)
                {
                    builder.Append(SqlSyntax.COMMA);
                }
            }

            return builder.ToString();
        }
    }
}
