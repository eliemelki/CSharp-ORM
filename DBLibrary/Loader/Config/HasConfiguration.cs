using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loader.Config
{
    public interface HasConfiguration
    {
        ConfigurationInfo GetConfigurationInfo();
    }
}
