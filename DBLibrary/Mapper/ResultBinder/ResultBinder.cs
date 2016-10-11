using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DBLibrary.Mapper.ResultBinder
{
    public delegate void OnBind<T>(T aResult);

    public interface ResultBinder<T>
    {
        T Bind(SqlDataReader aReader);
        OnBind<T> OnBind { set; get; }
    }

    public abstract class AbstractBinder<T> : ResultBinder<T> where T : class
    {
    
        public T Bind(SqlDataReader aReader)
        {
            if (aReader.FieldCount > 0)
            {
               T data = Binding(aReader);
               if (this.OnBind != null)
               {
                   OnBind(data);
               }
                   return data;
            }
            return null;
        }

        protected abstract T Binding(SqlDataReader aReader);

        public OnBind<T> OnBind { set; get; }

    }

    public class DataReaderBinder : AbstractBinder<SqlDataReader>
    {
        protected override SqlDataReader Binding(SqlDataReader aReader)
        {
            return aReader;
        }
    }
}

