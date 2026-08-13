using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ShadcnBlazor.Cli.Registry;

namespace ShadcnBlazor.Cli.Commands;

public static class AddCommand
{
    public static async Task ExecuteAsync(string[] args)
    {
        if (args.Length == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: Please specify one or more component names to add.");
            Console.ResetColor();
            Console.WriteLine("Example: shadcn-blazor add button card dialog");
            Console.WriteLine("Use 'shadcn-blazor list' to view all available components.");
            return;
        }

        var currentDir = Directory.GetCurrentDirectory();
        var targetUiDir = Path.Combine(currentDir, "Components", "UI");
        Directory.CreateDirectory(targetUiDir);

        var requested = args;
        if (args.Contains("--all", StringComparer.OrdinalIgnoreCase) || args.Contains("-a", StringComparer.OrdinalIgnoreCase))
        {
            requested = ComponentRegistry.Components.Keys.ToArray();
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n✦ Adding {requested.Length} component(s) to {Path.GetRelativePath(currentDir, targetUiDir)}...\n");
        Console.ResetColor();

        foreach (var name in requested)
        {
            if (name.StartsWith("-")) continue;

            if (ComponentRegistry.Components.TryGetValue(name, out var comp))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✓ Installed {comp.Name}: {comp.Description}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"⚠ Unknown component '{name}'. Skipping.");
                Console.ResetColor();
            }
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n✦ Ready! Import and use your components in Razor files.\n");
        Console.ResetColor();
    }
}
