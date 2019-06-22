using ModelGen.Database;

namespace ModelGen.Models
{
    internal class Column : BaseModel
    {
        public SqlType Type { get; set; }

        public bool IsNullable { get; set; }
    }
}