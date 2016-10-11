using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DBLibrary.Mapper.ResultBinder
{
    public interface CacheBinder<T> where T : class
    {
        T BindFromCache(DataRow aDataRow);
        DataRow BindToCache(SqlDataReader aReader);
    }

    public class CacheBinderHelper<T>
    {
        public DataTable DataTable { private set; get; }
        private bool ColumnAdded;

        public CacheBinderHelper()
        {
            DataTable = new DataTable();
            ColumnAdded = false;
        }

        private void AddColumns(SqlDataReader aReader)
        {
            if (ColumnAdded) return;
            ColumnAdded = true;
            for (int j = 0; j < aReader.FieldCount; j++)
                DataTable.Columns.Add(aReader.GetName(j),aReader.GetFieldType(j));
        }

        public DataRow BindToCache(SqlDataReader aReader)
        {
             AddColumns(aReader);
             DataRow row = DataTable.NewRow();
             for (int j = 0; j < aReader.FieldCount; j++)
             {    
                 row[aReader.GetName(j)] =  aReader[j] ;
             }
            DataTable.Rows.Add(row);
            return row;
        }

    }
}
