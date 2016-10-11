using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using DBLibrary.Repository;
using DBLibrary.Tools.Src.ClassMapGenerator;
using DBLibrary.Categories;
using System.Data;

namespace DBLibrary.Tools.Src.StoreProcedureGenerator
{
    public interface StoreProcedureTemplateGenerator
    {
        String Generate(String aNameSpace, StoredProcedure aStoreProcedure);
    }

    public class StoreProcedureTemplateGeneratorImpl : StoreProcedureTemplateGenerator
    {
        private const String CONST_NAMESPACE = "[NAMESPACE]";
        private const String STORED_PROCEDUDRE = "[STORED_PROCEDUDRE]";
        private const String STORED_PROCEDUDRE_PARAMATERS_ARGUMENTS = "[STORED_PROCEDUDRE_PARAMATERS_ARGUMENTS]";
        private const String STORED_PROCEDUDRE_PARAMATERS = "[STORED_PROCEDUDRE_PARAMATERS]";

        private ClassMapTemplateGenerator Generator;

        public StoreProcedureTemplateGeneratorImpl(ClassMapTemplateGenerator aGenerator)
        {
            Generator = aGenerator;
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

        public string Generate(string aNameSpace, StoredProcedure aStoredProcedure)
        {
            String _template = "";
            if (aStoredProcedure.Result == null)
            {
                var _path = this.GetType().Namespace + ".NoResultTemplate.txt";
                _template = GetTemplate(_path);
                _template = _template.Replace(CONST_NAMESPACE, aNameSpace);
            }
            else if (aStoredProcedure.Result.Columns.Count() == 1 &&
                aStoredProcedure.Result.Columns[0].Type == typeof(DataTable).ToString())
            {
                var _path = this.GetType().Namespace + ".UnknownResultTemplate.txt";
                _template = Generator.GenerateClass(_path, aNameSpace, aStoredProcedure.Result);
            }
            else
            {
                var _path = this.GetType().Namespace + ".Template.txt";
                _template = Generator.GenerateClass(_path, aNameSpace, aStoredProcedure.Result);
            }
             
            _template = _template.Replace(STORED_PROCEDUDRE, aStoredProcedure.Name);
            _template = _template.Replace(STORED_PROCEDUDRE_PARAMATERS_ARGUMENTS, GetArguments(aStoredProcedure));
            _template = _template.Replace(STORED_PROCEDUDRE_PARAMATERS, GetParamaters(aStoredProcedure));

            return _template;
        }

        public String GetArguments(StoredProcedure aStoredProcedure)
        {
            String t = "{0} {1} ,";
            StringBuilder _R = new StringBuilder();
            foreach (Column aParamater in aStoredProcedure.Paramaters)
            {
                var _f  = String.Format(t, aParamater.Type, aParamater.Name);
                _R.Append(_f);
            }

            return _R.ToString().RemoveLastCharacter(",");
        }

        public String GetParamaters(StoredProcedure aStoredProcedure)
        {
            String t = "\t\t\tdic.Add(\"{0}\",{0});\n";
            StringBuilder _R = new StringBuilder();
            foreach (Column aParamater in aStoredProcedure.Paramaters)
            {
                var _f = String.Format(t, aParamater.Name);
                _R.Append(_f);
            }
            return _R.ToString();
        }
    }
}
