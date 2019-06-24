using System.Collections.Generic;

namespace ModelGen.Models
{
    internal interface IColumnObject
    {
        string Name { get; set; }

        IEnumerable<IColumn> Columns { get; set; }
    }
}