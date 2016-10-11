using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Expressions;
using System.ComponentModel;

namespace DBLibrary.Repository.Command
{
   
    public interface DBSort<T> where T : class , new()
    {
        Order GetOrder();
        ListSortDirection SortDirection { get; set; }
    }

    public class DBSortImpl<TEntity, TType> : DBSort<TEntity> where TEntity : class , new()
    {
        public System.Linq.Expressions.Expression<Func<TEntity, TType>> Property { get; set; }
        public ListSortDirection SortDirection { get; set; }

        public Order GetOrder()
        {
            DBExpression _exp = Loader.BaseFactory.Instance.GetInstance<DBExpression>();
            ExpressionHelper<TEntity> _expHelper = _exp.GetHelper<TEntity>();
           
            switch (SortDirection)
            {
                case ListSortDirection.Ascending:
                    return _expHelper.OrderAsc<TType>(Property);
                case ListSortDirection.Descending:
                    return _expHelper.OrderDes<TType>(Property);
            }
            return null;
        }
    }
}
