using System.Collections.Generic;
using ShadcnBlazor;
using Xunit;

namespace ShadcnBlazor.Tests;

public class CnTests
{
    [Fact]
    public void Class_CombinesSimpleStrings()
    {
        var result = Cn.Class("btn", "btn-primary", "p-4");
        Assert.Equal("btn btn-primary p-4", result);
    }

    [Fact]
    public void Class_ResolvesConflictingPadding()
    {
        // Later p-4 should override p-2
        var result = Cn.Class("p-2", "p-4");
        Assert.Equal("p-4", result);
    }

    [Fact]
    public void Class_ResolvesConflictingBackground()
    {
        // Later bg-primary should override bg-destructive
        var result = Cn.Class("bg-destructive", "bg-primary");
        Assert.Equal("bg-primary", result);
    }

    [Fact]
    public void Class_HandlesConditionalTuples()
    {
        bool isActive = true;
        bool isDisabled = false;

        var result = Cn.Class(
            "base-class",
            (isActive, "active-class"),
            (isDisabled, "disabled-class")
        );

        Assert.Equal("base-class active-class", result);
    }

    [Fact]
    public void Class_HandlesConditionalDictionary()
    {
        var dict = new Dictionary<string, bool>
        {
            ["visible"] = true,
            ["hidden"] = false
        };

        var result = Cn.Class("block", dict);
        Assert.Equal("block visible", result);
    }

    [Fact]
    public void Class_IgnoresNullAndWhitespace()
    {
        var result = Cn.Class("first", null, "   ", "", "second");
        Assert.Equal("first second", result);
    }

    [Fact]
    public void Class_ResolvesModifierConflictsSeparately()
    {
        // hover:bg-red-500 and hover:bg-blue-500 should resolve hover:bg
        // but not conflict with regular bg-primary
        var result = Cn.Class("bg-primary hover:bg-red-500 hover:bg-blue-500");
        Assert.Equal("bg-primary hover:bg-blue-500", result);
    }
}
