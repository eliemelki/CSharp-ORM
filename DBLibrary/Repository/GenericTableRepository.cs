using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBLibrary.Repository
{
    public class GenericTableRepository<T> : AbstractRepository<T> where T : class, new()
    {
        public GenericTableRepository(DBHelper aDBHelper) : base(aDBHelper)
        {
           
        }
    }
}
