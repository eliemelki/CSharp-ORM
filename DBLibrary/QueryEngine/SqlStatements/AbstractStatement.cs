using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;
using System.Data.SqlClient;


namespace DBLibrary.QueryEngine.Statements
{
  
    public abstract class AbstractStatement 
    {
 
        
        protected String From { set; get; }
        protected int ParemeterCount;

        protected abstract String GetSqlFromat();
        
        protected Object[] GetData()
        {
            List<Object> data = new List<Object>();
            GetData(data);
            return data.ToArray();
        }

        protected abstract void GetData(List<Object> aData);


        public string GetSqlString(int aParemeterCount)
        {
            Parameters = new List<SqlParameter>();
            ParemeterCount = aParemeterCount;
            return String.Format(GetSqlFromat(), GetData());
        }

        public List<SqlParameter> Parameters { get; private set; }

        public AbstractStatement AddFrom(String aFrom)
        {
            From = aFrom;
            return this;
        }
    }
}
