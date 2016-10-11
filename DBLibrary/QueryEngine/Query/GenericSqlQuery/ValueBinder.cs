using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Statements;
using DBLibrary.Mapper;
using DBLibrary.Configuration;
using log4net;

namespace DBLibrary.QueryEngine.Query.GenericSqlQuery
{
 
    public interface ValueBinder
    {
        void BindValue(ParemeterStatement aStatement, PropertyMap aMap, Object aData, bool isBindNull);
    }

    class ValueBinderImpl : ValueBinder
    {
        private ValueExtractor ValueExtractor { get; set; }
        private readonly ILog logger = LogManager.GetLogger(typeof(DatabaseLogger));

        public ValueBinderImpl(ValueExtractor aValueExtractor)
        {
            ValueExtractor = aValueExtractor;
        }

        private const String ERROR = "When trying to bind property {0}, the data model supplied is null or  the property belongs to a composite which is null by itself. This will cause an exception of null instance to be thrown";
        public void BindValue(ParemeterStatement aStatement, PropertyMap aMap, Object aData,bool isBindNull)
        {
            if (aData == null)
            {
                logger.Debug(String.Format(ERROR,aMap.Member.Name));
            }
            Object value = ValueExtractor.GetValue(aMap.Member.Name, aData);
            if (!isBindNull && value == null)
                return;
            
            aStatement.AddField(aMap.GetColumn(), value);
        }
    }

}
