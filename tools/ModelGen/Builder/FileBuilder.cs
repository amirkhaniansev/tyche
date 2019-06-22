using System.Linq;
using System.Text;
using ModelGen.Constants;

namespace ModelGen.Builder
{
    internal class FileBuilder
    {
        private StringBuilder builder;

        public FileBuilder()
        {
            this.builder = new StringBuilder();
        }

        public FileBuilder AddNamespaces(params string[] namespaces)
        {
            var orderedNamespaces = namespaces
                .OrderBy(n => n)
                .ThenBy(n => n.Length)
                .ToList();

            foreach(var nameSpace in orderedNamespaces)
            {
                this.builder = this.builder
                    .Append(Keywords.Using)
                    .Append(Symbols.Space)
                    .Append(nameSpace)
                    .Append(Symbols.SemiColon)
                    .Append(Symbols.NewLine);
            }

            return this;
        }

        public FileBuilder StartNamespace(string nameSpace)
        {
            this.builder = this.builder
                .Append(Symbols.NewLine)
                .Append(Keywords.NameSpace)
                .Append(Symbols.Space)
                .Append(nameSpace)
                .Append(Symbols.NewLine)
                .Append(Symbols.OpeningBracket);

            return this;
        }
        
        public FileBuilder StartClass(string className, string baseName)
        {
            this.builder = this.builder
                .Append(Symbols.NewLineTab)
                .Append(Symbols.SummaryOpen)
                .Append(Symbols.NewLineTab)
                .Append($"/// Class for modelling {className} entity")
                .Append(Symbols.NewLineTab)
                .Append(Symbols.SummaryClose)
                .Append(Symbols.NewLineTab)
                .Append(Keywords.Public)
                .Append(Symbols.Space)
                .Append(Keywords.Partial)
                .Append(Symbols.Space)
                .Append(Keywords.Class)
                .Append(Symbols.Space)
                .Append(className)
                .Append(Symbols.Space)
                .Append(Symbols.Colon)
                .Append(Symbols.Space)
                .Append(baseName)
                .Append(Symbols.NewLineTab)
                .Append(Symbols.OpeningBracket);

            return this;
        }        

        public FileBuilder AddProperty(string name, string type)
        {
            this.builder = this.builder
                .Append(Symbols.NewLineDoubleTab)
                .Append(Symbols.SummaryOpen)
                .Append(Symbols.NewLineDoubleTab)
                .Append($"/// Gets or sets {name}.")
                .Append(Symbols.NewLineDoubleTab)
                .Append(Symbols.SummaryClose)
                .Append(Symbols.NewLineDoubleTab)
                .Append(Keywords.Public)
                .Append(Symbols.Space)
                .Append(type)
                .Append(Symbols.Space)
                .Append(name)
                .Append(" { get; set; }");

            return this;
        }

        public string Build()
        {
            this.builder = this.builder
                .Append(Symbols.NewLineTab)
                .Append(Symbols.ClosingBracket)
                .Append(Symbols.NewLine)
                .Append(Symbols.ClosingBracket);

            return this.builder.ToString();
        }

        public FileBuilder Clear()
        {
            this.builder = this.builder.Clear();

            return this;
        }
    }
}
