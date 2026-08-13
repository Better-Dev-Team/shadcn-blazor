using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ShadcnBlazor;

/// <summary>
/// High-performance class builder and Tailwind class merger inspired by clsx and tailwind-merge.
/// </summary>
public static class Cn
{
    private static readonly HashSet<string> StandaloneClasses = new(StringComparer.OrdinalIgnoreCase)
    {
        "flex", "inline-flex", "grid", "inline-grid", "block", "inline-block", "inline", "hidden",
        "relative", "absolute", "fixed", "sticky", "static",
        "visible", "invisible", "collapse",
        "italic", "not-italic", "underline", "line-through", "no-underline",
        "uppercase", "lowercase", "capitalize", "normal-case",
        "truncate", "overflow-ellipsis", "overflow-clip",
        "border", "border-solid", "border-dashed", "border-dotted", "border-double", "border-none",
        "rounded", "rounded-none", "rounded-sm", "rounded-md", "rounded-lg", "rounded-xl", "rounded-2xl", "rounded-3xl", "rounded-full"
    };

    /// <summary>
    /// Combines and merges class values, discarding duplicates and conflicting tailwind/utility classes.
    /// </summary>
    public static string Class(params object?[]? inputs)
    {
        if (inputs == null || inputs.Length == 0) return string.Empty;

        var tokens = new List<string>();

        void ProcessInput(object? input)
        {
            if (input == null) return;

            switch (input)
            {
                case string str:
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        var parts = str.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        tokens.AddRange(parts);
                    }
                    break;

                case ValueTuple<bool, string> tuple:
                    if (tuple.Item1 && !string.IsNullOrWhiteSpace(tuple.Item2))
                    {
                        ProcessInput(tuple.Item2);
                    }
                    break;

                case KeyValuePair<string, bool> kvp:
                    if (kvp.Value && !string.IsNullOrWhiteSpace(kvp.Key))
                    {
                        ProcessInput(kvp.Key);
                    }
                    break;

                case IEnumerable enumerable and not string:
                    foreach (var item in enumerable)
                    {
                        ProcessInput(item);
                    }
                    break;

                default:
                    var s = input.ToString();
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        ProcessInput(s);
                    }
                    break;
            }
        }

        foreach (var input in inputs)
        {
            ProcessInput(input);
        }

        return Merge(tokens);
    }

    /// <summary>
    /// Merges an array of class tokens, resolving conflicts so that later classes override earlier conflicting ones.
    /// </summary>
    public static string Merge(IEnumerable<string> tokens)
    {
        var classMap = new Dictionary<string, string>();
        var resultList = new List<string>();

        foreach (var token in tokens)
        {
            if (string.IsNullOrWhiteSpace(token)) continue;

            var cleanToken = token.Trim();
            var key = GetConflictKey(cleanToken);

            if (string.IsNullOrEmpty(key))
            {
                // Standalone without direct conflicts, just deduplicate exact duplicate
                if (!resultList.Contains(cleanToken))
                {
                    resultList.Add(cleanToken);
                }
            }
            else
            {
                if (classMap.TryGetValue(key, out var existing))
                {
                    var index = resultList.IndexOf(existing);
                    if (index >= 0)
                    {
                        resultList[index] = cleanToken;
                    }
                    else
                    {
                        resultList.Add(cleanToken);
                    }
                    classMap[key] = cleanToken;
                }
                else
                {
                    classMap[key] = cleanToken;
                    resultList.Add(cleanToken);
                }
            }
        }

        return string.Join(" ", resultList);
    }

    /// <summary>
    /// Resolves the group key for conflict detection (e.g. "bg-", "p-", "text-", "rounded-").
    /// </summary>
    private static string GetConflictKey(string cls)
    {
        // Handle modifier prefixes (e.g. hover:, dark:, focus:, sm:, md:, lg:, disabled:)
        var modifier = "";
        var lastColon = cls.LastIndexOf(':');
        if (lastColon >= 0)
        {
            modifier = cls.Substring(0, lastColon + 1);
            cls = cls.Substring(lastColon + 1);
        }

        // Exact match standalone categories
        if (cls.StartsWith("bg-")) return modifier + "bg";
        if (cls.StartsWith("text-"))
        {
            if (cls.StartsWith("text-left") || cls.StartsWith("text-center") || cls.StartsWith("text-right") || cls.StartsWith("text-justify"))
                return modifier + "text-align";
            return modifier + "text-color-or-size";
        }
        if (cls.StartsWith("font-")) return modifier + "font";
        if (cls.StartsWith("tracking-")) return modifier + "tracking";
        if (cls.StartsWith("leading-")) return modifier + "leading";

        // Margins & Paddings
        if (cls.StartsWith("p-") || cls == "p") return modifier + "p";
        if (cls.StartsWith("px-")) return modifier + "px";
        if (cls.StartsWith("py-")) return modifier + "py";
        if (cls.StartsWith("pt-")) return modifier + "pt";
        if (cls.StartsWith("pr-")) return modifier + "pr";
        if (cls.StartsWith("pb-")) return modifier + "pb";
        if (cls.StartsWith("pl-")) return modifier + "pl";

        if (cls.StartsWith("m-") || cls == "m") return modifier + "m";
        if (cls.StartsWith("mx-")) return modifier + "mx";
        if (cls.StartsWith("my-")) return modifier + "my";
        if (cls.StartsWith("mt-")) return modifier + "mt";
        if (cls.StartsWith("mr-")) return modifier + "mr";
        if (cls.StartsWith("mb-")) return modifier + "mb";
        if (cls.StartsWith("ml-")) return modifier + "ml";

        // Sizing
        if (cls.StartsWith("w-")) return modifier + "w";
        if (cls.StartsWith("min-w-")) return modifier + "min-w";
        if (cls.StartsWith("max-w-")) return modifier + "max-w";
        if (cls.StartsWith("h-")) return modifier + "h";
        if (cls.StartsWith("min-h-")) return modifier + "min-h";
        if (cls.StartsWith("max-h-")) return modifier + "max-h";

        // Borders & Radius
        if (cls.StartsWith("rounded-")) return modifier + "rounded";
        if (cls == "rounded") return modifier + "rounded";
        if (cls.StartsWith("border-") && (cls.EndsWith("-0") || cls.EndsWith("-2") || cls.EndsWith("-4") || cls.EndsWith("-8") || cls.EndsWith("-t") || cls.EndsWith("-b") || cls.EndsWith("-l") || cls.EndsWith("-r")))
            return modifier + "border-width";
        if (cls.StartsWith("border-")) return modifier + "border-color";
        if (cls == "border") return modifier + "border-width";

        // Shadows & Rings
        if (cls.StartsWith("shadow-") || cls == "shadow") return modifier + "shadow";
        if (cls.StartsWith("ring-") || cls == "ring") return modifier + "ring";
        if (cls.StartsWith("ring-offset-")) return modifier + "ring-offset";

        // Flex & Grid
        if (cls.StartsWith("flex-") && (cls == "flex-row" || cls == "flex-col" || cls == "flex-row-reverse" || cls == "flex-col-reverse"))
            return modifier + "flex-direction";
        if (cls.StartsWith("items-")) return modifier + "items";
        if (cls.StartsWith("justify-")) return modifier + "justify";
        if (cls.StartsWith("gap-")) return modifier + "gap";
        if (cls.StartsWith("gap-x-")) return modifier + "gap-x";
        if (cls.StartsWith("gap-y-")) return modifier + "gap-y";

        // Display
        if (cls is "block" or "inline-block" or "inline" or "flex" or "inline-flex" or "grid" or "inline-grid" or "hidden")
            return modifier + "display";

        // Positions
        if (cls is "relative" or "absolute" or "fixed" or "sticky" or "static")
            return modifier + "position";

        // Opacity
        if (cls.StartsWith("opacity-")) return modifier + "opacity";

        // Cursor
        if (cls.StartsWith("cursor-")) return modifier + "cursor";

        // Transitions
        if (cls.StartsWith("transition-") || cls == "transition") return modifier + "transition";
        if (cls.StartsWith("duration-")) return modifier + "duration";

        return modifier + cls;
    }
}
