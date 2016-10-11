using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Ninject.Modules;
using System.Reflection;
using Loader.Config;

namespace Loader
{
    public interface EntryPoint
    {
        void Load();
    }

    public interface InjectEntryPoint : EntryPoint    
    {
        INinjectModule[] GetModules();
    }

}
