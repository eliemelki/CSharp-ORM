using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DBLibrary.Mapper
{
    public interface ValueExtractor
    {
        Object GetValue(String aMemberName, Object aData);
        void SetValue(String aMemberName, Object aData, Object aValue);
        Object GetValue( Object aData,params MemberInfo[] aMember);

    }


    class ValueExtractorImpl : ValueExtractor
    {
        public Object GetValue(String aMemberName, Object aData)
        {
            return aData.GetType().InvokeMember(
                         aMemberName,
                         System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.GetField,
                         null,
                         aData,
                         null);
        }
        public void SetValue(String aMemberName, Object aData, Object aValue)
        {
            aData.GetType().InvokeMember(
                        aMemberName,
                        System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.SetField,
                        null,
                        aData,
                        new Object[] { aValue });

        }

        public Object GetValue(Object aData, params MemberInfo[] aMember)
        {
            var _value = aData;
            foreach (MemberInfo _parent in aMember)
            {
                _value = GetValue(_parent.Name, _value);                
            }
            return _value;
        }
    }
}
