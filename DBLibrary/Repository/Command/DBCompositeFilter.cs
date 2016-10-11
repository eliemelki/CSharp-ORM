using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Expressions;

namespace DBLibrary.Repository.Command
{
    public enum DBLogicalOperator
    {
        And = 0,
        Or = 1,
    }

    public interface DBCompositeFilter<T> : DBFilterBase<T> where T : class, new()
    {
        List<DBFilterBase<T>> DBFilters { get; }
        DBLogicalOperator LogicalOperator { get; }
    }

    public class DBCompositeFilterImpl<TEntity> : DBCompositeFilter<TEntity> where TEntity : class , new()
    {
        public List<DBFilterBase<TEntity>> DBFilters { set; get; }

        public DBLogicalOperator LogicalOperator { set; get; }

        public QueryEngine.Expressions.Expression Expression()
        {
            Expression[] _expressions = new Expression[DBFilters.Count];
            for (int i = 0; i < DBFilters.Count; i++)
            {
                _expressions[i] = DBFilters[i].Expression();
            }

            switch (LogicalOperator)
            {
                case DBLogicalOperator.And:
                    return ExpressionHelper.And(_expressions);
                default:
                case DBLogicalOperator.Or:
                    return ExpressionHelper.Or(_expressions);
            }

        }
    }
}
