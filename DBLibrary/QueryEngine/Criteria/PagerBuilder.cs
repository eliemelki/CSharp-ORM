using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;


namespace DBLibrary.QueryEngine.Criteria
{
    public class PagerBuilder 
    {
        private int Maximum;
        private int Minimum;

        public static String GetSqlString(String aQuery, int aMinimum, int aMaximum)
        {
            return new PagerBuilder(aMinimum, aMaximum).GeneratePager(aQuery);
        }

        private PagerBuilder(int aMinimum, int aMaximum)
        {
            Maximum = aMaximum;
            Minimum = aMinimum;
        }

        private bool checkifPager()
        {
            if (Minimum > 0 && Maximum > 0)
                return true;
            return false;
        }

        private bool checkifTop()
        {
            if (checkifPager())
                return false;
            if (Maximum > 0) 
                return true;
            return false;
        }

        private int SELECT_COUNT = "Select".Count();


        private String GeneratePager(String aQuery)
        {
            if (checkifTop())
               return AppendTop(aQuery.ToString());
            else if (checkifPager())
                return AppendPager(aQuery.ToString());
            return aQuery;
        }


        private String AppendTop(String aQuery)
        {

            int pos = aQuery.IndexOf("Select",System.StringComparison.OrdinalIgnoreCase);

            String withoutSelect = aQuery.Substring(pos + SELECT_COUNT, (aQuery.Count()) - (pos + SELECT_COUNT));

            return String.Format(SqlSyntax.SELECT_TOP + withoutSelect,Maximum);
        }

        
        private const String PAGER_ROW = ",ROW_NUMBER() OVER ({0}) AS SQLBUILDERROWID";
        private const String PAGER_SQL =
        "Select * From ({0}) as Parent Where SQLBUILDERROWID > {1} AND "+
        "SQLBUILDERROWID < {2} Order by SQLBUILDERROWID ";

        private const String PAGER_APPENDED = "{0} From {1}";
        
        private String AppendPager(String aQuery)
        {
            int FromPos = GetFromPosition(aQuery);
            int oderbyPos;
            String orderby = GetOrderBy(aQuery, out oderbyPos);
            if (oderbyPos > 0)
            {
                aQuery = aQuery.Substring(0, oderbyPos);
            }
            String beforeFrom = aQuery.Substring(0, FromPos);
            String afterFrom = aQuery.Substring(FromPos + fromCount, aQuery.Count() - (FromPos + fromCount));
            String appenededbeforeFrom = String.Format(beforeFrom +PAGER_ROW, orderby);
            String final = String.Format(PAGER_APPENDED,appenededbeforeFrom ,afterFrom);
            return String.Format(PAGER_SQL,  final, Minimum, Maximum);

        }

        private const String DEFAULT_ORDER = "ORDER BY CURRENT_TIMESTAMP";

        private String GetOrderBy(String aQuery,out int orderByPos)
        {
            orderByPos = aQuery.IndexOf("ORDER BY", System.StringComparison.OrdinalIgnoreCase);
            if (orderByPos > 0)
            {
                return aQuery.Substring(orderByPos, aQuery.Count() - orderByPos); 
            }
            return DEFAULT_ORDER;
        }

        private int fromCount = "From".Count();
        private int GetFromPosition(String aQuery)
        {
            int FromPos = aQuery.IndexOf("]from[", System.StringComparison.OrdinalIgnoreCase);
            if (FromPos > 0)
                return FromPos+1;

            FromPos = aQuery.IndexOf("]from", System.StringComparison.OrdinalIgnoreCase);
            if (FromPos > 0)
                return FromPos+1;
        
            FromPos = aQuery.IndexOf("from[", System.StringComparison.OrdinalIgnoreCase);
            if (FromPos > 0)
                return FromPos;

            FromPos = aQuery.IndexOf(" from", System.StringComparison.OrdinalIgnoreCase);
            if (FromPos > 0)
            {
                fromCount += 1;
                return FromPos;
            }
            FromPos = aQuery.IndexOf("from ", System.StringComparison.OrdinalIgnoreCase);
            if (FromPos > 0)
                return FromPos;

            throw new Exception("Invalid Query. Check the query");

        }
    }


}
