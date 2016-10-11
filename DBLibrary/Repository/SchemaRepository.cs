using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.Mapper;
using DBLibrary.Session;
using DBLibrary.QueryEngine.Query;
using DBLibrary.Configuration;
using DBLibrary.QueryEngine.Query.Queries;
using DBLibrary.Mapper.ResultBinder;
using Loader;

namespace DBLibrary.Repository
{

    public class DBColumn
    {
        public String TABLE_SCHEMA { set; get; }
        public String TABLE_CATALOG { set; get; }
        public String TABLE_NAME { set; get; }
        public virtual String COLUMN_NAME { set; get; }
        public String DATA_TYPE { set; get; }
        public String CONSTRAINT_TYPE { set; get; }
    }

    public class TablesClassMap : ClassMap<DBColumn>
    {
        public TablesClassMap()
        {
            Map(this);
        }

        public static void Map<T>(ClassMap<T> aClassMap) where T : DBColumn, new()
        {
            aClassMap.MapField(m => m.TABLE_SCHEMA).SetColumn("TABLE_SCHEMA");
            aClassMap.MapField(m => m.TABLE_CATALOG).SetColumn("TABLE_CATALOG");
            aClassMap.MapField(m => m.TABLE_NAME).SetColumn("TABLE_NAME");
            aClassMap.MapField(m => m.COLUMN_NAME).SetColumn("COLUMN_NAME");
            aClassMap.MapField(m => m.DATA_TYPE).SetColumn("DATA_TYPE");
            aClassMap.MapField(m => m.CONSTRAINT_TYPE).SetColumn("CONSTRAINT_TYPE");
            aClassMap.SetTableName("");
        }
    }


    public interface SchemaRepository
    {
        IEnumerable<TableStructure> GetSchemaTables();
    }

    public class SchemaRepositoryImpl : SchemaRepository
    {
        protected DBHelper DBHelper;
  
        public SchemaRepositoryImpl(DBHelper aDBHelper)
        {
            DBHelper = aDBHelper;
        }

        public IEnumerable<TableStructure> GetSchemaTables()
        {

            SqlQuery _query = NativeQuery.SimpleQueryHelper(
                "select col.TABLE_SCHEMA, col.TABLE_CATALOG, col.TABLE_NAME, col.COLUMN_NAME, col.DATA_TYPE, const.CONSTRAINT_TYPE " +
                "from INFORMATION_SCHEMA.COLUMNS as col " +
                "LEFT JOIN " +
                "(SELECT cu.TABLE_NAME, cu.COLUMN_NAME,  cu.CONSTRAINT_NAME, tc.CONSTRAINT_TYPE " +
                "From INFORMATION_SCHEMA.KEY_COLUMN_USAGE cu INNER JOIN " +
                "INFORMATION_SCHEMA.TABLE_CONSTRAINTS as tc ON " +
                "(cu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME) where tc.CONSTRAINT_TYPE = 'PRIMARY KEY') AS const " +
                "ON  " +
                "(col.TABLE_NAME = const.TABLE_NAME AND col.COLUMN_NAME = const.COLUMN_NAME) " +
                "Order by col.TABLE_NAME ASC, col.COLUMN_NAME ASC");

            var _dbColumns = ExecuteQuery(_query);
            return _dbColumns.Convert();
            
        }

        public List<DBColumn> ExecuteQuery(SqlQuery aQuery)
        {
            TemplateBinder<DBColumn> _binder = new TemplateBinder<DBColumn>();
            List<DBColumn> _r = new List<DBColumn>();
            _binder.OnBind = delegate(DBColumn aT)
                {
                    _r.Add(aT);
                };
            DBHelper.Execute(delegate(DbSession aDBSession)
                       {
                           aDBSession.Execute(aQuery, _binder);
                       });
            return _r;
        }

    }

    public interface DBTypeConverter
    {
        String ConvertType(String aDBType, bool isPrimaryKey);
        object GetDefaultValue(string aDBType);
    }

    public class MSSQLTypeConverter : DBTypeConverter
    {
        public object GetDefaultValue(string aDBType)
        {
            if (aDBType == "int" || aDBType == "smallint" || aDBType == "bigint")
                return 0;


            else if (aDBType == "bit")
                return true;


