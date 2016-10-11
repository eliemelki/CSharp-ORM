using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using Ninject;
using Loader.Config;

namespace Loader
{
    public class InjectModule : NinjectModule
    {
        public List<ConfigurationInfo> ConfigurationInfos { set; get; }

        public override void Load()
        {
            Kernel.Bind<FileConfigLoader>().To<FileConfigLoaderImpl>().InSingletonScope().WithConstructorArgument("aInfo", ConfigurationInfos);
            
        }

        public void LoadConfigurationInfos(params EntryPoint[] anEntryPoint)
        {
            ConfigurationInfos = new List<ConfigurationInfo>();
            foreach (var _entryPoint in anEntryPoint)
            {
                if (typeof(HasConfiguration).IsAssignableFrom(_entryPoint.GetType()))
                {
                    ConfigurationInfos.Add(((HasConfiguration)_entryPoint).GetConfigurationInfo());
                }
            }

        }
    }
}
