using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ShadcnBlazor.Cli.Registry;

public static class AssetStore
{
    private const string Prefix = "ShadcnBlazor.Cli.assets.";

    private static readonly Dictionary<string, string> Assets = Load();

    public static bool TryRead(string relativePath, out string content)
    {
        var key = Normalize(relativePath);
        if (Assets.TryGetValue(key, out var resourceName))
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                using var reader = new StreamReader(stream);
                content = reader.ReadToEnd();
                return true;
            }
        }

        content = string.Empty;
        return false;
    }

    private static Dictionary<string, string> Load()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var name in assembly.GetManifestResourceNames())
        {
            if (name.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase))
            {
                map[ToRelativePath(name)] = name;
            }
        }

        return map;
    }

    private static string Normalize(string relativePath) =>
        relativePath.Replace('\\', '/');

    private static string ToRelativePath(string resourceName)
    {
        var parts = resourceName.Substring(Prefix.Length).Split('.');
        if (parts.Length < 2)
        {
            return Normalize(resourceName.Substring(Prefix.Length));
        }

        var fileName = parts[^2] + "." + parts[^1];
        var folders = parts.Take(parts.Length - 2);
        return Normalize(folders.Any() ? string.Join("/", folders) + "/" + fileName : fileName);
    }
}