using System;
using CLIUtils;

namespace DirCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            CLI.StartMessage = "";
            CLI.InputMarker = ">";
            CLI.RegisterCommand("analyze", "Analyze the directory.", (cmdArgs) =>
            {
                if (cmdArgs.Length != 1)
                {
                    Console.WriteLine("Invalid arguments.");
                    Console.WriteLine("Usage: analyze \"<dirPath(full/relative)>\"");
                    return;
                }

                var cleaner = new DirectoryCleaner();
                var path = cmdArgs[0].Replace("\"", "");
                cleaner.Analyze(path);
                cleaner.Clean();
            });

            CLI.StartMainLoop();
        }
    }
}
