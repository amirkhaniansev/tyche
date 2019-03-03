using Newtonsoft.Json;
using System;
using TycheBL;

namespace BusinessLogicTest
{
    static class Print
    {
        internal static void PrintDbResponse(string name, DbResponse dbResponse)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(name);
            Console.ResetColor();
            Console.WriteLine(JsonConvert.SerializeObject(dbResponse, Formatting.Indented));
            Console.ReadLine();
        }
    }
}