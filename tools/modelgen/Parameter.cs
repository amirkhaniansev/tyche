﻿namespace modelgen
{
    internal class Parameter : BaseModel
    {
        public SqlType Type { get; set; }

        public bool IsNullable { get; set; }
    }
}