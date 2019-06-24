using System;
using System.Linq;
using ModelGen.Constants;
using ModelGen.Database;
using ModelGen.Models;

namespace ModelGen.Builder
{
    internal class ModelBuilder
    {
        private ClassBuilder builder;

        private string nameSpace;

        private string baseName;

        public ModelBuilder(string nameSpace, string baseName)
        {
            this.nameSpace = nameSpace;
            this.baseName = baseName;

            this.builder = new ClassBuilder();
        }

        public string Build(string name, IColumnObject model, bool clear = true)
        {
            var namespaces = model.Columns
                .Select(column => Configuration.Default.Types[column.Type].Namespace)
                .Distinct()
                .ToArray();

            this.builder = this.builder.AddNamespaces(namespaces)
                .StartNamespace(nameSpace)
                .StartClass(name, baseName);

            var type = default(Type);
            var typeName = default(string);
            foreach(var column in model.Columns)
            {
                type = Configuration.Default.Types[column.Type];
                typeName = 
                    Configuration.Default.UseFriendlyTypeNames &&
                    Configuration.Default.FriendlyTypeNames.TryGetValue(type, out var tName)
                    ? tName
                    : type.Name;

                if (column.IsNullable && type.IsValueType)
                    typeName += Symbols.QuestionMark;

                this.builder = this.builder.AddProperty(column.Name, typeName);
            }

            var result =  this.builder.Build();
            if (clear)
                this.builder = this.builder.Clear();

            return result;
        }
    }
}