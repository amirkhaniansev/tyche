using System.Collections.Generic;

namespace modelgen
{
    internal class Procedure : BaseModel
    {
        public IEnumerable<ProcedureParameter> Parameters { get; set; }
    }
}