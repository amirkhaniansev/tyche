/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Program
 * Copyright (C) <2019>
 *      Authors: <amirkhaniansev>  <amirkhanyan.sevak@gmail.com>
 *               <DavidPetr>       <david.petrosyan11100@gmail.com>
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
using System.Collections.Generic;

namespace licensegen
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Invalid amount of arguments.");
                return;
            }

            if (!Directory.Exists(args[0]))
            {
                Console.WriteLine("Repository path does not exist.");
                return;
            }

            if (!File.Exists(args[1]))
            {
                Console.WriteLine("License file does not exist.");
                return;
            }

            var i = 0;
            var j = 0;
            var repoPath = args[0];
            var licensePath = args[1];
            var filePaths = new List<string>();
            var extensionFiles = default(string[]);
            var text = string.Empty;
            var fileName = string.Empty;
            var licenseText = string.Empty;
            var license = File.ReadAllText(licensePath);
            var extensions = args
                .Skip(2)
                .Select(e => e.Contains("*") ? e : "*" + e);

            foreach (var extension in extensions)
            {
                extensionFiles = Directory.GetFiles(repoPath, extension, SearchOption.AllDirectories);
                filePaths.AddRange(extensionFiles);
            }

            foreach (var path in filePaths)
            {
                text = File.ReadAllText(path);

                if (text.StartsWith("/*"))
                {
                    for (i = 2, j = 0; i < text.Length - 2; i++)
                    {
                        if (text[i] == '*' && text[i + 1] == '/')
                            break;
                        j++;
                    }

                    if (text.Length > j + 6)
                    {
                        text = text.Remove(0, j + 6);
                    }
                }
                else
                {
                    license += "\r\n";
                }

                fileName = Path.GetFileNameWithoutExtension(path);
                licenseText = string.Format(license, fileName);
                text = licenseText + "\r\n" + text;
                File.WriteAllText(path, text);
                Console.WriteLine("License is appended to {0}", path);
            }

            Console.WriteLine("License appending is ended in repo : {0}", repoPath);
            Console.WriteLine("Press <Enter> to close");
            Console.ReadLine();
        }
    }
}