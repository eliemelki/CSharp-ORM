using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;
using System.Data.SqlClient;

namespace DBLibrary.QueryEngine.Statements
{
    public class DeleteStatement : AbstractStatement
    {
        protected override String GetSqlFromat()
        {
            return SqlSyntax.STATEMENT_DELETE;
        }

        protected override void GetData(List<Object> aData)
        {
            aData.Add(From);
        }

    }
}
