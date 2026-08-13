using Microsoft.Extensions.DependencyInjection;

namespace ShadcnBlazor;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers ShadcnBlazor core services (ThemeService, ToastService, etc.) into the dependency injection container.
    /// </summary>
    public static IServiceCollection AddShadcnBlazor(this IServiceCollection services)
    {
        services.AddScoped<IThemeService, ThemeService>();
        services.AddScoped<IToastService, ToastService>();
        return services;
    }
}
