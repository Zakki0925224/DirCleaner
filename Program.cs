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
            CLI.RegisterCommand("analyze", "Analyze the directory.", (args) =>
            {
                var cleaner = new DirectoryCleaner();
                var path = args[0].Replace("\"", "");
                cleaner.Analyze(path);
                cleaner.Clean();
                cleaner.Analyze(path);
                cleaner.Clean();
            });

            CLI.StartMainLoop();
        }
    }
}
