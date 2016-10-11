    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.Mapper;
using System.Data.SqlClient;
using System.Data;
using DBLibrary.Configuration;
using DBLibrary.Mapper.ResultBinder;
namespace DBLibrary.QueryEngine.Query
{
    public interface SqlQuery
    {
        String Query { get; }
        List<SqlParameter> Parameters { get; }
    }

    public interface SqlString
    {
        String GetSqlString();
    }

    class SqlSyntax
    {

        public const String DBTABLE = "[dbo].[{0}]";

        public const String DBFIELD = DBTABLE + ".[{1}]";

        public const String COUNT_QUERY = "Count(*)";
        public const String COUNT_ID_QUERY = "Count({0})";


        public const String STATEMENT_DBFIELD = DBFIELD + " as {0}_{1}";

        public const String PAREMETER_CHARACTER = "p";


        public const String DEFAULT_FIELD = "*";

        public const String From = "From";
        public const String SELECT = "Select";
        public const String TOP = " Top({0})";
        public const String SELECT_TOP = SELECT + TOP;

        public const String WHERE = "Where";
        public const String ROWCOUNT = ";select @@ROWCOUNT;";

        public const String SPACE = " ";
        public const String EQ = "=";
        public const String LIKE = "like";
        public const String ISNULL = "is null";

        public const String AND = "AND";
        public const String OR = "OR";
        public const String NOT = "NOT";

        public const String LT = "<";
        public const String GR = ">";

        public const String LTE = "<=";
        public const String GRE = ">=";

        public const String IN = "IN";

        public const String LEFT_PARENTHESE = "(";
        public const String RIGHT_PARENTHESE = ")";
        public const String COMMA = ",";
        public const String AT = "@";

        public const String VALUES = "VALUES";

        public const String ORDER = " Order by ";

        public const String GROUP = " Group by ";

        public const String STATEMENT_UPDATE = "Update {0} set {1}";
        public const String STATEMENT_DELETE = "Delete From {0}";
        public const String STATEMENT_INSERT = "Insert Into {0} {1};Select @@Identity {2}";
        public const String STATEMENT_SELECT = "Select {0} From {1}";
        public const String STORED_PROECDURE = "EXEC {0}";
    }
}
