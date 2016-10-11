using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DBLibrary.QueryEngine.Query;
using DBLibrary.Mapper;
using DBLibrary.Configuration;
using System.Reflection;

namespace DBLibrary.QueryEngine.Expressions
{
    public class DBExpression
    {
        public ExpressionHelper<TEntity> GetHelper<TEntity>() where TEntity : class, new()
        {
            return new ExpressionHelper<TEntity>();
        }
    }

    public class ExpressionHelper<TEntity> where TEntity : class, new()
    {

        public Order OrderAsc<TType>(Expression<Func<TEntity, TType>> aProperty)
        {
            return new Order(GetField(aProperty), SortType.ASC);
        }

        public Order OrderDes<TType>(Expression<Func<TEntity, TType>> aProperty)
        {
            return new Order(GetField(aProperty), SortType.DESC);
        }

        private String GetField(String aTableName, String aColumn)
        {
            return String.Format(SqlSyntax.DBFIELD, aTableName, aColumn);
        }

        public String GetField(PropertyMap aMap)
        {
            ClassMapLoader loader = Database.Current.Factory.GetClassMapLoader();
            ClassMap<TEntity> classmap = loader.GetClassMap<TEntity>();
            return GetField(classmap.GetTableName(), aMap.GetColumn());
        }

        public String GetCountField<T>()
        {
            ClassMapLoader loader = Database.Current.Factory.GetClassMapLoader();
            ClassMap<TEntity> classmap = loader.GetClassMap<TEntity>();
            if (classmap.Identity != null)
            {
                var _idField = GetField(classmap.Identity);
                return String.Format(SqlSyntax.COUNT_ID_QUERY, _idField);
            }
            else
                return String.Format(SqlSyntax.COUNT_QUERY);
        }

        public String GetField<TType>(Expression<Func<TEntity, TType>> anExpression)
        {
            ClassMapLoader loader = Database.Current.Factory.GetClassMapLoader();
            ClassMap<TEntity> classmap = loader.GetClassMap<TEntity>();
            PropertyMap property = classmap.GetProperty(GetMemberExpression(anExpression));
            return GetField(classmap.GetTableName(), property.GetColumn());
        }

        public String GetTable()
        {
            ClassMapLoader loader = Database.Current.Factory.GetClassMapLoader();
            ClassMap<TEntity> classmap = loader.GetClassMap<TEntity>();
            return String.Format(SqlSyntax.DBTABLE, classmap.GetTableName());
        }

        public  ExpressionSimple Eq<TType>(Expression<Func<TEntity, TType>> anExpression, TType aValue)
        {
            return new ExpressionSimple(SqlSyntax.EQ, GetField(anExpression), aValue);
        }

        public ExpressionSql IsNull<TType>(Expression<Func<TEntity, TType>> anExpression)
        {
            return ExpressionHelper.IsNull(GetField(anExpression));
        }

        public ExpressionSimple Eq<TType>(Expression<Func<TEntity, TType>> anExpression, Object aValue)
        {
            return new ExpressionSimple(SqlSyntax.EQ, GetField(anExpression), aValue);
        }

        public  ExpressionSimple Like<TType>(Expression<Func<TEntity, TType>> anExpression, Object aValue)
        {
            return new ExpressionSimple(SqlSyntax.LIKE, GetField(anExpression), aValue);
        }


        public  ExpressionSimple LT<TType>(Expression<Func<TEntity, TType>> anExpression, TType aValue)
        {
            return new ExpressionSimple(SqlSyntax.LT, GetField(anExpression), aValue);
        }

        public  ExpressionSimple Gr<TType>(Expression<Func<TEntity, TType>> anExpression, TType aValue)
        {
            return new ExpressionSimple(SqlSyntax.GR, GetField(anExpression), aValue);
        }

        public  ExpressionSimple Lte<TType>(Expression<Func<TEntity, TType>> anExpression, TType aValue)
        {
            return new ExpressionSimple(SqlSyntax.LTE, GetField(anExpression), aValue);
        }

        public  ExpressionSimple Gre<TType>(Expression<Func<TEntity, TType>> anExpression, TType aValue)
        {
            return new ExpressionSimple(SqlSyntax.GRE, GetField(anExpression), aValue);
        }

