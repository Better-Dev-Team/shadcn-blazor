using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ShadcnBlazor.Cli.Registry;

namespace ShadcnBlazor.Cli.Commands;

public static class AddCommand
{
    public static async Task<int> ExecuteAsync(string[] args)
    {
        if (args.Length == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: Please specify one or more component names to add.");
            Console.ResetColor();
            Console.WriteLine("Example: shadcn-blazor add button card dialog");
            Console.WriteLine("Use 'shadcn-blazor list' to view all available components.");
            return 1;
        }

        var requested = args.Where(a => !a.StartsWith("-")).ToList();
        if (args.Contains("--all", StringComparer.OrdinalIgnoreCase) || args.Contains("-a", StringComparer.OrdinalIgnoreCase))
        {
            requested = ComponentRegistry.Components.Keys.ToList();
        }

        if (requested.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: No component names provided.");
            Console.ResetColor();
            return 1;
        }

        var components = ResolveComponents(requested);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n✦ Adding {components.Count} component(s) to Components/UI...\n");
        Console.ResetColor();

        var currentDir = Directory.GetCurrentDirectory();
        var targetUiDir = Path.Combine(currentDir, "Components", "UI");
        Directory.CreateDirectory(targetUiDir);

        var installed = 0;
        var missing = 0;

        foreach (var comp in components)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"✓ {comp.Name}: ");
            Console.ResetColor();
            Console.WriteLine(comp.Description);

            foreach (var file in comp.Files)
            {
                if (!AssetStore.TryRead("Components/" + file, out var content))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"    ⚠ Asset '{file}' not found in package. Skipping.");
                    Console.ResetColor();
                    missing++;
                    continue;
                }

                var targetPath = Path.Combine(targetUiDir, file.Replace('/', Path.DirectorySeparatorChar));
                var wasUpdated = File.Exists(targetPath);

                var dir = Path.GetDirectoryName(targetPath);
                if (!string.IsNullOrEmpty(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(targetPath, content);
                Console.WriteLine($"    → {file}{(wasUpdated ? " (updated)" : "")}");
                installed++;
            }
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n✦ Installed {installed} file(s) across {components.Count} component(s).");
        Console.WriteLine("  Import and use your components in Razor files.\n");
        Console.ResetColor();

        return installed == 0 ? 1 : 0;
    }

    private static List<RegistryComponent> ResolveComponents(IEnumerable<string> requested)
    {
        var result = new List<RegistryComponent>();
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var queue = new Queue<string>();

        foreach (var name in requested)
        {
            if (ComponentRegistry.Components.TryGetValue(name, out var comp))
            {
                if (visited.Add(comp.Name))
                {
                    result.Add(comp);
                    queue.Enqueue(comp.Name);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"⚠ Unknown component '{name}'. Skipping.");
                Console.ResetColor();
            }
        }

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            foreach (var dependency in ComponentRegistry.Components[current].Dependencies)
            {
                if (ComponentRegistry.Components.TryGetValue(dependency, out var dep) && visited.Add(dep.Name))
                {
                    Console.WriteLine($"   (dependency: {dep.Name})");
                    result.Insert(0, dep);
                    queue.Enqueue(dep.Name);
                }
            }
        }

        return result;
    }
}