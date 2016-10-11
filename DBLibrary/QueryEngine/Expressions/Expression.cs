using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DBLibrary.QueryEngine.Expressions
{
    public interface Expression
    {
        void AppendSqlString(StringBuilder aBuilder,List<SqlParameter> aParameters);
    }

    public abstract class AbstractExpression : Expression
    {
        public AbstractExpression()
        {
        }

        public static AbstractExpression operator &(AbstractExpression anExpression1, AbstractExpression anExpression2)
        {
            return new AndExpression(anExpression1, anExpression2);
        }

        public static AbstractExpression operator |(AbstractExpression anExpression1, AbstractExpression anExpression2)
        {
            return new OrExpression(anExpression1, anExpression2);
        }

        public static AbstractExpression operator !(AbstractExpression anExpression1)
        {
            return new NotExpression(anExpression1);
        }

        public abstract void AppendSqlString(StringBuilder aBuilder,List<SqlParameter> aParameters);

    }
}
