using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loader;
using DBLibrary.Configuration;
using Ninject.Modules;

namespace DBLibrary.Tools.Src.Configuration
{
    public class EntryPoint : InjectEntryPoint
    {
        private String DBConnectionParamater;
        private DBlibraryToolModules ToolModules = new DBlibraryToolModules();
        private DBLibrary.Configuration.EntryPoint SqlEntryPoint;


        public EntryPoint(String aDBConnectionParamater, DBlibraryToolModules aToolModules = null, SqlModules aSqlModule = null)
        {
            DBConnectionParamater = aDBConnectionParamater;
            if (aToolModules != null)
                ToolModules = aToolModules;

            SqlModules _sqlModule = new SqlModules();
            if (aSqlModule != null)
                _sqlModule = aSqlModule;

            SqlEntryPoint = new DBLibrary.Configuration.EntryPoint(typeof(EntryPoint), aDBConnectionParamater, _sqlModule);

        }

        public Ninject.Modules.INinjectModule[] GetModules()
        {
            List<INinjectModule> _modules = new List<INinjectModule>();
            _modules.AddRange(SqlEntryPoint.GetModules());
            _modules.Add(ToolModules);

            return _modules.ToArray();
        }

        public void Load()
        {
            SqlEntryPoint.Load();
        }
    }
}
