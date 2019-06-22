using System.Collections.Generic;

namespace ModelGen.Models
{
    internal class Procedure : BaseModel
    {
        public IEnumerable<ProcedureParameter> Parameters { get; set; }
    }
}