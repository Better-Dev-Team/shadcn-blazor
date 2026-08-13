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

        // 6. Register DI services in Program.cs
        ConfigureProgramCs(currentDir);

        // 7. Wire stylesheet & interop script into wwwroot/index.html
        ConfigureIndexHtml(currentDir);

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

    private static void ConfigureProgramCs(string projectDir)
    {
        var programPath = Path.Combine(projectDir, "Program.cs");
        if (!File.Exists(programPath))
        {
            Console.WriteLine("  ⚠ Program.cs not found. Add builder.Services.AddShadcnBlazor(); manually.");
            return;
        }

        var content = File.ReadAllText(programPath);
        if (content.Contains("AddShadcnBlazor", StringComparison.Ordinal))
        {
            Console.WriteLine("  - Skipped Program.cs (AddShadcnBlazor already registered)");
            return;
        }

        const string servicesLine = "builder.Services.AddShadcnBlazor();";
        var lines = new List<string>(content.Split('\n'));

        if (!content.Contains("using ShadcnBlazor", StringComparison.Ordinal))
        {
            lines.Insert(0, "using ShadcnBlazor;");
        }

        var headOutletIndex = lines.FindIndex(l => l.Contains("HeadOutlet", StringComparison.Ordinal));
        var insertAt = headOutletIndex >= 0 ? headOutletIndex + 1 : -1;
        if (insertAt < 0)
        {
            var buildIndex = lines.FindIndex(l => l.Contains("builder.Build()", StringComparison.Ordinal));
            insertAt = buildIndex;
        }

        if (insertAt >= 0)
        {
            lines.Insert(insertAt, servicesLine);
            File.WriteAllText(programPath, string.Join("\n", lines));
            Console.WriteLine("  ✓ Program.cs configured with builder.Services.AddShadcnBlazor().");
        }
        else
        {
            Console.WriteLine("  ⚠ Could not find an insertion point in Program.cs. Add builder.Services.AddShadcnBlazor(); manually.");
        }
    }

    private static void ConfigureIndexHtml(string projectDir)
    {
        var indexPath = Path.Combine(projectDir, "wwwroot", "index.html");
        if (!File.Exists(indexPath))
        {
            Console.WriteLine("  ⚠ wwwroot/index.html not found. Link shadcn-blazor.css and shadcn-blazor.js manually.");
            return;
        }

        var content = File.ReadAllText(indexPath);
        var changed = false;

        RemoveBootstrapLink(ref content, ref changed);

        if (content.Contains("shadcn-blazor.css", StringComparison.Ordinal))
        {
            Console.WriteLine("  - Skipped index.html (shadcn-blazor.css already linked)");
        }
        else
        {
            const string cssLink = "    <link rel=\"stylesheet\" href=\"shadcn-blazor.css\" />";
            var headClose = content.IndexOf("</head>", StringComparison.OrdinalIgnoreCase);
            if (headClose >= 0)
            {
                content = content.Insert(headClose, cssLink + Environment.NewLine);
                changed = true;
            }
            else
            {
                Console.WriteLine("  ⚠ Could not find </head> in index.html. Link shadcn-blazor.css manually.");
            }
        }

        if (content.Contains("shadcn-blazor.js", StringComparison.Ordinal))
        {
            Console.WriteLine("  - Skipped index.html (shadcn-blazor.js already referenced)");
        }
        else
        {
            const string jsScript = "    <script src=\"shadcn-blazor.js\"></script>";
            var frameworkIdx = content.IndexOf("_framework/blazor.webassembly", StringComparison.OrdinalIgnoreCase);
            if (frameworkIdx < 0)
            {
                frameworkIdx = content.IndexOf("_framework/blazor.web.js", StringComparison.OrdinalIgnoreCase);
            }

            if (frameworkIdx >= 0)
            {
                var lineStart = content.LastIndexOf("\n", frameworkIdx) + 1;
                content = content.Insert(lineStart, jsScript + Environment.NewLine);
                changed = true;
            }
            else
            {
                Console.WriteLine("  ⚠ Could not find Blazor bootstrap script. Link shadcn-blazor.js manually.");
            }
        }

        if (changed)
        {
            File.WriteAllText(indexPath, content);
            Console.WriteLine("  ✓ index.html wired with ShadcnBlazor stylesheet & interop script.");
        }
    }

    private static void RemoveBootstrapLink(ref string content, ref bool changed)
    {
        var bootstrapIdx = content.IndexOf("bootstrap.min.css", StringComparison.OrdinalIgnoreCase);
        if (bootstrapIdx < 0)
        {
            return;
        }

        var lineStart = content.LastIndexOf("\n", bootstrapIdx) + 1;
        var lineEnd = content.IndexOf("\n", bootstrapIdx);
        if (lineEnd < 0)
        {
            lineEnd = content.Length;
        }

        content = content.Remove(lineStart, lineEnd - lineStart);
        changed = true;
        Console.WriteLine("  ✓ Removed Bootstrap stylesheet (ShadcnBlazor provides its own styling).");
    }
}