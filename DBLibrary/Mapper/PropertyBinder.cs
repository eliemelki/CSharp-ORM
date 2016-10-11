using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace DBLibrary.Mapper
{
    public delegate void TableName(String aTableName);
    public delegate void OnPropertyBind(PropertyMap aMap);
    public delegate void OnIdentityBind(IdentityMap aMap);
    public delegate void OnComponentBind(PropertyMap aMap, params MemberInfo[] aWrapperMember);

    public interface PropertyBinder
    {
        void BindProperty<T>(OnPropertyBind onPropertyBind, OnIdentityBind onIdentityBind, OnComponentBind onComponentBind)
            where T: class, new();
        String GetTable<T>() where T : class, new();
    }

    class PropertyBinderImpl : PropertyBinder 
    {
        private ClassMapLoader Loader;
        public PropertyBinderImpl(ClassMapLoader aLoader)
        {
            Loader = aLoader;
        }
        public String GetTable<T>() where T : class, new()
        {
            ClassMap<T> classMap = Loader.GetClassMap<T>();
            return classMap.GetTableName();
        }

        public void BindProperty<T>(OnPropertyBind onPropertyBind, OnIdentityBind onIdentityBind, OnComponentBind onComponentBind)
        where T : class, new()
        {
            ClassMap<T> classMap = Loader.GetClassMap<T>();

            if (onPropertyBind != null)
            {
                foreach (PropertyMap map in classMap.Properties)
                {
                    onPropertyBind(map);
                }
            }

            if (onIdentityBind != null && classMap.Identity != null)
            {
                onIdentityBind(classMap.Identity);
            }

            if (onComponentBind != null)
            {
                MapComponent(classMap, onComponentBind, classMap.Components);
            }
        }

        private void MapComponent<T>(ClassMap<T> classMap, OnComponentBind onComponentBind, List<ComponentMap> aComponentMap,params MemberInfo[] aWrapperMember)
             where T : class, new()
        {
            foreach (ComponentMap map in aComponentMap)
            {
                var _wrapperMember = new List<MemberInfo>(aWrapperMember);
                _wrapperMember.Add(map.Member);
                foreach (PropertyMap map2 in map.Properties)
                {
                    onComponentBind(map2, _wrapperMember.ToArray());
                }

                MapComponent(classMap, onComponentBind, map.Components, _wrapperMember.ToArray());
               
            }
        }
    }
}
