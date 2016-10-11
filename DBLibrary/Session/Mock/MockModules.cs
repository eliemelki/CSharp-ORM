using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.Configuration;

namespace DBLibrary.Session.Mock
{
    public class MockModules : SqlModules
    {
        public override void Load()
        {
            base.Load();
            Unbind<DBConnection>();
            Bind<DBConnection>().To<MockConnection>();

            Unbind<Transaction>();
            Bind<Transaction>().To<MockTransaction>();
        }
    }
}
