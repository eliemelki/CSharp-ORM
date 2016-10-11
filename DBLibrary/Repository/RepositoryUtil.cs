using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBLibrary.Repository
{
    public class RepositoryUtil
    {
        public static DateTime GetDate(int aDays)
        {
            return DateTime.Parse(DateTime.Now.AddDays(aDays).ToShortDateString());
        }
    }
}