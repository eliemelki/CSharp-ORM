using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;
using DBLibrary.Mapper;
namespace DBLibrary.Mapper.ResultBinder
{
    public class TemplateInsertBinder<T> : AbstractBinder<T> where T : class, new() 
    {
        protected ValueExtractor ValueExtractor;
        protected PropertyBinder PropertyBinder;

        public TemplateInsertBinder()
        {
            PropertyBinder = Database.Current.Factory.GetInstance<PropertyBinder>();
            ValueExtractor = Database.Current.Factory.GetInstance<ValueExtractor>();
        }


        protected override T Binding(SqlDataReader aReader)
        {
            T data = new T();
            PropertyBinder.BindProperty<T>(
                 null,
                 delegate(IdentityMap aMap)
                 {
                     Bind(aMap, aReader, data);
                 },
                 null
             );

            return data;
        }

        private void Bind(PropertyMap aMap, SqlDataReader aReader, Object aData)
        {
            try
            {
                int ordinal = aReader.GetOrdinal(aMap.GetColumn());
                var value = aReader.IsDBNull(ordinal) ? null : System.Convert.ChangeType(aReader[ordinal], aMap.Type);
                ValueExtractor.SetValue(aMap.Member.Name, aData, value);
            }
            catch (IndexOutOfRangeException)
            {
            }
        }
    }
}
