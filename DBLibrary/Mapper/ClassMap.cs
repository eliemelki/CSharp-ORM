using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using DBLibrary.Configuration;
using DBLibrary.QueryEngine.Expressions;
using Loader;
namespace DBLibrary.Mapper
{
    public abstract class ClassMap
    {
        public abstract String GetTableName();
    }

    public class ClassMap<TEntity> : ClassMap where TEntity : class, new()
    {
        private Dictionary<MemberInfo, PropertyMap> map;
        public List<PropertyMap> Properties { get; private set; }
        public IdentityMap Identity { get; private set; }
        public List<ComponentMap> Components { get; private set; }
        public ExpressionHelper<TEntity> ExpHelper { get; private set; }
        private String table;

        public ClassMap()
        {
            map = new Dictionary<MemberInfo, PropertyMap>();
            Properties = new List<PropertyMap>();
            Components = new List<ComponentMap>();
            table = typeof(TEntity).Name;
            ExpHelper = BaseFactory.Instance.GetInstance<DBExpression>().GetHelper<TEntity>();
        }

        public override String GetTableName()
        {
            return table;
        }

        public ClassMap<TEntity> SetTableName(String aTable)
        {
            table = aTable;
            return this;
        }
        
        private const String ERROR = "in Class={0}, Key = {1} already added to the classmap list";
        private void AddToMap(MemberInfo aMember, PropertyMap aMapper)
        {
            PropertyMap value;
            if (map.TryGetValue(aMember,out value) == true)
                throw new Exception(String.Format(ERROR, typeof(TEntity).Name, aMember.Name));
            map.Add(aMember, aMapper);
        }

        public FieldMap MapField<TType>(Expression<Func<TEntity, TType>> anExpression)
        {
            MemberExpression expression = (MemberExpression)anExpression.Body;
            MemberInfo _member = ExpHelper.GetMemberExpression(anExpression);
            FieldMap mapper = new FieldMap(_member, expression.Type);
            Properties.Add(mapper);
            AddToMap(_member, mapper);
            return mapper;
        }

        public IdentityMap MapIdentity<TType>(Expression<Func<TEntity, TType>> anExpression)
        {
            MemberExpression expression = (MemberExpression)anExpression.Body;
            MemberInfo _member = ExpHelper.GetMemberExpression(anExpression);
            IdentityMap mapper = new IdentityMap(_member, expression.Type);
            AddToMap(_member, mapper);
            Identity = mapper;
            return mapper;
        }

        public ComponentMap<E> MapComponent<E>(Expression<Func<TEntity, E>> anExpression, Action<ComponentMap<E>> anAction)
        where E : class, new()
        {
            MemberExpression expression = (MemberExpression)anExpression.Body;
            MemberInfo _member = ExpHelper.GetMemberExpression(anExpression);
            ComponentMap<E> mapper = new ComponentMap<E>(_member, expression.Type);
            anAction(mapper);
            foreach (PropertyMap comMap in mapper.map.Values)
            {
                AddToMap(comMap.Member, comMap);
            }
            Components.Add(mapper);
            return mapper;
        }

        public PropertyMap GetProperty(MemberInfo aMember)
        {
            PropertyMap property = null;
            map.TryGetValue(aMember, out property);
            return property;
        }
    }
}
