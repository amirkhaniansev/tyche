using System;
using System.IO;
using System.Linq;
using System.Text;

namespace namespacegen
{
    class Program
    {
        static void Main(string[] args)
        {
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