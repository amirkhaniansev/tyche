using System.Collections.Generic;

namespace modelgen
{
    internal class Table : BaseModel
    {
        public IEnumerable<TableColumn> Columns { get; set; }
    }
}