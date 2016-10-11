using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Expressions;

namespace DBLibrary.Repository.Command
{
    public enum DBFilterOperator
    {
        IsLessThan = 0,
        IsLessThanOrEqualTo = 1,
        IsEqualTo = 2,
        IsNotEqualTo = 3,
        IsGreaterThanOrEqualTo = 4,
        IsGreaterThan = 5,
        StartsWith = 6,
        EndsWith = 7,
        Contains = 8,
        IsContainedIn = 9,
        DoesNotContain = 10,
    }


    public interface DBFilterBase<T> where T : class , new()
    {
        Expression Expression();
    }

    public interface DBFilter<T> : DBFilterBase<T> where T : class , new()
    {
        DBFilterOperator Operator { get; set; }

    }

    public class DBFilterImpl<TEntity, TType> : DBFilter<TEntity> where TEntity : class , new()
    {
        public TType Value { get; set; }
        public System.Linq.Expressions.Expression<Func<TEntity, TType>> Property { get; set; } 
        public DBFilterOperator Operator { get; set; }
        
        public DBFilterImpl()
        {
        }

        public Expression Expression()
        {
            DBExpression _exp = Loader.BaseFactory.Instance.GetInstance<DBExpression>();
            ExpressionHelper<TEntity> _expHelper = _exp.GetHelper<TEntity>();
            switch (Operator)
            {
                case DBFilterOperator.IsLessThan:
                   return _expHelper.LT(Property, Value);
                case DBFilterOperator.IsLessThanOrEqualTo:
                   return _expHelper.Lte(Property, Value);
                case DBFilterOperator.IsEqualTo:
                   return _expHelper.Eq(Property, Value);
                case DBFilterOperator.IsNotEqualTo:
                   return !_expHelper.Eq(Property, Value);
                case DBFilterOperator.IsGreaterThanOrEqualTo:
                   return _expHelper.Gre(Property, Value);
                case DBFilterOperator.IsGreaterThan:
                   return _expHelper.Gr(Property, Value);
                case DBFilterOperator.StartsWith:
                   return _expHelper.Like(Property, Value + "%");
                case DBFilterOperator.EndsWith:
                   return _expHelper.Like(Property, "%" + Value);
                case DBFilterOperator.Contains:
                   return _expHelper.Like(Property, "%" + Value + "%");
                case DBFilterOperator.IsContainedIn:
                   return _expHelper.In(Property, Value);
            }
            return null;
        }
    }
}
