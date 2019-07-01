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

namespace NamespaceGen
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "NamespaceGen";

            const string invaliAmountOfArguments = "Invalid amount of arguments.";
            const string directoryNotExist = "Directory does not exist.";
            
            if (args.Length < 2)
            {
                Console.WriteLine(invaliAmountOfArguments);
                return;
            }

            var directory = args[0];
            var topLevelNamespace = args[1];
            var reserved = args.Skip(2);

            if (!Directory.Exists(directory))
            {
                Console.WriteLine(directoryNotExist);
                return;
            }

            const string extension = "*.cs";
            const string usingText = "using";
            const string namespaceText = "namespace";

            var allPaths = Directory.GetFiles(directory, extension, SearchOption.AllDirectories);
           
            var lines = default(string[]);
            var parts = default(string[]);
            var actualNamespace = string.Empty;
            var constructedLine = string.Empty;
            var fileText = string.Empty;
            var sb = new StringBuilder();

            foreach (var path in allPaths)
            {
                lines = File.ReadAllLines(path);

                foreach(var line in lines)
                {
                    constructedLine = line;

                    if (line.StartsWith(usingText) || line.StartsWith(namespaceText))
                    {
                        parts = line.Split(" ");
                        actualNamespace = parts[1];

                        if (!reserved.Any(n => actualNamespace.StartsWith(n)))
                        {
                            actualNamespace = topLevelNamespace + "." + actualNamespace;
                            constructedLine = parts[0] + " " + actualNamespace;
                        }
                    }

                    sb.Append(constructedLine);
                    sb.Append(Environment.NewLine);
                }

                fileText = sb.ToString();

                File.WriteAllText(path, fileText);

                sb.Clear();
            }
        }
    }
}