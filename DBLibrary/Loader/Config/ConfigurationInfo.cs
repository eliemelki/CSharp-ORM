using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loader.Config
{
    
    public class ConfigurationInfo
    {
        public ConfigurationInfo(bool aIsUpdatable, String aFileName, String aDefaultFileName, Type aConfigType)
        {
            isUpdatable = aIsUpdatable;
            FileName = aFileName;
            DefaultFileName = aDefaultFileName;
            ConfigType = aConfigType;
        }

        public bool isUpdatable { get; private set; }

        public String FileName { get; private set; }

        public String DefaultFileName { get; private set; }

        public Type ConfigType { get; private set; } 
    }
}
