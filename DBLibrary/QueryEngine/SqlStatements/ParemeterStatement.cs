using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DBLibrary.QueryEngine.Query;
using DBLibrary.QueryEngine.Expressions;

namespace DBLibrary.QueryEngine.Statements
{
    public abstract class ParemeterStatement : AbstractStatement
    {
        protected Dictionary<String, Object> FieldValue;
        protected ParemetersHelper ParemeterHelper;
        public ParemeterStatement()
        {
            FieldValue = new Dictionary<String, Object>();
            ParemeterHelper =  ParemetersHelperFactory.GetParemeterHelper();
        }

        protected void AddExpressionParameters(Object aValue)
        {
            Parameters.Add(ParemeterHelper.GetSqlParemeter(this.ParemeterCount, aValue));
            ParemeterCount++;
        }


        public virtual ParemeterStatement AddField(String aField, Object aValue)
        {
            FieldValue.Add(aField, aValue);
            return this;
        }

        public virtual ParemeterStatement AddField(Dictionary<String, Object> aFieldsValues)
        {
            Dictionary<String, Object>.Enumerator enumerator = aFieldsValues.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<String, Object> keyValue = enumerator.Current;
                aFieldsValues.Add(keyValue.Key, keyValue.Value);
            }
            return this;
        }

        protected abstract String GetParemetrized();

        protected override void GetData(List<Object> aData)
        {
            aData.Add(From);
            aData.Add(GetParemetrized());
        }
    }
}