        public  ExpressionIn In<TType>(Expression<Func<TEntity, TType>> anExpression, params Object[] aValue) 
        {
            return new ExpressionIn(GetField(anExpression), aValue);
        }

        public  ExpressionBetween Between<TType>(Expression<Func<TEntity, TType>> anExpression, TType aValue1, TType aValue2)
        {
            return new ExpressionBetween(GetField(anExpression), SqlSyntax.GR, aValue1, SqlSyntax.LT, aValue2);
        }

        public  ExpressionBetween BetweenGteLt<TType>(Expression<Func<TEntity, TType>> anExpression, TType aValue1, TType aValue2)
        {
            return new ExpressionBetween(GetField(anExpression), SqlSyntax.GRE, aValue1, SqlSyntax.LT, aValue2);
        }

        public  ExpressionBetween BetweenGtLte<TType>(Expression<Func<TEntity, TType>> anExpression, TType aValue1, TType aValue2)
        {
            return new ExpressionBetween(GetField(anExpression), SqlSyntax.GR, aValue1, SqlSyntax.LTE, aValue2);
        }

        public  ExpressionBetween BetweenGteLte<TType>(Expression<Func<TEntity, TType>> anExpression, TType aValue1, TType aValue2)
        {
            return new ExpressionBetween(GetField(anExpression), SqlSyntax.GRE, aValue1, SqlSyntax.LTE, aValue2);
        }

        public  ExpressionBetween BetweenLtGt<TType>(Expression<Func<TEntity, TType>> anExpression, TType aValue1, TType aValue2)
        {
            return new ExpressionBetween(GetField(anExpression), SqlSyntax.LT, aValue1, SqlSyntax.GR, aValue2);
        }

        public  ExpressionBetween BetweenLteGr<TType>(Expression<Func<TEntity, TType>> anExpression, TType aValue1, TType aValue2)
        {
            return new ExpressionBetween(GetField(anExpression), SqlSyntax.LTE, aValue1, SqlSyntax.GR, aValue2);
        }

        public  ExpressionBetween BetweenLtGre<TType>(Expression<Func<TEntity, TType>> anExpression, TType aValue1, TType aValue2)
        {
            return new ExpressionBetween(GetField(anExpression), SqlSyntax.LT, aValue1, SqlSyntax.GRE, aValue2);
        }

        public  ExpressionBetween BetweenLteGre<TType>(Expression<Func<TEntity, TType>> anExpression, TType aValue1, TType aValue2)
        {
            return new ExpressionBetween(GetField(anExpression), SqlSyntax.LTE, aValue1, SqlSyntax.GRE, aValue2);
        }

        public ExpressionBoolean BooleanExpression<TType>(Expression<Func<TEntity, TType>> anExpression, String anOp, String aFormatFunction, params Object[] aValues)
        {
            return new ExpressionBoolean(GetField(anExpression), anOp, aFormatFunction, aValues);
        }

        public String GetMemberExpressionASString<TType>(Expression<Func<TEntity, TType>> anExpression)
        {
            MemberExpression expression = (MemberExpression)anExpression.Body;
            int _indexAfterEntity = expression.Expression.ToString().IndexOf('.');
            if (_indexAfterEntity < 0)
                _indexAfterEntity = expression.Expression.ToString().Length;

            String _expBody = expression.ToString();
            return _expBody.Remove(0, _indexAfterEntity + 1);
        }

        public MemberInfo GetMemberExpression<TType>(Expression<Func<TEntity, TType>> anExpression)
        {
            MemberExpression expression = (MemberExpression)anExpression.Body;
            Type paramType = anExpression.Parameters[0].Type;
            String _stringExp = GetMemberExpressionASString(anExpression);
            MemberInfo _memberInfo = null;
            foreach (String _member in _stringExp.Split('.'))
            {
                _memberInfo = paramType.GetMember(_member)[0];

                if (typeof(PropertyInfo).IsAssignableFrom(_memberInfo.GetType()))
                    paramType = ((PropertyInfo)_memberInfo).PropertyType;
                else if (typeof(FieldInfo).IsAssignableFrom(_memberInfo.GetType()))
                    paramType = ((FieldInfo)_memberInfo).FieldType;
            
            }
            return _memberInfo;
        }
    }
}
