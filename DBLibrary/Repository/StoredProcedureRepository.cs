using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.Mapper;
using DBLibrary.QueryEngine.Query.Queries;
using DBLibrary.Configuration;
using DBLibrary.Mapper.ResultBinder;
using DBLibrary.Session;
using System.Data.SqlClient;
using DBLibrary.QueryEngine.Query;
using System.Data;
using log4net;

namespace DBLibrary.Repository
{
    public class StoredProcedure
    {
        public String Name { set; get; }
        public List<Column> Paramaters { set; get; }
        public TableStructure Result { set; get; }
    }

    public class DBStoredProcedureParamater : DBColumn
    {
        public String PARAMETER_NAME { set; get; } 

        public override String COLUMN_NAME
        {
            get {
             return PARAMETER_NAME.Replace("@","");
            }
        }
    }

    public class DBStoredProcedureParamaterClassMap : ClassMap<DBStoredProcedureParamater>
    {
        
        public DBStoredProcedureParamaterClassMap()
        {
            Map(this);
        }

        public static void Map<T>(ClassMap<T> aClassMap) where T : DBStoredProcedureParamater, new()
        {

            aClassMap.MapField(m => m.TABLE_SCHEMA).SetColumn("SPECIFIC_SCHEMA");
            aClassMap.MapField(m => m.TABLE_CATALOG).SetColumn("SPECIFIC_CATALOG");
            aClassMap.MapField(m => m.TABLE_NAME).SetColumn("SPECIFIC_NAME");
            aClassMap.MapField(m => m.PARAMETER_NAME).SetColumn("PARAMETER_NAME");
            aClassMap.MapField(m => m.DATA_TYPE).SetColumn("DATA_TYPE");

            aClassMap.SetTableName("");
        }
    }

    public interface StoredProcedureRepository
    {
        IEnumerable<StoredProcedure> GetStoredProcedures();

    }

    public class StoredProcedureRepositoryImpl : StoredProcedureRepository
    {
        protected DBHelper DBHelper;
        protected Config Config;
        public StoredProcedureRepositoryImpl(DBHelper aDBHelper,Config aConfig)
        {
            DBHelper = aDBHelper;
            Config = aConfig;
        }

        public List<DBStoredProcedureParamater> GetStoredProcedureParamaters()
        {
            var q = NativeQuery.SimpleQueryHelper("select r.SPECIFIC_SCHEMA, r.SPECIFIC_NAME, r.SPECIFIC_CATALOG, p.PARAMETER_NAME, p.DATA_TYPE from  DataBank.INFORMATION_SCHEMA.ROUTINES as r inner JOIN information_schema.PARAMETERS as p ON p.SPECIFIC_NAME = r.SPECIFIC_NAME where routine_type = @p0","PROCEDURE");

            TemplateBinder<DBStoredProcedureParamater> _binder = new TemplateBinder<DBStoredProcedureParamater>();
            List<DBStoredProcedureParamater> _r = new List<DBStoredProcedureParamater>();
            _binder.OnBind = delegate(DBStoredProcedureParamater aT)
            {
                _r.Add(aT);
            };
            DBHelper.Execute(delegate(DbSession aDBSession)
            {
                aDBSession.Execute(q, _binder);
            });
            return _r;

        }

        public IEnumerable<StoredProcedure> GetStoredProcedures()
        {
            List<DBStoredProcedureParamater> _dbspp = this.GetStoredProcedureParamaters();
            IEnumerable<TableStructure> _sps = _dbspp.Convert();
            return _sps.Convert(Config, DBHelper);
            
        }

      
    }


    public static class SConverter
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DatabaseLogger));

        public static IEnumerable<StoredProcedure> Convert(this IEnumerable<TableStructure> aStructure,Config aConfig, DBHelper aHelper) 
          {
              List<StoredProcedure> _r = new List<StoredProcedure>();
              foreach (TableStructure _st in aStructure)
              {
                  StoredProcedure _sp = new StoredProcedure();
                  _sp.Name = _st.TableName;
                  _sp.Paramaters = _st.Columns;
                  _sp.fetchResult(aConfig, aHelper);
                  _r.Add(_sp);
              }
              return _r;
          }


        public static void fetchResult(this StoredProcedure aSP, Config aConfig, DBHelper aHelper)
        {
            List<DBColumn> _result = new List<DBColumn>();
            var paramaters = GetParamaters(aSP.Paramaters);
            var query = NativeStoredProcedureQuery.GetSQLQuery(aSP.Name, paramaters);

            using (SqlConnection con = new SqlConnection(aConfig.DataSource))
            {
                try
                {
                    con.Open();
                    String q = query.Query;
                    q = "SET FMTONLY ON;" + q + ";SET FMTONLY OFF;";
                    using (SqlCommand Command = new SqlCommand(query.Query, con))
                    {
                        Command.Parameters.AddRange(query.Parameters.ToArray());
                        logger.Debug(q);

                        using (SqlDataReader aReader = Command.ExecuteReader())
                        {
                            DataTable _schema = aReader.GetSchemaTable();
                            if (_schema != null)
                            {

                                foreach (DataRow row in _schema.Rows)
                                {
                                    _result.Add(new DBColumn
                                    {
                                        TABLE_NAME = "SP" + aSP.Name,
                                        DATA_TYPE = row["DataType"].ToString(),
                                        COLUMN_NAME = row["BaseColumnName"].ToString()
                                    });


                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Debug(ex);
                    con.Close();
                    _result.Add(new DBColumn
                    {
                        TABLE_NAME = "SP" + aSP.Name,
                        DATA_TYPE = typeof(DataTable).ToString(),
                        COLUMN_NAME = "Result"
                    });

                    var q = NativeQuery.SimpleQueryHelper("SET FMTONLY OFF;");
                    aHelper.Execute(delegate(DbSession aDBSession)
                    {
                        aDBSession.Execute(q, new DataReaderBinder());
                    });

                }

                var r = _result.Convert(false);
                aSP.Result = r.Count() > 0 ? r.First() : null; 
            }
        }


          private static Dictionary<String, Object> GetParamaters(List<Column> aParamaters)
          {
              Dictionary<String, Object> _p = new Dictionary<String, Object>();
              foreach (Column _sp in aParamaters)
              {
                  String _spName = _sp.Name ;
                  _p.Add(_spName, _sp.DefaultValue);
              }

              return _p;
          }
    }

}
