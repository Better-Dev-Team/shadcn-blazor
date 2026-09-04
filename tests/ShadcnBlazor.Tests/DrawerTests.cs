using Bunit;
using Xunit;

namespace ShadcnBlazor.Tests;

public class DrawerTests : TestContext
{
    [Fact]
    public void Drawer_WhenClosed_DoesNotRenderContentSurface()
    {
        var cut = RenderComponent<Drawer>(parameters => parameters
            .Add(p => p.Open, false)
            .AddChildContent(builder =>
            {
                builder.OpenComponent<DrawerContent>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(2, "Drawer Body")));
                builder.CloseComponent();
            })
        );

        Assert.DoesNotContain("Drawer Body", cut.Markup);
    }

    [Fact]
    public void Drawer_WhenOpen_RendersContentAndHandle()
    {
        var cut = RenderComponent<Drawer>(parameters => parameters
            .Add(p => p.Open, true)
            .AddChildContent(builder =>
            {
                builder.OpenComponent<DrawerContent>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(2, "Drawer Body")));
                builder.CloseComponent();
            })
        );

        Assert.Contains("Drawer Body", cut.Markup);
        Assert.Contains("role=\"dialog\"", cut.Markup);
    }
}
