using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;

namespace DBLibrary.QueryEngine.Expressions
{
    public class ExpressionHelper
    {
        public static Order OrderAsc(String aProperty)
        {
            return new Order(aProperty, SortType.ASC);
        }

        public static Order OrderDes(String aProperty)
        {
            return new Order(aProperty, SortType.DESC);
        }

        public static ExpressionSimple Eq(String aProperty, Object aValue)
        {
            return new ExpressionSimple(SqlSyntax.EQ, aProperty, aValue);
        }

        public static ExpressionSql IsNull(String aProperty)
        {
            return new ExpressionSql(String.Format("{0} {1}", aProperty, SqlSyntax.ISNULL));
        }

        public static ExpressionSimple Like(String aProperty, Object aValue)
        {
            return new ExpressionSimple(SqlSyntax.LIKE, aProperty, aValue);
        }


        public static ExpressionSimple LT(String aProperty, Object aValue)
        {
            return new ExpressionSimple(SqlSyntax.LT, aProperty, aValue);
        }

        public static ExpressionSimple Gr(String aProperty, Object aValue)
        {
            return new ExpressionSimple(SqlSyntax.GR, aProperty, aValue);
        }

        public static ExpressionSimple Lte(String aProperty, Object aValue)
        {
            return new ExpressionSimple(SqlSyntax.LTE, aProperty, aValue);
        }

        public static ExpressionSimple Gre(String aProperty, Object aValue)
        {
            return new ExpressionSimple(SqlSyntax.GRE, aProperty, aValue);
        }

        public static ExpressionIn In(String aProperty, params Object[] aValue)
        {
            return new ExpressionIn(aProperty, aValue);
        }

        public static ExpressionBetween Between(String aProperty, Object aValue1,Object aValue2)
        {
            return new ExpressionBetween(aProperty,  SqlSyntax.GR, aValue1,SqlSyntax.LT, aValue2);
        }

        public static ExpressionBetween BetweenGteLt(String aProperty, Object aValue1, Object aValue2)
        {
            return new ExpressionBetween(aProperty, SqlSyntax.GRE, aValue1, SqlSyntax.LT, aValue2);
        }

        public static ExpressionBetween BetweenGtLte(String aProperty, Object aValue1, Object aValue2)
        {
            return new ExpressionBetween(aProperty, SqlSyntax.GR, aValue1, SqlSyntax.LTE, aValue2);
        }

        public static ExpressionBetween BetweenGteLte(String aProperty, Object aValue1, Object aValue2)
        {
            return new ExpressionBetween(aProperty, SqlSyntax.GRE, aValue1, SqlSyntax.LTE, aValue2);
        }

        public static ExpressionBetween BetweenLtGt(String aProperty, Object aValue1, Object aValue2)
        {
            return new ExpressionBetween(aProperty, SqlSyntax.LT, aValue1, SqlSyntax.GR, aValue2);
        }

        public static ExpressionBetween BetweenLteGr(String aProperty, Object aValue1, Object aValue2)
        {
            return new ExpressionBetween(aProperty, SqlSyntax.LTE, aValue1, SqlSyntax.GR, aValue2);
        }

        public static ExpressionBetween BetweenLtGre(String aProperty, Object aValue1, Object aValue2)
        {
            return new ExpressionBetween(aProperty, SqlSyntax.LT, aValue1, SqlSyntax.GRE, aValue2);
        }

        public static ExpressionBetween BetweenLteGre(String aProperty, Object aValue1, Object aValue2)
        {
            return new ExpressionBetween(aProperty, SqlSyntax.LTE, aValue1, SqlSyntax.GRE, aValue2);
        }

        public static AbstractExpression And(Expression anExpression1, Expression anExpression2)
        {
            return new AndExpression(anExpression1,anExpression2);
        }

        public static AbstractExpression Or(Expression anExpression1, Expression anExpression2)
        {
            return new OrExpression(anExpression1, anExpression2);
        }

        public static AbstractExpression And(params Expression[] aExpressions)
        {
            return new AndExpression(aExpressions);
        }

        public static AbstractExpression Or(params Expression[] aExpressions)
        {
            return new OrExpression(aExpressions);
        }

        public static AbstractExpression Not(Expression anExpression1)
        {
            return new NotExpression(anExpression1);
        }

        public static AbstractExpression Sql(String anSqlString)
        {
            return new ExpressionSql(anSqlString);
        }

        public static ExpressionBoolean BetweenLtGt(String aProperty, String anOp, String aFormatFunction , params Object[] aValues)
        {
            return new ExpressionBoolean(aProperty, anOp, aFormatFunction, aValues);
        }

        public static Group GroupBy(params String[] aFields)
        {
            return new Group(aFields);
        }
    }
}
