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

            if (!File.Exists("․/" + args[1]))
            {
                Console.WriteLine("License file does not exist.");
                return;
            }

            var repoPath = args[0];
            var licensePath = args[1];
            var extensions = args.Skip(2);
            var filePaths = new List<string>();
            var text = string.Empty;
            var fileName = string.Empty;
            var licenseText = string.Empty;
            var license = File.ReadAllText(licensePath);
            var i = 0;
            var j = 0;

            foreach (var extension in extensions)
            {
                filePaths.AddRange(Directory.GetFiles(repoPath, extension, SearchOption.AllDirectories));
            }

            foreach (var path in filePaths)
            {
                text = File.ReadAllText(path);

                for (i = 3, j = 0; i < text.Length - 3; i++)
                {
                    if (text[i] == '*' && text[i + 1] == '*' && text[i + 2] == '/')
                        break;
                    j++;
                }

                text = text.Remove(0, j + 6);

                fileName = Path.GetFileNameWithoutExtension(path);
                licenseText = string.Format(license, fileName);
                text = licenseText + text;
                File.WriteAllText(path, text);
                Console.WriteLine("License is appended to {0}", path);
            }

            Console.WriteLine(
                "License appending is ended in repo : {0}",
                repoPath);
            Console.WriteLine("Press <Enter> to close");
            Console.ReadLine();
        }
    }
}