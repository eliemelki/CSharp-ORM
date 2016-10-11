using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using DBLibrary.QueryEngine.Expressions;
using Loader;


namespace DBLibrary.Mapper
{
    public interface PropertyMap
    {
        MemberInfo Member { get; }
        Type Type { get; }
        String GetColumn();
        PropertyMap SetColumn(String aColumn);
    }

    public class AbstractMap : PropertyMap
    {
        private String column;


        public AbstractMap(MemberInfo aMember, Type aType)
        {
            Member = aMember;
            column = aMember.Name;
            Type = aType;
        }

        public MemberInfo Member { get; protected set; }

        public String GetColumn() { return column; }

        public PropertyMap SetColumn(string aColumn)
        {
            column = aColumn;
            return this;
        }


        public Type Type
        {
            get;
            private set;
        }

        private const String DISPLAY = "[{0}: Member:{1}-Column:{2}-Type:{3}]";
        public override string ToString()
        {
            return String.Format(DISPLAY, base.ToString(), Member, column, Type);
        }
    }

    public class FieldMap : AbstractMap
    {
        public FieldMap(MemberInfo aMember, Type aType)
            : base(aMember, aType)
        {

        }
    }

    public class IdentityMap : AbstractMap
    {
        public IdentityMap(MemberInfo aMember, Type aType)
            : base(aMember, aType)
        {

        }
    }

    public interface ComponentMap
    {
        List<PropertyMap> Properties { get; }
        MemberInfo Member { get; }
        Type Type { get; }
        List<ComponentMap> Components { get; }
    }

    public class ComponentMap<T> : ComponentMap
        where T : class, new()
    {
        public ExpressionHelper<T> ExpHelper;

        public ComponentMap(MemberInfo aMember, Type aType)
        {
            Member = aMember;
            Type = aType;
            Properties = new List<PropertyMap>();
            Components = new List<ComponentMap>();
            map = new Dictionary<MemberInfo, PropertyMap>();
            ExpHelper = BaseFactory.Instance.GetInstance<DBExpression>().GetHelper<T>();
        }
        public MemberInfo Member { get; private set; }
        public Type Type { get; private set; }
        public List<PropertyMap> Properties { get; private set; }
        public List<ComponentMap> Components { get; private set; }
        public Dictionary<MemberInfo, PropertyMap> map;

        private const String ERROR = "in Class={0}, Key = {1} already added to the classmap list";
        private void AddToMap(MemberInfo aMember, PropertyMap aMapper)
        {
            PropertyMap value;
            if (map.TryGetValue(aMember, out value) == true)
                throw new Exception(String.Format(ERROR, typeof(T).Name, aMember.Name));
            map.Add(aMember, aMapper);
        }

        public FieldMap MapField<TType>(Expression<Func<T, TType>> anExpression)
        {
            MemberExpression expression = (MemberExpression)anExpression.Body;
            MemberInfo _member = ExpHelper.GetMemberExpression(anExpression);
            FieldMap mapper = new FieldMap(_member, expression.Type);
            Properties.Add(mapper);
            AddToMap(_member, mapper);
            return mapper;
        }

        private const String DISPLAY = "[{0}: Member:{1}-Type:{2}-Properties:[{3}]]";
        public override string ToString()
        {
            StringBuilder value = new StringBuilder();
            foreach (PropertyMap map in this.Properties)
            {
                value.Append("\r" + map);
            }
            return String.Format(DISPLAY, base.ToString(), Member, value.ToString(), Type);
        }

        public ComponentMap<E> MapComponent<E>(Expression<Func<T, E>> anExpression, Action<ComponentMap<E>> anAction)
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
    }
}
