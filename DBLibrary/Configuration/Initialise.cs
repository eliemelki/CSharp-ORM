using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.Mapper;
namespace DBLibrary.Configuration
{
    public interface Initialise
    {
        void Start(String aConnectionString, params Type[] aTypes);
    }

    class InitialiseImpl : Initialise
    {
        private Config Config;
        private ClassMapLoader Loader;
        public InitialiseImpl(Config aConfig, ClassMapLoader aLoader)
        {
            Loader = aLoader;
            Config = aConfig;
        }

        public void Start(String aConnectionString, params Type[] aTypes)
        {
            Config.Load(aConnectionString, aTypes);
            Loader.Load();
        }
    }

}
