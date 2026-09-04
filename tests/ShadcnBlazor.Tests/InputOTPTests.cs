using Bunit;
using Xunit;

namespace ShadcnBlazor.Tests;

public class InputOTPTests : TestContext
{
    [Fact]
    public void InputOTP_RendersSlotsAndDisplaysValue()
    {
        var cut = RenderComponent<InputOTP>(parameters => parameters
            .Add(p => p.Value, "42")
            .Add(p => p.MaxLength, 4)
            .AddChildContent(builder =>
            {
                builder.OpenComponent<InputOTPGroup>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b =>
                {
                    b.OpenComponent<InputOTPSlot>(0);
                    b.AddAttribute(1, "Index", 0);
                    b.CloseComponent();

                    b.OpenComponent<InputOTPSlot>(2);
                    b.AddAttribute(3, "Index", 1);
                    b.CloseComponent();

                    b.OpenComponent<InputOTPSlot>(4);
                    b.AddAttribute(5, "Index", 2);
                    b.CloseComponent();
                }));
                builder.CloseComponent();
            })
        );

        Assert.Contains("4", cut.Markup);
        Assert.Contains("2", cut.Markup);
    }
}
