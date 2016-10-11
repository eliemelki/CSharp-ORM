using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using DBLibrary.Tools.Src.ClassMapGenerator;

namespace DBLibrary.Tools.Src.Configuration
{
    public class DBlibraryToolModules: NinjectModule
    {
        public override void Load()
        {
            Bind<ClassMapGenerator.ClassMapGenerator>().To<ClassMapGenerator.ClassMapGeneratorImpl>();
            Bind<ClassMapTemplateGenerator>().To<ClassMapTemplateGeneratorImpl>();
            Bind<StoreProcedureGenerator.StoreProcedureGenerator>().To<StoreProcedureGenerator.StoreProcedureGeneratorImpl>();
            Bind<StoreProcedureGenerator.StoreProcedureTemplateGenerator>().To<StoreProcedureGenerator.StoreProcedureTemplateGeneratorImpl>();
        }
    }
}
