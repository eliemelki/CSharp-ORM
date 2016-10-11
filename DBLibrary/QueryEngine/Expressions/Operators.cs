using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;

namespace DBLibrary.QueryEngine.Expressions
{
    public class OpHelper
    {
        public readonly static LogicalOperator EQ = new LogicalOperator("=");
        public readonly static LogicalOperator LT = new LogicalOperator("<");
        public readonly static LogicalOperator GR = new LogicalOperator(">");
        public readonly static LogicalOperator LTE = new LogicalOperator("<=");
        public readonly static LogicalOperator GRE = new LogicalOperator(">=");
        public readonly static LogicalOperator NotEq = new LogicalOperator("<>");
        public readonly static LogicalOperator IN = new LogicalOperator("In");
        public readonly static LogicalOperator LIKE = new LogicalOperator("Like");

        public readonly static BooleanOperator AND = new BooleanOperator("And");
        public readonly static BooleanOperator OR = new BooleanOperator("Or");
        public readonly static BooleanOperator NOT = new BooleanOperator("Not");


        public readonly static SqlOperator WHERE = new SqlOperator("where");
        public readonly static SqlOperator SELECT = new SqlOperator("Select");
        public readonly static SqlOperator FROM = new SqlOperator("From");
        public readonly static SqlOperator JOIN = new SqlOperator("Join");
        public readonly static SqlOperator VALUES = new SqlOperator("Values");
        public readonly static SqlOperator ORDERBY = new SqlOperator("Order By");
        public readonly static SqlOperator GROUPBY = new SqlOperator("Group By");
        public readonly static SqlOperator DISTINCT = new SqlOperator("Distinct");
        public readonly static SqlOperator ROWCOUNT = new SqlOperator(";select @@ROWCOUNT;");
        public readonly static SqlOperator DEFAULT_ORDER = new SqlOperator("CURRENT_TIMESTAMP");

        public readonly static NonOperator COMMA = new NonOperator(",");
        public readonly static NonOperator SPACE = new NonOperator(" ");
        public readonly static NonOperator LEFT_P = new NonOperator("(");
        public readonly static NonOperator RIGHT_P = new NonOperator(")");
        public readonly static NonOperator STAR = new NonOperator("*");
        public readonly static NonOperator INTO = new NonOperator("INTO");
        public readonly static NonOperator PAREMETER = new NonOperator("p");
        public readonly static NonOperator LEFT_B = new NonOperator("[");
        public readonly static NonOperator RIGHT_B = new NonOperator("]");


        public readonly static SortOperator ASC = new SortOperator("ASC");
        public readonly static SortOperator DESC = new SortOperator("DESC");


        public readonly static SingleParameterOperator COUNT = new SingleParameterOperator("Count", "Count({0})");
        public readonly static SingleParameterOperator TOP = new SingleParameterOperator("Top", "Top({0})");
        public readonly static SingleParameterOperator AT = new SingleParameterOperator("@", String.Format("@{0}{1})", PAREMETER, "{0}"));
        public readonly static SingleParameterOperator TABLE = new SingleParameterOperator("[dbo]", "[dbo].[{0}]");

        public readonly static JoinOperator INNERJOIN = new JoinOperator("Inner Join", "{0} INNER JOIN {1} ON {2} = {3}");

        public readonly static DBFieldTemplate FIELD = new DBFieldTemplate();
        public readonly static InsertTemplate INSERT_TEMPLATE = new InsertTemplate();
        public readonly static DeleteTemplate DELETE_TEMPLATE = new DeleteTemplate();
        public readonly static SelectTemplate SELECT_TEMPLATE = new SelectTemplate();
        public readonly static UpdateTemplate UPDATE_TEMPLATE = new UpdateTemplate();

    }


    public interface Operator
    {
        String Value { get; }
    }

    public abstract class AbstractOperator : Operator
    {
        private String value;

        public AbstractOperator(String aValue)
        {
            value = aValue;
        }
        public override string ToString() { return Value; }

        public string Value { get { return value; } }
    }

    public class LogicalOperator : AbstractOperator
    {
        public LogicalOperator(String aValue)
            : base(aValue)
        {
        }
    }

    public class BooleanOperator : AbstractOperator
    {
        public BooleanOperator(String aValue)
            : base(aValue)
        {
        }
    }

    public class SqlOperator : AbstractOperator
    {
        public SqlOperator(String aValue)
            : base(aValue)
        {
        }
    }

    public class NonOperator : AbstractOperator
    {
        public NonOperator(String aValue)
            : base(aValue)
        {
        }
    }

    public class SortOperator : AbstractOperator
    {
        public SortOperator(String aValue)
            : base(aValue)
        {
        }
    }

    public class ParameterOperator : AbstractOperator
    {
        private String Format;
        public ParameterOperator(String aValue, String aFormat)
            : base(aValue)
        {
            Format = aFormat;
        }

        public String GetValue(params Object[] aValues)
        {
            return String.Format(Format, aValues);
        }

    }

    public class SingleParameterOperator : AbstractOperator
    {
        private String Format;
        public SingleParameterOperator(String aValue, String aFormat)
            : base(aValue)
        {
            Format = aFormat;
        }

        public String GetValue(Object aValue)
        {
            return String.Format(Format, aValue);
        }

    }

    public class JoinOperator : AbstractOperator
    {
        private String Format;
        public JoinOperator(String aValue, String aFormat)
            : base(aValue)
        {
            Format = aFormat;
        }

        public String GetValue(String aTable1, String aTable2, String aField1, String aField2)
        {
            return String.Format(Format, aTable1, aTable2, aField1, aField2);
        }

    }

    public class DBFieldTemplate : AbstractOperator
    {
        private static String Format = "[dbo].[{0}].[1]";
        public DBFieldTemplate()
            : base(Format)
        {
        }

        public String GetValue(String aTable, String aField)
        {
            return String.Format(Format, aTable, aField);
        }

    }

    public class InsertTemplate : AbstractOperator
    {
        private static String Format = "Insert Into {0} {1};Select @@Identity {2}";
        public InsertTemplate()
            : base(Format)
        {
        }


        public String GetValue(String aTable, String aValues, String anId)
        {
            return String.Format(Format, aTable, aValues, anId);
        }
    }

    public class SelectTemplate : AbstractOperator
    {
        private static String Format = "Select {0} From {1}";
        public SelectTemplate()
            : base(Format)
        {
        }

        public String GetValue(String aFields, String aFrom)
        {
            return String.Format(Format, aFields, aFrom);
        }
    }

    public class DeleteTemplate : AbstractOperator
    {
        private static String Format = "Delete From {0}";
        public DeleteTemplate()
            : base(Format)
        {
        }
        public String GetValue(String aTable)
        {
            return String.Format(Format, aTable);
        }
    }

    public class UpdateTemplate : AbstractOperator
    {
        private static String Format = "Update {0} set {1}";
        public UpdateTemplate()
            : base(Format)
        {
        }
        public String GetValue(String aTable, String aValues)
        {
            return String.Format(Format, aTable, aValues);
        }
    }

    public class PagerTemplate : AbstractOperator
    {
        private static String Format =
            "Select * From ({0}) as Parent Where SQLBUILDERROWID > {1} AND " +
        "SQLBUILDERROWID < {2} Order by SQLBUILDERROWID ";

      
        public PagerTemplate()
            : base(Format)
        {
        }

        public String GetSqlValue(String anInnerQuery, int aMinimun, int aMaximum)
        {
            return String.Format(Format, anInnerQuery, aMinimun, aMaximum);
        }

      
    }
}


