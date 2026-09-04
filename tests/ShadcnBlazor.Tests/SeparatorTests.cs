using Bunit;
using Xunit;

namespace ShadcnBlazor.Tests;

public class SeparatorTests : TestContext
{
    [Fact]
    public void Separator_RendersHorizontalByDefault()
    {
        var cut = RenderComponent<Separator>();

        var div = cut.Find("div");
        Assert.Contains("h-[1px]", div.ClassName);
        Assert.Contains("w-full", div.ClassName);
    }

    [Fact]
    public void Separator_RendersVerticalOrientation()
    {
        var cut = RenderComponent<Separator>(parameters => parameters
            .Add(p => p.Orientation, "vertical")
        );

        var div = cut.Find("div");
        Assert.Contains("h-full", div.ClassName);
        Assert.Contains("w-[1px]", div.ClassName);
    }
}
