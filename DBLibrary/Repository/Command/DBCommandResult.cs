using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBLibrary.Repository.Command
{
    public interface DBCommandResult<T> where T : class,  new()
    {
        List<T> Results { get; }
        int TotalCount { get; }
        DBPager Pager { get; }
    }

    public class DBCommandResultImpl<T> : DBCommandResult<T> where T : class,  new()
    {
        public DBCommandResultImpl(List<T> aResults, int aTotalCount, DBPager aPager)
        {
            Results = aResults;
            TotalCount = aTotalCount;
            Pager = aPager;
        }

        public List<T> Results { get; private set; }
        public int TotalCount { get; private set; }
        public DBPager Pager { get; private set; }
    }
}
