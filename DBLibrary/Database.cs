using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using Ninject;
using log4net;
using DBLibrary.Configuration;
using Loader;

namespace DBLibrary
{
    public class Database
    {
        private static Database instance = new Database();

        public static Database Current { get { return instance; } }

        public SqlFactory Factory
        {
            get
            {
                return BaseFactory.Instance.GetInstance<SqlFactory>();
            }
        }

        private Database()
        {
        }
    }
}
