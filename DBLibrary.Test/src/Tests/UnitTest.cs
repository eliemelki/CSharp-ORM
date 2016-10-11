using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Tables;
using DBLibrary.Session;
using DBLibrary.Repository;

namespace DatabaseTest
{
    [TestClass]
    public class UnitTest : AbstractUnitTest
    {

        [TestMethod]
        public void SingleTest()
        {
            var dbHelper = Factory.GetDBHelper();
            dbHelper.Execute(delegate (DbSession session)
            {
                var portal = session.GetSingle<Portal>();
                Assert.Equals(portal.Title, "title");
            });
        }

        [TestMethod]
        public void SingleUsingRepositoryTest()
        {
            var dbHelper = Factory.GetDBHelper();
            var rep = new GenericTableRepository<Portal>(dbHelper);
            var portal = rep.GetSingle(null, null);
            Assert.Equals(portal.Title, "title");
        }

        [TestMethod]
        public void SimpleSelectTest()
        {
            var dbHelper = Factory.GetDBHelper();
            dbHelper.Execute(delegate (DbSession session)
            {
                var portals = session.Get<Portal>();
                Assert.Equals(portals.Count, "title");
            });
        }

        [TestMethod]
        public void SimpleSelectUsingRepositoryTest()
        {
            var dbHelper = Factory.GetDBHelper();
            var rep = new GenericTableRepository<Portal>(dbHelper);
            var portals = rep.Get();
            Assert.Equals(portals.Count, "title");
        }


        [TestMethod]
        public void CriteriaSelectTest()
        {
            var dbHelper = Factory.GetDBHelper();
            var exp = Factory.GetExpressionHelper<Portal>();
            var criteria = Factory.CreateCriteria();
            dbHelper.Execute(delegate (DbSession session)
            {
                criteria.Add(exp.Eq(m => m.Title, "title")).Add(!exp.Eq(m => m.Http, "http://"));
                var portals = session.Get<Portal>(criteria);
                Assert.Equals(portals.Count, "title");
            });
        }

        [TestMethod]
        public void CriteriaSelectUsingRepositoryTest()
        {
            var dbHelper = Factory.GetDBHelper();
            var rep = new GenericTableRepository<Portal>(dbHelper);
            var criteria = rep.GetCriteria();
            var exp = rep.ExpressionHelper;
            criteria.Add(exp.Eq(m => m.Title, "title")).Add(!exp.Eq(m => m.Http, "http://"));
            var portals = rep.Get(criteria);
            Assert.Equals(portals.Count, "title");
        }

        [TestMethod]
        public void SaveTest()
        {
            var dbHelper = Factory.GetDBHelper();
            var model = new Portal
            {
                Title = "title",
                Http = "http://"
            };
            dbHelper.Execute(delegate (DbSession session)
            {
                session.Save(model);
            });
            Assert.IsNotNull(model.id);

            model.Title = "Changed Title";
            dbHelper.Execute(delegate (DbSession session)
            {
                session.Save(model);
            });
            Assert.Equals(model.Title, "Changed Title");

        }

        [TestMethod]
        public void SaveUsingRepositoryTest()
        {
            var dbHelper = Factory.GetDBHelper();
            var rep = new GenericTableRepository<Portal>(dbHelper);
            var model = new Portal
            {
                Title = "title",
                Http = "http://"
            };
            rep.Save(model);
            Assert.IsNotNull(model.id);

            model.Title = "Changed Title";
            rep.Save(model);
            Assert.Equals(model.Title, "Changed Title");
        }

        [TestMethod]
        public void DeleteTest()
        {
            var dbHelper = Factory.GetDBHelper();
            var model = new Portal
            {
                id = 1
            };
            dbHelper.Execute(delegate (DbSession session)
            {
                session.Delete(model);
            });

        }

        [TestMethod]
        public void DeleteUsingRepositoryTest()
        {
            var dbHelper = Factory.GetDBHelper();
            var rep = new GenericTableRepository<Portal>(dbHelper);
            var model = new Portal
            {
                id = 1
            };
            rep.Delete(model);
        }

        [TestMethod]
        public void rollback()
        {
            var dbHelper = Factory.GetDBHelper();
            var count = 0;
            dbHelper.Execute(delegate (DbSession Session)
            {
                count = Session.Count<Portal>();
            });

            dbHelper.Execute(

                delegate (DbSession aDBSession)
                {

                    var model = new Portal
                    {
                        Title = "title",
                        Http = "http://"
                    };
                    dbHelper.Execute(delegate (DbSession session)
                    {
                        session.Save(model);
                    });

                    throw new Exception("");
                },
                delegate (Exception anExc)
                {
                    System.Console.Out.Write(anExc);
                });

            var newCount = 0;
            dbHelper.Execute(delegate (DbSession Session)
            {
                newCount = Session.Count<Portal>();
            });

            Assert.Equals(count, newCount);
        }

        [TestMethod]
        public void rollbackUsingRepository()
        {
            var dbHelper = Factory.GetDBHelper();
            var rep = new GenericTableRepository<Portal>(dbHelper);
            var count = rep.GetCount(null, null);

            dbHelper.Execute(

                delegate (DBRepository aDBRepository)
                {
                    aDBRepository.Execute(
                        delegate (GenericTableRepository<Portal> repo)
                        {
                            Portal _news = new Portal();
                            _news.Title = "Elie";
                            repo.Insert(_news);
                            repo.Insert(_news);
                        });
                    aDBRepository.Execute(
                        delegate (GenericTableRepository<Menu> repo)
                        {
                            Menu menu = new Menu();
                            menu.Name = "test";
                            repo.Insert(menu);
                            repo.Insert(menu);
                            throw new Exception("Throw exception");
                        });
                },
                delegate (Exception anExc)
                {
                    System.Console.Out.Write(anExc);
                });
            var newCount = rep.GetCount(null, null);
            Assert.Equals(count, newCount);
        }
    }
}
