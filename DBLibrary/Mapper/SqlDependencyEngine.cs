using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.Configuration;
using System.Reflection;
using System.Web.Caching;
using log4net;

namespace DBLibrary.Mapper
{
   
    public interface SqlCacheDependencyEngine
    {
        void Start(params Type[] aDependencyTablesType);
        void Stop();
        bool isDependencyTableEnabled(Type aType);

        IEnumerable<String> RegisteredSqlDependencyTablesNames { get; }
        IEnumerable<String> DependencyTablesNames { get; }
        IEnumerable<Type> DependencyTablesType { get; }
    }

    class SqlCacheDependencyEngineImpl : SqlCacheDependencyEngine
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(DatabaseLogger));

        private List<Type> MutableDependencyTablesType;
        private Config Config;
        private ClassMapLoader Loader;

        public SqlCacheDependencyEngineImpl(Config aConfig,ClassMapLoader aClassMapLoader)
        {
            MutableDependencyTablesType = new List<Type>();
            Config = aConfig;
            Loader = aClassMapLoader;
        }

        public void Start(params Type[] aDependencyTablesType)
        {
            try
            {
                String _datasource = Config.DataSource;
                SqlCacheDependencyAdmin.EnableNotifications(_datasource);
                System.Data.SqlClient.SqlDependency.Start(_datasource);
                EnableDependency(aDependencyTablesType);
            }
            catch (Exception anExc)
            {
                logger.Error(anExc);
            }
        }

        private void EnableDependency(params Type[] DependencyTablesType)
        {
            foreach (Type _tableType in DependencyTablesType)
            {
                String _table = Loader.GetClassMap(_tableType).GetTableName();
                if (Loader.HasClassMap(_tableType))
                {
                    if (!SqlCacheDependencyAdmin.GetTablesEnabledForNotifications(Config.DataSource).Contains(_table))
                    {
                        SqlCacheDependencyAdmin.EnableTableForNotifications(Config.DataSource, _table);
                        logger.Info("Enabling Notification For: " + _table);
                    }
                    else if (!MutableDependencyTablesType.Exists(m => m == _tableType))
                    {
                        logger.Info("Already Enabled Notification For: " + _table);
                        MutableDependencyTablesType.Add(_tableType);
                    }
                }
            }
        }

        public void Stop()
        {
            System.Data.SqlClient.SqlDependency.Stop(Config.DataSource);
        }

        public bool isDependencyTableEnabled(Type aType)
        {
            String _table = Loader.GetClassMap(aType).GetTableName();
            return Loader.HasClassMap(aType)
               &&
               SqlCacheDependencyAdmin.GetTablesEnabledForNotifications(Config.DataSource).Contains(_table);
        }

        public IEnumerable<Type> DependencyTablesType { get { return MutableDependencyTablesType; } }

        public IEnumerable<String> DependencyTablesNames
        {
            get
            {
                List<String> _dependencyTablesName = new List<String>();
                foreach (Type type in MutableDependencyTablesType)
                {
                    _dependencyTablesName.Add(Loader.GetClassMap(type).GetTableName());
                }
                return _dependencyTablesName;
            }
        }


        public IEnumerable<String> RegisteredSqlDependencyTablesNames
        {
            get
            {
                List<String> _sqlDependencyTablesName = new List<String>();
                foreach (String _table in SqlCacheDependencyAdmin.GetTablesEnabledForNotifications(Config.DataSource))
                {
                    _sqlDependencyTablesName.Add(_table);
                }
                return _sqlDependencyTablesName;
            }
        }
    }
}
