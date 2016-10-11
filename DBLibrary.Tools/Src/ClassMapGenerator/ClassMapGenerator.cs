using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.Repository;
using System.IO;

namespace DBLibrary.Tools.Src.ClassMapGenerator
{
    public interface ClassMapGenerator
    {
        bool GenerateClassMap(String aNameSpace, String aOutPutPath); 
    }

    public class ClassMapGeneratorImpl : ClassMapGenerator
    {
        private ClassMapTemplateGenerator TemplateGenerator { set; get; }
        private SchemaRepository SchemaRepository { set; get; }

        public ClassMapGeneratorImpl(
            SchemaRepository aSchemaRepository,
            ClassMapTemplateGenerator aTemplateGenerator
            )
        {
            SchemaRepository = aSchemaRepository;
            TemplateGenerator = aTemplateGenerator;
        }

        public bool GenerateClassMap(string aNameSpace, String aOutPutPath)
        {
            IEnumerable<TableStructure> _ts = SchemaRepository.GetSchemaTables();
            foreach (TableStructure _t in _ts)
            {
                String _value = TemplateGenerator.GenerateClass(aNameSpace, _t);
                File.WriteAllText(Path.Combine(aOutPutPath, _t.TableName + ".cs"), _value);
            }

            return true;
        }
    }
}
