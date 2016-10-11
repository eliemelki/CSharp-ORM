using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loader;

namespace DBLibrary.Configuration
{
    public class EntryPoint : InjectEntryPoint
    {
        private String DBConnectionParamater;
        private Type Type;
        private SqlModules SqlModules;

        public EntryPoint(Type aType, String aDBConnectionParamater, SqlModules anSqlModules)
        {
            DBConnectionParamater = aDBConnectionParamater;
            Type = aType;
            SqlModules = anSqlModules;
        }

        public EntryPoint(Type aType, String aDBConnectionParamater)
            : this(aType,aDBConnectionParamater, new SqlModules())
        {
            
        }

        public Ninject.Modules.INinjectModule[] GetModules()
        {
            return new Ninject.Modules.INinjectModule[] { SqlModules };
        }

        public void Load()
        {
            SqlFactory _factory = BaseFactory.Instance.GetInstance<SqlFactory>();
            Initialise initialiser = _factory.GetInstance<Initialise>();
            initialiser.Start( DBConnectionParamater, Type);
        }
    }
}
