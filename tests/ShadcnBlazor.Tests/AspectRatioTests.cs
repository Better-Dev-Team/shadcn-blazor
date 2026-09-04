using Bunit;
using Xunit;

namespace ShadcnBlazor.Tests;

public class AspectRatioTests : TestContext
{
    [Fact]
    public void AspectRatio_RendersChildContent_AndAppliesCalculatedPadding()
    {
        var cut = RenderComponent<AspectRatio>(parameters => parameters
            .Add(p => p.Ratio, 16.0 / 9.0)
            .AddChildContent("<span id=\"child\">Test Content</span>")
        );

        cut.Find("#child").MarkupMatches("<span id=\"child\">Test Content</span>");
        var outer = cut.Find("div");
        Assert.Contains("padding-bottom: 56.25%", outer.GetAttribute("style"));
    }

    [Fact]
    public void AspectRatio_RendersSquare1To1()
    {
        var cut = RenderComponent<AspectRatio>(parameters => parameters
            .Add(p => p.Ratio, 1.0)
        );

        var outer = cut.Find("div");
        Assert.Contains("padding-bottom: 100%", outer.GetAttribute("style"));
    }
}
