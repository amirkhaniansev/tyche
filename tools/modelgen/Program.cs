﻿using System.Threading.Tasks;
using ModelGen.Database;

namespace modelgen
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var scheme = new Scheme();

            await scheme.InitializeQueries();
            await scheme.InitializeTables();
            await scheme.InitializeFunctions();
            await scheme.InitializeProcedures();
            

        }
    }
}