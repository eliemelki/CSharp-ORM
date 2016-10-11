using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Configuration;
using DBLibrary.Repository;

namespace DBLibrary.Configuration
{
    public interface Config
    {
        String DataSource { get; }
        Assembly[] DbAssembly { get; }
        void Load(String aConnectionString, params Type[] aTypes);
    }


    class ConfigImpl : Config
    {
        private String ConnectionString;
        public ConfigImpl()
        {
          
    
        }


        public String DataSource { get { return ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString; } }

        public Assembly[] DbAssembly { get; private set; }



        public void Load(String aConnectionString, params Type[] aTypes)
        {
            List<Assembly> _assembly = new List<Assembly>();
            _assembly.Add(Assembly.GetAssembly(typeof(Column)));
            foreach (Type _type in aTypes)
                _assembly.Add(Assembly.GetAssembly(_type));
            DbAssembly = _assembly.ToArray();
            ConnectionString = aConnectionString;
        }
    }
}
