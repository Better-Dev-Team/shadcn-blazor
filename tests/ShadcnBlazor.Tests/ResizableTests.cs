using Bunit;
using Xunit;

namespace ShadcnBlazor.Tests;

public class ResizableTests : TestContext
{
    [Fact]
    public void Resizable_RendersPanelGroupAndHandles()
    {
        var cut = RenderComponent<ResizablePanelGroup>(parameters => parameters
            .Add(p => p.Direction, ResizableOrientation.Horizontal)
            .AddChildContent(builder =>
            {
                builder.OpenComponent<ResizablePanel>(0);
                builder.AddAttribute(1, "DefaultSize", 50.0);
                builder.AddAttribute(2, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(3, "Panel 1")));
                builder.CloseComponent();

                builder.OpenComponent<ResizableHandle>(4);
                builder.AddAttribute(5, "WithHandle", true);
                builder.CloseComponent();

                builder.OpenComponent<ResizablePanel>(6);
                builder.AddAttribute(7, "DefaultSize", 50.0);
                builder.AddAttribute(8, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(9, "Panel 2")));
                builder.CloseComponent();
            })
        );

        Assert.Contains("Panel 1", cut.Markup);
        Assert.Contains("Panel 2", cut.Markup);
        Assert.Contains("data-panel-group-direction=\"horizontal\"", cut.Markup);
    }
}
