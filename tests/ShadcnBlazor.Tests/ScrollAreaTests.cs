using Bunit;
using Xunit;

namespace ShadcnBlazor.Tests;

public class ScrollAreaTests : TestContext
{
    [Fact]
    public void ScrollArea_RendersWithVerticalOrientationByDefault()
    {
        var cut = RenderComponent<ScrollArea>(parameters => parameters
            .AddChildContent("<p>Scrollable text</p>")
        );

        var viewport = cut.Find("div > div");
        Assert.Contains("overflow-y-auto", viewport.ClassName);
        Assert.Contains("overflow-x-hidden", viewport.ClassName);
        Assert.Contains("Scrollable text", cut.Markup);
    }

    [Fact]
    public void ScrollArea_RendersHorizontalOrientation()
    {
        var cut = RenderComponent<ScrollArea>(parameters => parameters
            .Add(p => p.Orientation, ScrollOrientation.Horizontal)
            .AddChildContent("<p>Horizontal</p>")
        );

        var viewport = cut.Find("div > div");
        Assert.Contains("overflow-x-auto", viewport.ClassName);
        Assert.Contains("overflow-y-hidden", viewport.ClassName);
    }
}
