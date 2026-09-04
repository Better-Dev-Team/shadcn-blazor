using Bunit;
using Xunit;

namespace ShadcnBlazor.Tests;

public class ButtonTests : TestContext
{
    [Fact]
    public void Button_RendersDefaultVariantAndSize()
    {
        var cut = RenderComponent<Button>(parameters => parameters
            .AddChildContent("Click me")
        );

        var button = cut.Find("button");
        Assert.Equal("Click me", button.TextContent.Trim());
        Assert.Contains("bg-primary", button.ClassName);
        Assert.Contains("h-9", button.ClassName);
    }

    [Fact]
    public void Button_RendersDestructiveVariant()
    {
        var cut = RenderComponent<Button>(parameters => parameters
            .Add(p => p.Variant, ButtonVariant.Destructive)
            .AddChildContent("Delete")
        );

        var button = cut.Find("button");
        Assert.Contains("bg-destructive", button.ClassName);
    }

    [Fact]
    public void Button_RendersAnchor_WhenHrefIsProvided()
    {
        var cut = RenderComponent<Button>(parameters => parameters
            .Add(p => p.Href, "https://github.com")
            .AddChildContent("Link")
        );

        var anchor = cut.Find("a");
        Assert.Equal("https://github.com", anchor.GetAttribute("href"));
        Assert.Contains("inline-flex", anchor.ClassName);
    }
}
