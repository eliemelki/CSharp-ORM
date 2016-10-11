using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;
namespace DBLibrary.QueryEngine.Statements
{
    public class SelectStatement : SqlStatementFields
    {
        protected override String GetDefaultField()
        {
            return SqlSyntax.DEFAULT_FIELD;
        }
    }
}
