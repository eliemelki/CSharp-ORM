# ORM

ORM library is an Object Relation Model library that maps objects to database tables. It uses expression to build queries.

<b>Last change was since 2013.</b>


## Table of Contents

* [Initialisation](#initialisation)
* [Mapping](#mapping)
	* [Simple Mapping](#class-mapping)
	* [Map Generator tool](#map-generator-tool)
		* [Table](#table-generator-tool)
		* [Stored Procedure](#stored-procedure-generator-tool)
* [Queries](#queries)
	* [Select](#select-query)	
	* [Save](#save-query)
	* [Delete](#delete-query)
* [Caching](#caching) 
	

## <a name="initialisation"></a>Initialisation

The library expect a connection string and data model type. Will explain in the next section the use of data model type. 

Here is an example:

```
     var sqlModule = new DBLibrary.Configuration.EntryPoint(typeof(Portal), "CONNECTION_STRING");
     var kernel = new StandardKernel(sqlModule.GetModules());
     BaseFactory.Instance = new BaseFactory(kernel);
     sqlModule.Load();

```

## <a name="mapping"></a>Mapping

As stated earlier the library expect a data model type. Usually this reflect one of your Mapping Table classes. This needed so the library can find the ClassMap classes for a specific dll and load them upfront. 


### <a name="class-mapping"></a>Simple Mapping

Here is a sample of mapping a class to table. We also support having composition. 

```
 public class Portal 
    {
        public Portal()
        {
          
        }

        public int id { set; get; }
        public String Title { set; get; }
        public String Http { set; get; }
    }

    public class PortalMap : ClassMap<Portal>
    {
        public PortalMap()
        {
            MapIdentity(m => m.id).SetColumn("PortalID");
            MapField(m => m.Title).SetColumn("PortalTitle");
            MapField(m => m.Http).SetColumn("PortalHTTP");
            SetTableName("Portal");

        }
    }
```

### <a name="map-generator-tool"></a>Map Generator tool

In order not to waste time on creating Classes and there appropriate Database Table mapping. The DBLibrary.Tools can be used to auto-generate classes and there correspoding database tables and views  mapping. We aslo support of generating stored procedure mapping.

##### <a name="table-generator-tool"></a>Table/Views

Here is a sample of how to generate database table/views mapping. Check DBlibrary.Tools.Test for an example.

```
  ClassMapGenerator _g = BaseFactory.Instance.GetInstance<ClassMapGenerator>();
  bool _result = _g.GenerateClassMap("ClassLibrary1.Tables.generated", "PATH_TO_GENERATED_CLASSES");
  Assert.AreEqual(true, _result); 
```

##### <a name="stored-procedure-generator-tool"></a>Stored Procedure

Here is a sample of how to generate database stored procedure mapping. Check DBlibrary.Tools.Test for an example.

```
  StoreProcedureGenerator _g =     BaseFactory.Instance.GetInstance<StoreProcedureGenerator>();
bool _result = _g.GenerateClassMap("ClassLibrary1.DBStoredProcedure.generated", "PATH_TO_GENERATED_STORED_PROCEDURES");
Assert.AreEqual(true, _result);

```

## <a name="queries"></a>Queries

### <a name="select-query"></a>Select

```
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

```
### <a name="save-query"></a>Save

```
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
```

### <a name="delete-query"></a>Delete

```
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
```
### <a name="rollback"></a>Roll Back

```
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

```



## <a name="caching"></a>Caching

ORM support caching. We Support Sql Dependency Caching or InMemory caching. 

Here is an example of how to enable and start SqlDependency Caching 

```
//First you need to start sql dependency and provide which tables to monitor at very start
 SqlDependencyEngine = Factory.GetSqlDependencyEngine();
 SqlDependencyEngine.Start(typeof(Portal));
 

... 
//This how you stop 
SqlDependencyEngine.Stop();
            
```

Form now on if you require to execute a query using sql dependency 

```
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


```

if you wish to use in Memory we dont need to start the sql dependency caching. 


```
			//you set the last parameter to false to indicate no sql dependeny caching. 

 			CachePreferenceImpl Cache = new CachePreferenceImpl(true, 30, false);
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


```
