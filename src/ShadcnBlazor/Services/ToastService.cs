using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShadcnBlazor;

public class ToastService : IToastService
{
    private readonly List<ToastModel> _toasts = new();

    public IReadOnlyList<ToastModel> Toasts => _toasts.AsReadOnly();
    public event Action? OnChange;

    public ToastModel Show(string title, string? description = null, ToastType type = ToastType.Default, int durationMs = 4000, string? actionText = null, Func<Task>? onAction = null)
    {
        var toast = new ToastModel
        {
            Title = title,
            Description = description,
            Type = type,
            DurationMs = durationMs,
            ActionText = actionText,
            OnAction = onAction
        };

        _toasts.Add(toast);
        NotifyStateChanged();

        if (durationMs > 0 && type != ToastType.Loading)
        {
            _ = AutoDismissAsync(toast.Id, durationMs);
        }

        return toast;
    }

    public ToastModel Success(string title, string? description = null, int durationMs = 4000)
        => Show(title, description, ToastType.Success, durationMs);

    public ToastModel Error(string title, string? description = null, int durationMs = 4000)
        => Show(title, description, ToastType.Error, durationMs);

    public ToastModel Info(string title, string? description = null, int durationMs = 4000)
        => Show(title, description, ToastType.Info, durationMs);

    public ToastModel Warning(string title, string? description = null, int durationMs = 4000)
        => Show(title, description, ToastType.Warning, durationMs);

    public ToastModel Loading(string title, string? description = null)
        => Show(title, description, ToastType.Loading, durationMs: 0);

    public async Task PromiseAsync(Func<Task> operation, string loadingText, string successText, string errorText)
    {
        var toast = Loading(loadingText);
        try
        {
            await operation();
            toast.Title = successText;
            toast.Type = ToastType.Success;
            toast.DurationMs = 4000;
            NotifyStateChanged();
            _ = AutoDismissAsync(toast.Id, 4000);
        }
        catch (Exception ex)
        {
            toast.Title = errorText;
            toast.Description = ex.Message;
            toast.Type = ToastType.Error;
            toast.DurationMs = 5000;
            NotifyStateChanged();
            _ = AutoDismissAsync(toast.Id, 5000);
        }
    }

    public void Dismiss(Guid id)
    {
        var toast = _toasts.FirstOrDefault(t => t.Id == id);
        if (toast != null)
        {
            toast.IsDismissed = true;
            _toasts.Remove(toast);
            NotifyStateChanged();
        }
    }

    public void DismissAll()
    {
        _toasts.Clear();
        NotifyStateChanged();
    }

    private async Task AutoDismissAsync(Guid id, int delayMs)
    {
        await Task.Delay(delayMs);
        Dismiss(id);
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
