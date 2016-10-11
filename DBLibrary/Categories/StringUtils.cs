using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBLibrary.Categories
{
    public static class StringUtils
    {
        public static String RemoveLastCharacter(this String aString, String toRemove)
        {
            if (aString.Length > 0 && aString.LastIndexOf(toRemove) == aString.Length - 1)
            {
                aString = aString.Substring(0, aString.Length - 1);
            }
            return aString;
        }
    }
}
