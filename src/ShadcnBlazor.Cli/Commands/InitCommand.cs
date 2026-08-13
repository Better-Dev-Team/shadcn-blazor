using System;
using System.IO;
using System.Threading.Tasks;
using ShadcnBlazor.Cli.Registry;

namespace ShadcnBlazor.Cli.Commands;

public static class InitCommand
{
    public static async Task<int> ExecuteAsync(string[] args)
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

        // 3. Copy shared support layer (Cn, enums, services) required by the copied components
        CopyAsset(currentDir, "Common/Cn.cs", Path.Combine(uiDir, "Common", "Cn.cs"));
        CopyAsset(currentDir, "Common/Variants.cs", Path.Combine(uiDir, "Common", "Variants.cs"));
        CopyAsset(currentDir, "Services/IThemeService.cs", Path.Combine(uiDir, "Services", "IThemeService.cs"));
        CopyAsset(currentDir, "Services/IToastService.cs", Path.Combine(uiDir, "Services", "IToastService.cs"));
        CopyAsset(currentDir, "Services/ServiceCollectionExtensions.cs", Path.Combine(uiDir, "Services", "ServiceCollectionExtensions.cs"));
        CopyAsset(currentDir, "Services/ThemeService.cs", Path.Combine(uiDir, "Services", "ThemeService.cs"));
        CopyAsset(currentDir, "Services/ToastService.cs", Path.Combine(uiDir, "Services", "ToastService.cs"));

        // 4. Ensure wwwroot exists and copy css/js (design tokens & JS interop)
        var wwwrootDir = Path.Combine(currentDir, "wwwroot");
        Directory.CreateDirectory(wwwrootDir);
        CopyAsset(currentDir, "wwwroot/shadcn-blazor.css", Path.Combine(wwwrootDir, "shadcn-blazor.css"));
        CopyAsset(currentDir, "wwwroot/shadcn-blazor.js", Path.Combine(wwwrootDir, "shadcn-blazor.js"));
        Console.WriteLine("✓ Configured theme variables & design tokens.");

        // 5. Configure _Imports.razor with @using ShadcnBlazor
        ConfigureImports(currentDir);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n✦ ShadcnBlazor initialization complete! You can now add components with:");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("  shadcn-blazor add button card dialog tabs toast\n");
        Console.ResetColor();

        return 0;
    }

    private static bool CopyAsset(string projectDir, string assetPath, string targetPath)
    {
        var relative = Path.GetRelativePath(projectDir, targetPath);
        if (File.Exists(targetPath))
        {
            Console.WriteLine($"  - Skipped {relative} (already exists)");
            return false;
        }

        if (!AssetStore.TryRead(assetPath, out var content))
        {
            Console.WriteLine($"  ⚠ Asset '{assetPath}' not found in package. Skipping.");
            return false;
        }

        var dir = Path.GetDirectoryName(targetPath);
        if (!string.IsNullOrEmpty(dir))
        {
            Directory.CreateDirectory(dir);
        }

        File.WriteAllText(targetPath, content);
        Console.WriteLine($"  ✓ {relative}");
        return true;
    }

    private static void ConfigureImports(string projectDir)
    {
        const string importsLine = "@using ShadcnBlazor";
        var importsPath = Path.Combine(projectDir, "_Imports.razor");

        if (File.Exists(importsPath))
        {
            var content = File.ReadAllText(importsPath);
            if (content.Contains(importsLine, StringComparison.Ordinal))
            {
                Console.WriteLine("  - Skipped _Imports.razor (@using ShadcnBlazor already present)");
                return;
            }

            var separator = content.EndsWith("\n") ? "" : Environment.NewLine;
            File.AppendAllText(importsPath, separator + importsLine + Environment.NewLine);
        }
        else
        {
            File.WriteAllText(importsPath,
                "@using Microsoft.AspNetCore.Components.Web" + Environment.NewLine +
                importsLine + Environment.NewLine);
        }

        Console.WriteLine("  ✓ _Imports.razor configured with @using ShadcnBlazor.");
    }
}