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

                    if (text.Length > 2)
                    {
                        text = text.Remove(0, j + 6);
                    }
                }

                fileName = Path.GetFileNameWithoutExtension(path);
                licenseText = string.Format(license, fileName);
                text = licenseText + "\n\n" + text;
                File.WriteAllText(path, text);
                Console.WriteLine("License is appended to {0}", path);
            }

            Console.WriteLine("License appending is ended in repo : {0}", repoPath);
            Console.WriteLine("Press <Enter> to close");
            Console.ReadLine();
        }
    }
}