using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBLibrary.Session;
using Ninject;
using Ninject.Parameters;
using Loader;

namespace DBLibrary.Repository
{
    public delegate void Execute<R>(R aRepository)
        where R : Repository;

    public class DBRepository
    {
        private DbSession session;
        private DBHelperMulti DBHelperMulti;

        public DBRepository(DbSession aSession)
        {
            session = aSession;
            DBHelperMulti = new DBHelperMulti(aSession);
        }

        public void Execute<R>(Execute<R> aMultiRepoExecute)
            where R : Repository
        {
            var repo = BaseFactory.Instance.Kernel.Get<R>(
                new ConstructorArgument[] { 
                    new ConstructorArgument("aDBHelper", DBHelperMulti) });
            aMultiRepoExecute(repo);
        }
    }

    public class DBHelperMulti : DBHelper
    {
        private DbSession session;

        public DBHelperMulti(DbSession aSession)
        {
            session = aSession;
        }

        public void Execute(Execute anExecute, OnException anException)
        {
            anExecute(session);
        }

        public void Execute(Execute anExecute)
        {
            anExecute(session);
        }

        public void Execute(ExecuteRepo anExecute, OnException onException)
        {
            throw new NotImplementedException();
        }
    }
}