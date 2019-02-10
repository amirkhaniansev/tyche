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

            if (!Directory.Exists(args[1]))
            {
                Console.WriteLine("Repository path does not exist.");
                return;
            }

            if (!File.Exists(args[2]))
            {
                Console.WriteLine("License file does not exist.");
                return;
            }

            var repoPath = args[1];
            var licensePath = args[2];
            var extensions = args.Skip(3);
            var filePaths = new List<string>();
            var text = string.Empty;
            var fileName = string.Empty;
            var licenseText = string.Empty;
            var license = File.ReadAllText(licensePath);

            foreach (var extension in extensions)
            {
                filePaths.AddRange(Directory.GetFiles(repoPath, extension, SearchOption.AllDirectories));
            }

            foreach (var path in filePaths)
            {
                text = File.ReadAllText(path);
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