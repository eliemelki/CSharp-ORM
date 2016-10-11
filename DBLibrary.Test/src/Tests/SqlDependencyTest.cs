using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Tables;
using System.Threading;
using DBLibrary.Mapper;
using DBLibrary.QueryEngine.Query;
using Loader;
using DBLibrary.Configuration;
using DBLibrary;
using DBLibrary.Repository;

namespace DatabaseTest
{
    [TestClass]
    public class SqlDependencyTest : AbstractUnitTest, IDisposable
    {
        private SqlCacheDependencyEngine SqlDependencyEngine;

        public SqlDependencyTest()
        {
            SqlDependencyEngine = Factory.GetSqlDependencyEngine();
            SqlDependencyEngine.Start(typeof(Portal));
        }


        [TestMethod]
        public void TestMethod1()
        {
            System.Console.WriteLine("DependencyTablesNames");
            foreach (String _name in SqlDependencyEngine.DependencyTablesNames)
            {
                System.Console.WriteLine(_name);
            }

            System.Console.WriteLine("DependencyTablesType");
            foreach (Type _type in SqlDependencyEngine.DependencyTablesType)
            {
                System.Console.WriteLine(_type);
            }

            System.Console.WriteLine("RegisteredSqlDependencyTablesNames");
            foreach (String _name in SqlDependencyEngine.RegisteredSqlDependencyTablesNames)
            {
                System.Console.WriteLine(_name);
            }
          
        }

        [TestMethod]
        public void CacheTest()
        {
            CachePreferenceImpl Cache = new CachePreferenceImpl(true, 30, true);
            var dbHelper = Factory.GetDBHelper();
            var Repository = new GenericTableRepository<Portal>(dbHelper);

            var _result = Repository.Get(null, Cache);
            Assert.IsNotNull(_result);

            var _result1 = Repository.Get(null, Cache);
            Assert.AreEqual(_result.Count, _result1.Count);

            Repository.Insert(_result.First());
            Thread.Sleep(1000);

            var _result3 = Repository.Get(null, Cache);
            Assert.AreNotEqual(_result.Count, _result3.Count);

            var _result4 = Repository.Get(null, Cache);
            Assert.AreEqual(_result4.Count, _result3.Count);

        }

        public void Dispose()
        {
            SqlDependencyEngine.Stop();
        }
    }
}
