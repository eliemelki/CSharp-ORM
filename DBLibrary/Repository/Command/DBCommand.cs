using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBLibrary.Repository.Command
{
    public interface DBCommand<T> where T : class, new()
    {
        List<DBFilterBase<T>> Filters { get; }
        DBPager Pager { get; }
        List<DBSort<T>> Sorts { get; }
    }

    public class DBCommandImpl<T> : DBCommand<T> where T : class, new()
    {
        public DBCommandImpl(DBPager aPager, List<DBSort<T>> aSorts, List<DBFilterBase<T>> aFilters)
        {
            Pager = aPager;
            Sorts = aSorts;
            Filters = aFilters;
        }

        public List<DBFilterBase<T>> Filters { get; private set; }
        public List<DBSort<T>> Sorts { get; private set; }
        public DBPager Pager { get; private set; }
    }
}
