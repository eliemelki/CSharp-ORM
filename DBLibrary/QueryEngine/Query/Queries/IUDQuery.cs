using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Statements;
using DBLibrary.Mapper.ResultBinder;
namespace DBLibrary.QueryEngine.Query.Queries
{
    public class InsertQuery : AbstractQuery<InsertQuery, InsertStatement>
    {
        public InsertQuery()
        {
        }

        protected override InsertQuery GetThis()
        {
            return this;
        }
    }

    public class UpdateQuery : CriteriaQuery<UpdateQuery, UpdateStatement>
    {
        public UpdateQuery()
        {
        }

        protected override UpdateQuery GetThis()
        {
            return this;
        }
    }

    public class DeleteQuery : CriteriaQuery<DeleteQuery, DeleteStatement>
    {
        public DeleteQuery()
        {
        }

        protected override DeleteQuery GetThis()
        {
            return this;
        }
    }
}
