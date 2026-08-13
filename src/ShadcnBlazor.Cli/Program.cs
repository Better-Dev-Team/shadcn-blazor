using System;
using System.Linq;
using System.Threading.Tasks;
using ShadcnBlazor.Cli.Commands;

namespace ShadcnBlazor.Cli;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        if (args.Length == 0 || args[0] is "--help" or "-h" or "help")
        {
            PrintUsage();
            return 0;
        }

        var command = args[0].ToLowerInvariant();
        var cmdArgs = args.Skip(1).ToArray();

        switch (command)
        {
            case "init":
                return await InitCommand.ExecuteAsync(cmdArgs);

            case "add":
                return await AddCommand.ExecuteAsync(cmdArgs);

            case "list" or "ls":
                return ListCommand.Execute();

            case "--version" or "-v" or "version":
                Console.WriteLine("ShadcnBlazor CLI v1.0.0");
                return 0;

            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Unknown command '{command}'.");
                Console.ResetColor();
                PrintUsage();
                return 1;
        }
    }

    private static void PrintUsage()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
   _____ __               __            ____  __                      
  / ___// /_  ____ _____/ /________  / __ )/ /___ _____  ____  _____
  \__ \/ __ \/ __ `/ __  / ___/ __ \/ __  / / __ `/_  / / __ \/ ___/
 ___/ / / / / /_/ / /_/ / /__/ / / / /_/ / / /_/ / / /_/ /_/ / /    
/____/_/ /_/\__,_/\__,_/\___/_/ /_/_____/_/\__,_/ /___/\____/_/     
");
        Console.ResetColor();
        Console.WriteLine("Modern, copy-paste and package-ready UI components for Blazor.\n");
        Console.WriteLine("Usage:");
        Console.WriteLine("  shadcn-blazor [command] [options]\n");
        Console.WriteLine("Commands:");
        Console.WriteLine("  init              Initialize ShadcnBlazor in your current project");
        Console.WriteLine("  add <components>  Add components to your project (e.g. button dialog tabs)");
        Console.WriteLine("  list              List all available components");
        Console.WriteLine("  version           Display CLI tool version\n");
    }
}
