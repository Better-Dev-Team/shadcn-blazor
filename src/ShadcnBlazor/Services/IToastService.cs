using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;

public class ToastModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ToastType Type { get; set; } = ToastType.Default;
    public int DurationMs { get; set; } = 4000;
    public RenderFragment? CustomIcon { get; set; }
    public string? ActionText { get; set; }
    public Func<Task>? OnAction { get; set; }
    public string? CancelText { get; set; }
    public Func<Task>? OnCancel { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDismissed { get; set; }
}

public interface IToastService
{
    IReadOnlyList<ToastModel> Toasts { get; }
    event Action? OnChange;

    ToastModel Show(string title, string? description = null, ToastType type = ToastType.Default, int durationMs = 4000, string? actionText = null, Func<Task>? onAction = null);
    ToastModel Success(string title, string? description = null, int durationMs = 4000);
    ToastModel Error(string title, string? description = null, int durationMs = 4000);
    ToastModel Info(string title, string? description = null, int durationMs = 4000);
    ToastModel Warning(string title, string? description = null, int durationMs = 4000);
    ToastModel Loading(string title, string? description = null);
    Task PromiseAsync(Func<Task> operation, string loadingText, string successText, string errorText);
    void Dismiss(Guid id);
    void DismissAll();
}
