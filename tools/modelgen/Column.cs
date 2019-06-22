namespace modelgen
{
    internal class Column : BaseModel
    {
        public SqlType Type { get; set; }

        public bool IsNullable { get; set; }
    }
}