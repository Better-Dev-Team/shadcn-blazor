using Bunit;
using Xunit;

namespace ShadcnBlazor.Tests;

public class KbdTests : TestContext
{
    [Fact]
    public void Kbd_RendersCorrectly_WithDefaultClassesAndChildContent()
    {
        var cut = RenderComponent<Kbd>(parameters => parameters
            .AddChildContent("Ctrl")
            .Add(p => p.Class, "custom-kbd")
        );

        var kbd = cut.Find("kbd");
        Assert.Equal("Ctrl", kbd.TextContent);
        Assert.Contains("custom-kbd", kbd.ClassName);
        Assert.Contains("font-mono", kbd.ClassName);
    }
}
