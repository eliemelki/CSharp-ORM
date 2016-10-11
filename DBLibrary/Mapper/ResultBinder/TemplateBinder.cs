using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;

namespace DBLibrary.Mapper.ResultBinder
{
    public class TemplateBinder<T> : AbstractBinder<T>, CacheBinder<T> where T : class, new() 
    {
        protected ValueExtractor ValueExtractor;
        protected PropertyBinder PropertyBinder;
        protected CacheBinderHelper<T> CacheBinderHelper;

        public TemplateBinder()
        {
            PropertyBinder = Database.Current.Factory.GetInstance<PropertyBinder>();
            ValueExtractor = Database.Current.Factory.GetInstance<ValueExtractor>();
            CacheBinderHelper = new CacheBinderHelper<T>();
        }

        private void BindValue(PropertyMap aMap, SqlDataReader aReader, Object aData) 
        {
            try
            {
                int ordinal = aReader.GetOrdinal(aMap.GetColumn());
                var value = aReader.IsDBNull(ordinal) ? null : aReader[ordinal];
                ValueExtractor.SetValue(aMap.Member.Name, aData, value);
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        protected void BindValue(PropertyMap aMap, System.Data.DataRow aDataRow, Object aData)
        {
            try
            {
                var _value = aDataRow[aMap.GetColumn()];
                if (_value.GetType() == typeof(System.DBNull))
                    _value = null;
                
                ValueExtractor.SetValue(aMap.Member.Name, aData, _value);
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        protected override T Binding(SqlDataReader aReader) 
        {
            T data = new T();
            PropertyBinder.BindProperty<T>(
                delegate(PropertyMap aMap)
                {
                    BindValue(aMap, aReader, data);
                },
                delegate(IdentityMap aMap)
                {
                    BindValue(aMap, aReader, data);
                },
                delegate(PropertyMap aMap,MemberInfo[] aWrapperParents)
                {
                    var field = ValueExtractor.GetValue( data,aWrapperParents);
                    BindValue(aMap, aReader, field);
                }
            );
            return data;
        }

        public T BindFromCache(System.Data.DataRow aDataRow)
        {
            T data = new T();
            PropertyBinder.BindProperty<T>(
                delegate(PropertyMap aMap)
                {
                    BindValue(aMap, aDataRow, data);
                },
                delegate(IdentityMap aMap)
                {
                    BindValue(aMap, aDataRow, data);
                },
                     delegate(PropertyMap aMap, MemberInfo[] aWrapperParents)
                     {
                         var field = ValueExtractor.GetValue(data, aWrapperParents);
                         BindValue(aMap, aDataRow, field);
                     }
            );
            return data;
        }

        public DataRow BindToCache(SqlDataReader aReader)
        {
            return CacheBinderHelper.BindToCache(aReader);
        }
    }
}


