using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModelGen.Models;

namespace ModelGen.Builder
{
    internal static class FileBuilder
    {
        public static async Task CreateModels(
            string nameSpace,
            string baseModel,
            string path, 
            Func<IColumnObject, string> nameSelector,
            IEnumerable<IColumnObject> models)
        {
            var modelsBuilder = new ModelBuilder(nameSpace, baseModel);
            
            var fileContent = string.Empty;
            var modelName = string.Empty;

            foreach(var model in models)
            {
                modelName = nameSelector.Invoke(model);
                fileContent = modelsBuilder.Build(modelName, model);

                await File.WriteAllTextAsync($"{path}/{modelName}.cs", fileContent);
            }
        }
    }
}