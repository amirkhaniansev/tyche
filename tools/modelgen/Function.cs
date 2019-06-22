using System.Collections.Generic;

namespace modelgen
{
    internal class Function : BaseModel
    {
        public IEnumerable<FunctionParameter> Parameters { get; set; }

        public IEnumerable<FunctionColumn> Columns { get; set; }
    }
}