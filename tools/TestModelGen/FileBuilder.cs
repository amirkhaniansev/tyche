using System.Linq;
using System.Text;

namespace TestModelGen
{
    class FileBuilder
    {
        private StringBuilder builder;

        private StringBuilder namespacesBuilder;

        public FileBuilder()
        {
            this.builder = new StringBuilder();
            this.namespacesBuilder = new StringBuilder();
        }

        public FileBuilder AddNamespaces(params string[] namespaces)
        {
            var orderedNamespaces = namespaces
                .OrderBy(n => n)
                .ThenBy(n => n.Length)
                .ToArray();

            for(var i = 0; i < orderedNamespaces.Length - 1; i++)
            {
                this.namespacesBuilder = this.namespacesBuilder
                    .Append(Keywords.Using)
                    .Append()
            }

        }
    }
}
