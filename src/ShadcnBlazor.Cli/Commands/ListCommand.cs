using System;
using ShadcnBlazor.Cli.Registry;

namespace ShadcnBlazor.Cli.Commands;

public static class ListCommand
{
    public static int Execute()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n✦ Available ShadcnBlazor Components:\n");
        Console.ResetColor();

        Console.WriteLine("{0,-20} {1}", "Component", "Description");
        Console.WriteLine(new string('-', 70));

        foreach (var (name, comp) in ComponentRegistry.Components)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("{0,-20} ", name);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(comp.Description);
        }

        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nTo add a component: shadcn-blazor add <component-name>\n");
        Console.ResetColor();

        return 0;
    }
}
