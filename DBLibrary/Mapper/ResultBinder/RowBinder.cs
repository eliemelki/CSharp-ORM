using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DBLibrary.Mapper.ResultBinder
{
    public class RowBinder : AbstractBinder<DataRow>, CacheBinder<DataRow>
    {
        public DataTable DataTable { private set; get; }
        private bool ColumnAdded;
        protected CacheBinderHelper<DataRow> CacheBinderHelper;

        public RowBinder()
        {
            DataTable = new DataTable();
            ColumnAdded = false;
            CacheBinderHelper = new CacheBinderHelper<DataRow>();

        }

        private void AddColumns(SqlDataReader aReader)
        {
            if (ColumnAdded) return;
            ColumnAdded = true;
            for (int j = 0; j < aReader.FieldCount; j++)
                DataTable.Columns.Add(aReader.GetName(j),aReader.GetFieldType(j));
        }

        private void AddColumns(DataRow aDataRow)
        {
            if (ColumnAdded) return;
            ColumnAdded = true;
            for (int j = 0; j < aDataRow.Table.Columns.Count; j++)
                DataTable.Columns.Add(aDataRow.Table.Columns[j].ColumnName,aDataRow.Table.Columns[j].DataType);
        }

        protected override DataRow Binding(SqlDataReader aReader)
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

        public DataRow BindFromCache(DataRow aDataRow)
        {
            AddColumns(aDataRow);
            DataRow row = DataTable.NewRow();
            for (int j = 0; j < DataTable.Columns.Count; j++)
            {
                row[DataTable.Columns[j].ColumnName] = aDataRow[j];
            }
            DataTable.Rows.Add(row);
            return row;

        }

        public DataRow BindToCache(SqlDataReader aReader)
        {
            return CacheBinderHelper.BindToCache(aReader);
        }
    }
}
