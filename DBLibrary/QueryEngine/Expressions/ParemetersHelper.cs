using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DBLibrary.QueryEngine.Query;

namespace DBLibrary.QueryEngine.Expressions
{
    class ParemetersHelperFactory
    {
        public static ParemetersHelper GetParemeterHelper()
        {
            return new ParemetersHelperImpl();
        }
    }

    public interface ParemetersHelper
    {
        SqlParameter GetSqlParemeter(int aParemeterCount, Object aValue);
        SqlParameter GetSqlParemeter(String aParamater, Object aValue);
        String GetParameterString(int aCount);
    
    }

    class ParemetersHelperImpl : ParemetersHelper
    {

        public SqlParameter GetSqlParemeter(String aParamater, Object aValue)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = aParamater;
    
            if (aValue == null)
            {
                aValue = DBNull.Value;
            }

            param.SqlValue = aValue;
            return param;
        }
        public SqlParameter GetSqlParemeter(int aParemeterCount, Object aValue)
        {
            String _parameterName = SqlSyntax.PAREMETER_CHARACTER + aParemeterCount;
            return GetSqlParemeter(_parameterName,aValue);
        }


        public String GetParameterString(int aCount)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(SqlSyntax.AT);
            builder.Append(SqlSyntax.PAREMETER_CHARACTER);
            builder.Append(aCount);
            return builder.ToString();
        }
    }
}
