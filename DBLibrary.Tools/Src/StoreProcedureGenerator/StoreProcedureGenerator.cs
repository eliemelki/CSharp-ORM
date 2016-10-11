using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLibrary.Repository;
using System.IO;

namespace DBLibrary.Tools.Src.StoreProcedureGenerator
{
    public interface StoreProcedureGenerator
    {
         bool GenerateClassMap(String aNameSpace, String aOutPutPath);
    }

    public class StoreProcedureGeneratorImpl : StoreProcedureGenerator
    {
        private StoreProcedureTemplateGenerator TemplateGenerator { set; get; }
        private StoredProcedureRepository StoredProcedureRepository { set; get; }

        public StoreProcedureGeneratorImpl (
            StoreProcedureTemplateGenerator aTemplateGenerator,
            StoredProcedureRepository aStoreProcedureGenerator
            )
        {
            TemplateGenerator = aTemplateGenerator;
            StoredProcedureRepository = aStoreProcedureGenerator;
        }

        public bool GenerateClassMap(string aNameSpace, String aOutPutPath)
        {
            IEnumerable<StoredProcedure> _ts = StoredProcedureRepository.GetStoredProcedures();
            foreach (StoredProcedure _t in _ts)
            {
                String _value = TemplateGenerator.Generate(aNameSpace, _t);
                File.WriteAllText(Path.Combine(aOutPutPath, _t.Name + ".cs"), _value);
            }

            return true;
        }
    }
}
