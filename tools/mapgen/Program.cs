using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace mapgen
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Invalid amount of arguments");
                return;
            }

            var sqlPath = args[0];
            var xmlPath = args[1];

            if (!Directory.Exists(sqlPath))
            {
                Console.WriteLine("Invalid sql scripts path.");
                return;
            }

            if (!Directory.Exists(xmlPath))
            {
                Console.WriteLine("Invalid xml path.");
                return;
            }

            var paths = Directory.GetFiles(sqlPath, "*.sql", SearchOption.TopDirectoryOnly);
            var xmlBuilder = new StringBuilder();
            var parameters = new List<string>();
            var spName = string.Empty;
            var opName = string.Empty;
            var script = string.Empty;
            var typeStart = 0;
            var typeEnd = 0;
            var pStart = 0;
            var typeText = string.Empty;
            var type = string.Empty;

            xmlBuilder.Append("<?xml version=\"1.0\" encoding=\"utf - 8\" ?>\r\n");
            xmlBuilder.Append("<operations\r\n");
            xmlBuilder.Append("\t\t\txmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"\r\n");
            xmlBuilder.Append("\t\t\txsi:noNamespaceSchemaLocation=\"scheme.xsd\"\r\n");

            foreach (var path in paths)
            {
                script = File.ReadAllText(path);

                spName = Path.GetFileNameWithoutExtension(path);
                opName = spName.Remove(0, 3);

                typeStart = script.IndexOf("/***") + 4;
                typeEnd = script.IndexOf("***/") - 4;
                typeText = script.Substring(typeStart, typeEnd - typeStart);
                type = script.Split(new char[] { ':' }, StringSplitOptions.None)[1];
                
                // TODO
            }
        }
    }
}