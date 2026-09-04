using Bunit;
using Xunit;

namespace ShadcnBlazor.Tests;

public class HoverCardTests : TestContext
{
    [Fact]
    public void HoverCard_RendersTriggerCorrectly()
    {
        var cut = RenderComponent<HoverCard>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<HoverCardTrigger>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(2, "Hover Me")));
                builder.CloseComponent();
            })
        );

        Assert.Contains("Hover Me", cut.Markup);
    }
}
