using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;
using System.Data.SqlClient;
using DBLibrary.Configuration;

namespace DBLibrary.Session.Cache
{
    public interface CacheKeyGenerator
    {
        String GenerateKey(SqlQuery aQuery);

        //Rember this depend on queryTracker which is instaniate on a thread Scope;
        String GetLastQueryExecutedKey();
    }

    class CacheKeyGeneratorImpl : CacheKeyGenerator
    {
        private SqlFactory Factory;
        
        public CacheKeyGeneratorImpl(SqlFactory aFactory)
        {
            Factory = aFactory;
        }

        public String GenerateKey(string aQuery)
        {
            return aQuery.GetHashCode().ToString();
        }

        public string GenerateKey(SqlQuery aQuery)
        {
            String query = aQuery.Query;
            foreach (SqlParameter _parameter in aQuery.Parameters)
            {
                query += _parameter.Value;
                query += "|";
            }
            return query.GetHashCode().ToString();
        }


        public string GetLastQueryExecutedKey()
        {
            var _tracker = Factory.GetQueryTracker();
            SqlQuery _query = _tracker.GetLastExecutedQuery();
            return GenerateKey(_query);
        }
    }
}
