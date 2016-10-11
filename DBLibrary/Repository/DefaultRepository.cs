using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBLibrary.Repository
{
    public class DefaultRepository<T> : AbstractRepository<T> where T : class, new()
    {
        public DefaultRepository(DBHelper aDBHelper)
            : base(aDBHelper)
        {

        }
    }
}
