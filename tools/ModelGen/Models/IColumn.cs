using ModelGen.Database;

namespace ModelGen.Models
{
    internal interface IColumn
    {
        string Name { get; set; }

        bool IsNullable { get; set; }

        SqlType Type { get; set; }
    }
}