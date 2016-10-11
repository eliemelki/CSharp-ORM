using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Utils;

namespace Loader.Config
{
    public interface FileConfigLoader
    {
        T GetConfiguration<T>();
    }

    class ConfigurationValue
    {
        public ConfigurationValue(ConfigurationInfo anInfo, Object aConfiguration)
        {
            Info = anInfo;
            Configuration = aConfiguration;
        }

        public ConfigurationInfo Info { private set; get; }
        public Object Configuration { private set; get; }
    }

    public class FileConfigLoaderImpl : FileConfigLoader
    {

        private Dictionary<Type, ConfigurationValue> Configurations = new Dictionary<Type, ConfigurationValue>();
        private FileRetriever Retriever = FileRetrieverFactory.newInstance();

        public FileConfigLoaderImpl(List<ConfigurationInfo> aInfo)
        {
            LoadConfig(aInfo);
        }

        public void LoadConfig(List<ConfigurationInfo> aInfo) 
        {
            foreach (ConfigurationInfo _info in aInfo)
            {
                Configurations.Add(_info.ConfigType, new ConfigurationValue(_info, LoadConfig(_info)));
            }
        }



        private Object LoadConfig(ConfigurationInfo aConfigInfo)
        {
            var _file =  GetFile(aConfigInfo);
    
            using (Stream _stream = Retriever.RetrieveStream(_file))
            {
                return Deserialiase(aConfigInfo.ConfigType, _stream);
            }
        }

        private Object Deserialiase(Type aType, Stream aStream)
        {
            XmlSerializer xSerializer = new System.Xml.Serialization.XmlSerializer(aType);
            return xSerializer.Deserialize(aStream);
        }

        public T GetConfiguration<T>()
        {
            Type _type = typeof(T);
            var _info = Configurations[_type].Info;
            if (_info.isUpdatable && !Retriever.isAssembly(GetFile(_info)))
            {
                return (T)LoadConfig(_info);
            }
            return (T)Configurations[_type].Configuration;
        }

        private String GetFile(ConfigurationInfo aConfigInfo)
        {
            return !String.IsNullOrEmpty(aConfigInfo.FileName) ? aConfigInfo.FileName : aConfigInfo.DefaultFileName;
        }
    }
}
