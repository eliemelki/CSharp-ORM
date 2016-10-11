using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DBLibrary.QueryEngine.Expressions;
using DBLibrary.Categories;

namespace DBLibrary.QueryEngine.Query.Queries
{
   
    public class NativeStoredProcedureQuery : SqlQuery
    {
        private String ProcedureName { set; get; }
        public String Query
        {
            get
            {
                String _q = String.Format(SqlSyntax.STORED_PROECDURE, ProcedureName);
                foreach (SqlParameter entry in this.Parameters)
                {
                    _q += String.Format(" @{0} = @{0}", entry.ParameterName);
                    _q += ",";
                }
                return _q.RemoveLastCharacter(",");
            }
        }

        public List<SqlParameter> Parameters { get; set; }

        public NativeStoredProcedureQuery()
        {
            Parameters = new List<SqlParameter>();      
        }

 
        public static SqlQuery GetSQLQuery(String aStoreProcedurName, Dictionary<String,Object> aParamaters)
        {
            ParemetersHelper helper = ParemetersHelperFactory.GetParemeterHelper();
            NativeStoredProcedureQuery query = new NativeStoredProcedureQuery();
            query.ProcedureName = aStoreProcedurName;

            foreach (KeyValuePair<string, object> entry in aParamaters)
            {
                var sqlparam = helper.GetSqlParemeter(entry.Key, entry.Value);

                query.Parameters.Add(sqlparam);
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

