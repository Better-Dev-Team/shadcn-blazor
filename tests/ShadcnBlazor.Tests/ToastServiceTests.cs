using System;
using System.Linq;
using System.Threading.Tasks;
using ShadcnBlazor;
using Xunit;

namespace ShadcnBlazor.Tests;

public class ToastServiceTests
{
    [Fact]
    public void Show_AddsToastToList()
    {
        var service = new ToastService();
        var toast = service.Show("Test Toast", "Description text", ToastType.Success);

        Assert.Single(service.Toasts);
        Assert.Equal("Test Toast", service.Toasts[0].Title);
        Assert.Equal("Description text", service.Toasts[0].Description);
        Assert.Equal(ToastType.Success, service.Toasts[0].Type);
    }

    [Fact]
    public void Dismiss_RemovesToast()
    {
        var service = new ToastService();
        var t1 = service.Success("Title 1");
        var t2 = service.Error("Title 2");

        Assert.Equal(2, service.Toasts.Count);

        service.Dismiss(t1.Id);

        Assert.Single(service.Toasts);
        Assert.Equal(t2.Id, service.Toasts[0].Id);
    }

    [Fact]
    public void DismissAll_ClearsAllToasts()
    {
        var service = new ToastService();
        service.Success("1");
        service.Error("2");
        service.Warning("3");

        Assert.Equal(3, service.Toasts.Count);

        service.DismissAll();

        Assert.Empty(service.Toasts);
    }

    [Fact]
    public async Task PromiseAsync_UpdatesToastOnSuccess()
    {
        var service = new ToastService();
        await service.PromiseAsync(
            async () => await Task.Yield(),
            "Loading...",
            "Operation Successful",
            "Operation Failed"
        );

        var toast = service.Toasts.FirstOrDefault();
        Assert.NotNull(toast);
        Assert.Equal("Operation Successful", toast.Title);
        Assert.Equal(ToastType.Success, toast.Type);
    }
}
