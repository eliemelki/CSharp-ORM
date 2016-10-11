using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.QueryEngine.Query;
using System.Linq.Expressions;
using DBLibrary.Mapper;

namespace DBLibrary.QueryEngine.Expressions
{
    public enum SortType { ASC, DESC } ;


    public class Orders : SqlString
    {
        public List<Order> OrdersList { private set; get; }

        public Orders()
        {
            OrdersList = new List<Order>();
        }

        public void Add(Order anOrder)
        {
            OrdersList.Add(anOrder);
        }

        public String GetSqlString()
        {   

            int count = OrdersList.Count;
            if (count <= 0) return "";

            StringBuilder _queryBuilder = new StringBuilder();
            _queryBuilder.Append(SqlSyntax.ORDER);

            int i = 0;
            foreach (Order order in OrdersList)
            {

                _queryBuilder.Append(order.GetSqlString());
                if (i != count - 1)
                {
                    _queryBuilder.Append(SqlSyntax.COMMA);
                }

                i++;
            }

            return _queryBuilder.ToString();
        }
    }

    public class Order : SqlString
    {

        private String Property;
        private SortType sort;

        public Order(String aProperty, SortType aSorted)
        {
            Property = aProperty;
            sort = aSorted;
        }

        public String GetSqlString()
        {
            StringBuilder builder = new StringBuilder();

            //{0} Asc
            builder.Append(Property);
            builder.Append(SqlSyntax.SPACE);
            builder.Append(sort);
            return builder.ToString();
        }
    }
}
