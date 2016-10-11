using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using DBLibrary.Configuration;

namespace DBLibrary.Mapper.ResultBinder
{

    public class ObjectBinder : AbstractBinder<Object>
    {
        protected override object Binding(System.Data.SqlClient.SqlDataReader aReader)
        {
            return aReader.GetValue(0);
        }

    }

    public class IntResult
    {
        public int value { set; get; }
    }

    public class IntBinder : AbstractBinder<IntResult>
    {
        protected override IntResult Binding(System.Data.SqlClient.SqlDataReader aReader)
        {
            int value = int.Parse(aReader.GetValue(0).ToString());
            IntResult result = new IntResult();
            result.value = value;
            return result;
        }      
    }
}
