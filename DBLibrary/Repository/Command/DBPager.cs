using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBLibrary.Repository.Command
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
    }
}
