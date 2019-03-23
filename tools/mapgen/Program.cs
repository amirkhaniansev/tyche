/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Program
 * Copyright (C) <2019>
 *      Authors: <amirkhaniansev>  <amirkhanyan.sevak@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * Full notice : https://github.com/amirkhaniansev/tyche/tree/master/LICENSE
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
**/

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

/*
 * This tool is for fastening development process. 
 * It is not optimized or the best solution for tasks like this.
 */

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

            if (!File.Exists(xmlPath))
            {
                Console.WriteLine("Invalid xml path.");
                return;
            }

            var paths = Directory.GetFiles(sqlPath, "*.sql", SearchOption.AllDirectories);
            var xmlBuilder = new StringBuilder();
            var parameters = new List<string>();
            var script = new string[0];
            var spName = string.Empty;
            var opName = string.Empty;
            var typeLine = string.Empty;
            var typeText = string.Empty;
            var type = string.Empty;
            var count = 1;

            xmlBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
            xmlBuilder.Append("<operations\r\n");
            xmlBuilder.Append("\t\t\txmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"\r\n");
            xmlBuilder.Append("\t\t\txsi:noNamespaceSchemaLocation=\"scheme.xsd\">\r\n\r\n");

            foreach (var path in paths)
            {
                spName = Path.GetFileNameWithoutExtension(path);
                opName = spName.Remove(0, 3);
                script = File.ReadAllLines(path);
                typeLine = script.First(str => str.StartsWith("/***"));
                type = typeLine.Substring(4, typeLine.Length - 8)
                    .Split(":")
                    .Last()
                    .Replace(" ","");

                parameters = script.Where(str => str.StartsWith("\t@"))
                    .Select(str => str.Split("\t")[1].Remove(0,1))
                    .ToList();

                xmlBuilder.AppendFormat("\t<!-- Operation Number : {0} -->\r\n", count++);
                xmlBuilder.AppendFormat("\t<operation name=\"{0}\">\r\n", opName);
                xmlBuilder.AppendFormat("\t\t<spName>{0}</spName>\r\n", spName);
                xmlBuilder.Append("\t\t<parameters>\r\n");

                // Be careful.
                // Boilerplate is here.
                if (parameters.Count == 1)
                {
                    xmlBuilder.Append("\t\t\t<parameter>\r\n");
                    xmlBuilder.AppendFormat("\t\t\t\t<parameterName>primitive</parameterName>\r\n");
                    xmlBuilder.AppendFormat("\t\t\t\t<spParameterName>{0}</spParameterName>\r\n",
                        parameters.First());
                    xmlBuilder.Append("\t\t\t</parameter>\r\n");
                }
                foreach (var parameter in parameters)
                {
                    xmlBuilder.Append("\t\t\t<parameter>\r\n");
                    xmlBuilder.AppendFormat("\t\t\t\t<parameterName>{0}</parameterName>\r\n",
                        parameter.First().ToString().ToUpper() + parameter.Substring(1));
                    xmlBuilder.AppendFormat("\t\t\t\t<spParameterName>{0}</spParameterName>\r\n",
                        parameter);
                    xmlBuilder.Append("\t\t\t</parameter>\r\n");
                }

                xmlBuilder.Append("\t\t</parameters>\r\n");
                xmlBuilder.AppendFormat("\t\t<returnDataType>{0}</returnDataType>\r\n", type);
                xmlBuilder.Append("\t</operation>\r\n\r\n");
            }

            xmlBuilder.Append("</operations>");

            var xml = xmlBuilder.ToString();
            File.WriteAllText(xmlPath, xml);
        }
    }
}