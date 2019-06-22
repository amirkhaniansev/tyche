using System.Collections.Generic;

namespace ModelGen.Models
{
    internal class Table : BaseModel
    {
        public IEnumerable<TableColumn> Columns { get; set; }
    }
}