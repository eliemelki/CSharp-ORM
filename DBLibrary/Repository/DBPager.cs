using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBLibrary.Repository
{
    public class DBPager
    {
        public DBPager(int aPageNumber, int aRowCount)
        {
            PageNumber = aPageNumber;
            RowCount = aRowCount;
        }
        public int PageNumber { get; private set; }
        public int RowCount { get; private set; }
        public int TotalCount { get; set; }
    }

    public class DBPagerResult<T> where T : class, new()
    {
        public DBPagerResult(DBPager aPager, List<T> aResult)
        {
            Pager = aPager;
            Result = aResult;
        }
        public DBPager Pager { private set; get; }
        public List<T> Result { private set; get; }
    }

}
