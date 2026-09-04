using Bunit;
using Xunit;

namespace ShadcnBlazor.Tests;

public class BadgeTests : TestContext
{
    [Fact]
    public void Badge_RendersDefaultVariant()
    {
        var cut = RenderComponent<Badge>(parameters => parameters
            .AddChildContent("New")
        );

        var badge = cut.Find("div");
        Assert.Equal("New", badge.TextContent.Trim());
        Assert.Contains("bg-primary", badge.ClassName);
    }

    [Fact]
    public void Badge_RendersSecondaryVariant()
    {
        var cut = RenderComponent<Badge>(parameters => parameters
            .Add(p => p.Variant, BadgeVariant.Secondary)
            .AddChildContent("Draft")
        );

        var badge = cut.Find("div");
        Assert.Contains("bg-secondary", badge.ClassName);
    }
}
