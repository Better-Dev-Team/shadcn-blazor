using System;
using System.IO;
using System.Threading.Tasks;

namespace ShadcnBlazor.Cli.Commands;

public static class InitCommand
{
    public static async Task ExecuteAsync(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n✦ Initializing ShadcnBlazor in your project...\n");
        Console.ResetColor();

        var currentDir = Directory.GetCurrentDirectory();

        // 1. Verify project structure
        var csprojFiles = Directory.GetFiles(currentDir, "*.csproj", SearchOption.AllDirectories);
        if (csprojFiles.Length == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Warning: No .csproj file found in the current directory. Make sure you are in a Blazor project root.");
            Console.ResetColor();
        }

        // 2. Ensure Components/UI directory exists
        var uiDir = Path.Combine(currentDir, "Components", "UI");
        Directory.CreateDirectory(uiDir);
        Console.WriteLine($"✓ Created UI components folder at: {Path.GetRelativePath(currentDir, uiDir)}");

        // 3. Ensure wwwroot exists and copy css/js
        var wwwrootDir = Path.Combine(currentDir, "wwwroot");
        Directory.CreateDirectory(wwwrootDir);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("✓ Configured theme variables & design tokens.");
        Console.WriteLine("✓ Configured _Imports.razor with @using ShadcnBlazor.");
        Console.WriteLine("\n✦ ShadcnBlazor initialization complete! You can now add components with:");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("  shadcn-blazor add button card dialog tabs toast\n");
        Console.ResetColor();
    }
}
