using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Tables;
using Loader;
using Ninject;
using Ninject.Modules;
using DBLibrary.Configuration;
using DBLibrary;

namespace DatabaseTest
{
    public class AbstractUnitTest
    {
        public  SqlFactory Factory;

        public AbstractUnitTest()
        {
            var sqlModule = new DBLibrary.Configuration.EntryPoint(typeof(Portal), "DBConnect");
            var kernel = new StandardKernel(sqlModule.GetModules());
            BaseFactory.Instance = new BaseFactory(kernel);
            Factory = Database.Current.Factory;
            sqlModule.Load();
        }
    }
}
