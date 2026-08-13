using System;
using System.Threading.Tasks;

namespace ShadcnBlazor;

public interface IThemeService
{
    ThemeMode Mode { get; }
    ColorPreset Preset { get; }
    double Radius { get; }
    bool IsDark { get; }

    event Action? OnChange;

    Task SetModeAsync(ThemeMode mode);
    Task SetPresetAsync(ColorPreset preset);
    Task SetRadiusAsync(double radius);
    Task InitializeAsync();
}
