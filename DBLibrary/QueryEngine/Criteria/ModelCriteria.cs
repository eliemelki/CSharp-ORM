using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.Mapper;
using DBLibrary.Configuration;
using DBLibrary.QueryEngine.Expressions;
using System.Reflection;

namespace DBLibrary.QueryEngine.Criteria
{
    public class Property
    {
        public String Member { set; get; }
        public Object Value { set; get; }
        public Property(String aMember, Object aValue)
        {
            Member = aMember;
            Value = aValue;
        }
    
    }
    public delegate void AppendCriteria(SqlCriteria aCriteria, PropertyMap aMap, Object aValue);

    public interface ModelCriteria
    {
        List<Property> GetProperties<T>(T aData, bool isBindNull) where T : class, new();
        SqlCriteria GetCriteria<T>(T aData, AppendCriteria aCriteriaAppender, bool isBindNull,bool isIDIncludedIfExist) where T : class ,new();
  
    }

    class ModelCriteriaImpl : ModelCriteria
    {
        private PropertyBinder propertyBinder;
        private ValueExtractor extractor;
        private SqlFactory factory;

        public ModelCriteriaImpl(SqlFactory aFactory, PropertyBinder aBinder, ValueExtractor anExtractor)
        {
            propertyBinder = aBinder;
            extractor = anExtractor;
            factory = aFactory;
        }

        private void AppendMember<T>(List<Property> aMembers, PropertyMap aMap, T aData, bool isBindNull) where T : class, new()
        {
            object value = extractor.GetValue(aMap.Member.Name, aData);
            if (!isBindNull && value == null)
                return;
            aMembers.Add(new Property(aMap.GetColumn(), value));
        }

        private void AppendCriteria<T>(SqlCriteria aCriteria, AppendCriteria aCriteriaAppender, PropertyMap aMap, T aData, bool isBindNull) where T : class, new()
        {
            object value = extractor.GetValue(aMap.Member.Name, aData);
            if (!isBindNull && value == null)
                return;
            aCriteriaAppender(aCriteria, aMap, value);
        }


        public List<Property> GetProperties<T>(T aData, bool isBindNull) where T : class, new()
        {
            var memberList = new List<Property>();
            propertyBinder.BindProperty<T>(

               delegate(PropertyMap aMap)
               {
                   AppendMember(memberList, aMap, aData, isBindNull);

               },
               delegate(IdentityMap aMap)
               {
                   AppendMember(memberList, aMap, aData, isBindNull);
               },
                delegate(PropertyMap aMap, MemberInfo[] aWrapperParents)
                 {
                     var field = extractor.GetValue(aData, aWrapperParents);
                   AppendMember(memberList, aMap, field, isBindNull);
               });

            return memberList;
        }

        public SqlCriteria GetCriteria<T>(T aData, AppendCriteria aCriteriaAppender, bool isBindNull, bool isIdIncludedIfExists) where T : class ,new()
        {
            var criteria = factory.CreateCriteria();
             propertyBinder.BindProperty<T>(

                delegate(PropertyMap aMap)
                {
                    AppendCriteria(criteria, aCriteriaAppender, aMap, aData, isBindNull); 
                },
                delegate(IdentityMap aMap)
                {
                    if (isIdIncludedIfExists)
                    {
                        AppendCriteria(criteria, aCriteriaAppender, aMap, aData, isBindNull);
                    }
                },
                delegate(PropertyMap aMap, MemberInfo[] aParentsMembers)
                {
                        var field = extractor.GetValue(aData, aParentsMembers);
                    AppendCriteria(criteria, aCriteriaAppender, aMap, field, isBindNull); 
                });
            return criteria;
        }
    }
}

