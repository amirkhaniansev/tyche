﻿namespace ModelGen.Models
{
    internal class TableColumn : Column
    {
        public int TableId { get; set; }

        public bool IsIdentity { get; set; }
    }
}