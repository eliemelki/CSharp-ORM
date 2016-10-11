using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DBLibrary.Configuration;
using DBLibrary.QueryEngine.Query;
using DBLibrary.Session;
namespace DBLibrary.Mapper
{
    public interface ClassMapLoader
    {
        void Load();
        ClassMap<T> GetClassMap<T>() where T : class, new();
        ClassMap GetClassMap(Type aType);
        bool isClassMap(Type aType);
        bool HasClassMap(Type aType);
    }

    public class ClassMapException : Exception
    {
        public const String ERROR = "Could not map Class:{0} \r\n Exception Message:{1}";
        public ClassMapException(String anExc)
            : base(anExc)
        {
        }
    }

    class ClassMapLoaderImpl : ClassMapLoader
    {
        private Dictionary<Type,ClassMap> classmaps;

        public Config Config { set; get; }


        public ClassMapLoaderImpl(Config aConfig)
        {
            Config = aConfig;
            classmaps = new Dictionary<Type, ClassMap>();
        }

        public void Load()
        {
            Assembly[] _assemblies = Config.DbAssembly;
            foreach (Assembly _assembly in _assemblies.Distinct())
            {
                foreach (Type type in _assembly.GetExportedTypes().Where(isClassMap))
                {
                    addToList(type);
                }
            }
        }

        private void addToList(Type aType)
        {
            if (aType == null) return;
            Type basetype = aType.BaseType;

            if (basetype != null 
                && basetype.IsGenericType 
                && basetype.BaseType == typeof(ClassMap)
                )
            {
                Type type = null;
                type = basetype.GetGenericArguments()[0];
                classmaps.Add(type, (ClassMap)Activator.CreateInstance(aType));
            }
            else
            {
                addToList(basetype);
            }
        }

        public bool isClassMap(Type aType)
        {
            return aType.IsSubclassOf(typeof(ClassMap));
        }

        public bool HasClassMap(Type aType)
        {
            ClassMap _value;
            return classmaps.TryGetValue(aType, out _value);
        }

        public ClassMap<T> GetClassMap<T>() where T : class, new()
        {
            return (ClassMap<T>)classmaps[typeof(T)];
        }

        public ClassMap GetClassMap(Type aType)
        {
            return classmaps[aType];
        }

       
        private const String DISPLAY = "ClassMapLoader: Config:{0} Map={1}";


    }
}
