using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Ninject.Modules;
using Loader.Config;
using System.Reflection;

namespace Loader
{
    public class ApplicationLoader
    {
        private int count;
        private InjectModule Module;

        public ApplicationLoader(InjectModule aModule)
        {
            Module = aModule;
        }

        public ApplicationLoader() : this( new InjectModule())
        {
        }

        public BaseFactory Load(params EntryPoint[] anEntryPoint)
        {
            if (count != 0)
            {
                throw new Exception("You cannot load twice");
            }
            count++;

            BaseFactory.Instance = new BaseFactory(CreateKernel(anEntryPoint));
            LoadApplication(anEntryPoint);
            return BaseFactory.Instance;
        }



        private IKernel CreateKernel(params EntryPoint[] anEntryPoint)
        {
            List<INinjectModule> _modules = new List<INinjectModule>();

            foreach (var _entryPoint in anEntryPoint)
            {
                if (typeof(InjectEntryPoint).IsAssignableFrom(_entryPoint.GetType()))
                {
                    var _injector = (InjectEntryPoint)_entryPoint;
                    _modules.AddRange(_injector.GetModules());
                }
            }
            Module.LoadConfigurationInfos(anEntryPoint);
            _modules.Add(Module);
            return new StandardKernel(_modules.ToArray());
        }
       
        private void LoadApplication(params EntryPoint[] anEntryPoint)
        {
            foreach (var _entryPoint in anEntryPoint)
            {
                _entryPoint.Load();
            }
        }

    }
}