            else if (aDBType == "varchar"
                || aDBType == "nvarchar"
                || aDBType == "nchar"
                || aDBType == "ntext"
                || aDBType == "text"
                || aDBType == "char")
                return "";

            else if (aDBType == "float" || aDBType == "numeric")
                return 0.0;

            else if (aDBType == "smalldatetime"
                || aDBType == "date"
                || aDBType == "datetime"
                || aDBType == "datetime2")
                return DateTime.Now;

            else if (aDBType == "money"
                || aDBType == "decimal")
                return new decimal(0);
            else if (aDBType == "uniqueidentifier")
                return new Guid();
            else if (aDBType == "tinyint")
                return new byte();
            //TODO add more
            return null;

        }

        public string ConvertType(string aDBType, bool isPrimaryKey)
        {
            if (aDBType == "int" || aDBType == "smallint")
                return GetNullable("int", isPrimaryKey);

            else if (aDBType == "bigint")
                return GetNullable("long", isPrimaryKey);

            else if (aDBType == "bit")
                return GetNullable("bool", isPrimaryKey);

            else if (aDBType == "varchar"
                || aDBType == "nvarchar"
                || aDBType == "nchar"
                || aDBType == "ntext"
                || aDBType == "text"
                || aDBType == "char")
                return "String";

            else if (aDBType == "float")
                return GetNullable("float", isPrimaryKey);

            else if (aDBType == "numeric")
                return GetNullable("double", isPrimaryKey);

            else if (aDBType == "smalldatetime"
                || aDBType == "date"
                || aDBType == "datetime"
                || aDBType == "datetime2")
                return GetNullable("DateTime", isPrimaryKey);

            else if (aDBType == "money"
                || aDBType == "decimal")
                return GetNullable("decimal", isPrimaryKey);
            else if (aDBType == "uniqueidentifier")
                return GetNullable("Guid", isPrimaryKey);
            else if (aDBType == "tinyint")
                return GetNullable("byte", isPrimaryKey);
            //TODO add more
            return "NotConverted";

        }

        private String GetNullable(string aType, bool isPrimaryKey)
        {
            return aType + ((isPrimaryKey) ? "" : "?");
        }
    }


    public class TableStructure
    {
        public List<Column> Columns { set; get; }
        public String TableName { set; get; }
    }

    public class Column
    {
        public String Name { set; get; }
        public bool IsPrimaryKey { set; get; }
        public String Type { set; get; }
        public object DefaultValue { set; get; }
    }



    public class TableEqualityCompary : IEqualityComparer<DBColumn>
    {
        public bool Equals(DBColumn x, DBColumn y)
        {
            return x.TABLE_NAME == y.TABLE_NAME;
        }

        public int GetHashCode(DBColumn obj)
        {
            return obj.TABLE_NAME.GetHashCode();
        }
    }

    public static class Converter
    {

        public static IEnumerable<TableStructure> Convert<T>(this List<T> aColumns) where T : DBColumn, new()
        {
            return aColumns.Convert(true);
        }

        public static IEnumerable<TableStructure> Convert<T>(this List<T> aColumns, bool doConvertType) where T : DBColumn, new()
        {
            Dictionary<String, TableStructure> _tablesDic = new Dictionary<string, TableStructure>();
            DBTypeConverter _c = BaseFactory.Instance.GetInstance<DBTypeConverter>();

            foreach (DBColumn _t in aColumns)
            {
                TableStructure _n;
                if (_tablesDic.TryGetValue(_t.TABLE_NAME, out _n))
                {

                }
                else
                {
                    _n = new TableStructure();
                    _n.TableName = _t.TABLE_NAME;
                    _n.Columns = new List<Column>();
                    _tablesDic.Add(_n.TableName, _n);
                }

                bool _isPrimary = _t.CONSTRAINT_TYPE != null;
                _n.Columns.Add(new Column
                {
                    IsPrimaryKey = _isPrimary,
                    Name = _t.COLUMN_NAME,
                    Type = (doConvertType) ? _c.ConvertType(_t.DATA_TYPE, _isPrimary) : _t.DATA_TYPE ,
                    DefaultValue = (doConvertType) ? _c.GetDefaultValue(_t.DATA_TYPE) : null
                });
            }

            return _tablesDic.Values.ToList();
        }
    }

}
