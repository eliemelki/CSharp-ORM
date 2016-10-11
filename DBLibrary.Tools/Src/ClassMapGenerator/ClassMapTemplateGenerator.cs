using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using DBLibrary.Repository;

namespace DBLibrary.Tools.Src.ClassMapGenerator
{
    public interface ClassMapTemplateGenerator
    {
        String GenerateClass(String aNameSpace, TableStructure aTableStructure);
        String GenerateClass(String aTemplatePath, String aNameSpace, TableStructure aTableStructure);
    }

    public class ClassMapTemplateGeneratorImpl : ClassMapTemplateGenerator
    {
        private const String CONST_TYPE = "[TYPE]";
        private const String CONST_FIELD = "[FIELD]";
        private const String CONST_NAMESPACE = "[NAMESPACE]";
        private const String CONST_CLASS = "[CLASS]";
        private const String CONST_CLASS_BODY = "[CLASS_BODY]";
        private const String CONST_CLASSMAP_BODY = "[CLASSMAP_BODY]";
        // 
        //

        private const String CLASS_FIELDS_TEMPLATE =
            "\n\t\tpublic [TYPE] [FIELD] { set; get; }";
        private const String CLASS_MAP_FIELD_TEMPLATE =
            "\n\t\t\taClassMap.MapField(m => m.[FIELD]).SetColumn(\"[FIELD]\");"; 
        private const String CLASS_MAP_ID_FIELD_TEMPLATE =
            "\n\t\t\taClassMap.MapIdentity(m => m.[FIELD]).SetColumn(\"[FIELD]\");";
        private const String CLASS_MAP_TABLE_TEMPLATE =
            "\n\t\t\taClassMap.SetTableName(\"[CLASS]\");";


        public String GenerateClass(String aTemplatePath, String aNameSpace, TableStructure aTableStructure)
        {
            String _template = GetTemplate(aTemplatePath);

            _template = _template.Replace(CONST_NAMESPACE, aNameSpace);
            _template = _template.Replace(CONST_CLASS, aTableStructure.TableName);
            _template = _template.Replace(CONST_CLASS_BODY, GenerateClassBody(aTableStructure));
            return _template.Replace(CONST_CLASSMAP_BODY, GenerateClassMapBody(aTableStructure));
        }

        public string GenerateClass(String aNameSpace, TableStructure aTableStructure)
        {
            var path = this.GetType().Namespace + ".Template.txt";
            return GenerateClass(path, aNameSpace, aTableStructure);           

        }

        public String GenerateClassBody(TableStructure aTableStructure)
        {
            StringBuilder _r = new StringBuilder();
            foreach (Column _c in aTableStructure.Columns)
                _r.Append(GenerateField(aTableStructure, _c));
            return _r.ToString();
        }

        public String GenerateClassMapBody(TableStructure aTableStructure)
        {
            StringBuilder _r = new StringBuilder();
            foreach (Column _c in aTableStructure.Columns)
                _r.Append(GenerateMapField(aTableStructure,_c));
            
            _r.Append(CLASS_MAP_TABLE_TEMPLATE.Replace(CONST_CLASS, aTableStructure.TableName));
            return _r.ToString();
        }

        public String GenerateField(TableStructure aTableStructure, Column aColumn)
        {   
            String _field = CLASS_FIELDS_TEMPLATE.Replace(CONST_TYPE, aColumn.Type);
            return _field.Replace(CONST_FIELD, aColumn.GetCorrectedColumnName(aTableStructure));
        }

        public String GenerateMapField(TableStructure aTableStructure, Column aColumn)
        {
            String _colName = aColumn.GetCorrectedColumnName(aTableStructure);
            if (aColumn.IsPrimaryKey)
                  return CLASS_MAP_ID_FIELD_TEMPLATE.Replace(
                      CONST_FIELD,
                     _colName);
            else
                return CLASS_MAP_FIELD_TEMPLATE.Replace(
                    CONST_FIELD,
                    _colName);
        }

        public String GetTemplate(String aTemplatePath)
        {
            
            using (Stream stream = Assembly.GetExecutingAssembly()
                                .GetManifestResourceStream(aTemplatePath))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }

    public static class ColumnHelper
    {
        public static String GetCorrectedColumnName(this Column aColumn, TableStructure aTableStructure)
        {
            string _colName = aColumn.Name;
            if (_colName == aTableStructure.TableName)
                return _colName += "_F";
            else
                return _colName;
        }
    }
}
