using DBLibrary.Tools.Src.ClassMapGenerator;
using DBLibrary.Tools.Src.StoreProcedureGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DBLibrary.Repository;
using Loader;

namespace DBLibrary.Tool.Test
{
    
    
    /// <summary>
    ///This is a test class for ClassMapGeneratorImplTest and is intended
    ///to contain all ClassMapGeneratorImplTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ClassMapGeneratorImplTest
    {

         [TestMethod()]
        public void GenerateClassMapTest()
        {
            ClassMapGenerator _g = BaseFactory.Instance.GetInstance<ClassMapGenerator>();
            bool _result = _g.GenerateClassMap("ClassLibrary1.Tables.generated", "PATH_TO_GENERATED_CLASSES");
            Assert.AreEqual(true, _result); 
        }

         [TestMethod()]
         public void GenerateStoredProcedureTest()
         {
            
             StoreProcedureGenerator _g = BaseFactory.Instance.GetInstance<StoreProcedureGenerator>();
             bool _result = _g.GenerateClassMap("ClassLibrary1.DBStoredProcedure.generated", "PATH_TO_GENERATED_STORED_PROCEDURES");
             Assert.AreEqual(true, _result);
         }
    }
}
