using System;
using System.Collections.Generic;
using System.Text;

namespace modelgen
{
    internal static class Formats
    {
        public const string File = "{0}\n\n{1}";

        public const string Namespace = "using {0};";

        public const string NamespaceClause = "namespace {0}\n{{\n{1}\n}}";
        
        public const string ClassSummary = "/// <summary>\n\t/// {0}\n\t/// </summary>";

        public const string PropertySummary = "/// <summary>\n\t\t/// {0}\n\t\t/// </summary>";

        public const string PropertyWithSummary = "\t{0}\n\t{1}";

        public const string ClassWithSummary = "\t{0}\n\t{1}";

        public const string Property = "\tpublic int {0} {{ get; set; }}";

        public const string Class = "public partial class {0} : {1}\n\t{{\n\t{2}\n\t}}";
    }
}
