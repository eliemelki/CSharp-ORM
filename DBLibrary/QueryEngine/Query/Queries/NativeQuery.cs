using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DBLibrary.QueryEngine.Expressions;

namespace DBLibrary.QueryEngine.Query.Queries
{
    public class NativeQuery : SqlQuery
    {
        public string Query { get; set; }
        public List<SqlParameter> Parameters { get; set; }

        public NativeQuery()
        {
            Parameters = new List<SqlParameter>();      
        }
        
        //Assuming @p0, @p1 is the paremeters
        public static SqlQuery SimpleQueryHelper(String aQuery, params object[] aValues)
        {
            NativeQuery query = new NativeQuery();
            query.Query = aQuery;
            for (int i = 0; i < aValues.Length; i++)
            {
                ParemetersHelper helper = ParemetersHelperFactory.GetParemeterHelper();
                query.Parameters.Add(helper.GetSqlParemeter(query.Parameters.Count, aValues[i]));
            }

            return query;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("[");
            result.Append(Query);
            result.Append("]");
            result.AppendLine();
            result.Append("[");
            foreach (System.Data.SqlClient.SqlParameter paremeter in this.Parameters)
            {
                result.Append(SqlSyntax.AT);
                result.Append(paremeter.ParameterName);
                result.Append(SqlSyntax.EQ);
                result.Append(paremeter.SqlValue);
                result.Append(SqlSyntax.SPACE);
            }
            result.Append("]");
            return result.ToString();
        }
    }
}
