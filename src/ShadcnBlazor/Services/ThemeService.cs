using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace ShadcnBlazor;

public class ThemeService : IThemeService
{
    private readonly IJSRuntime _jsRuntime;
    private ThemeMode _mode = ThemeMode.System;
    private ColorPreset _preset = ColorPreset.Zinc;
    private double _radius = 0.5; // rem
    private bool _isSystemDark = false;
    private bool _initialized = false;

    public ThemeMode Mode => _mode;
    public ColorPreset Preset => _preset;
    public double Radius => _radius;
    public bool IsDark => _mode switch
    {
        ThemeMode.Light => false,
        ThemeMode.Dark => true,
        _ => _isSystemDark
    };

    public event Action? OnChange;

    public ThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        if (_initialized) return;

        try
        {
            var savedTheme = await _jsRuntime.InvokeAsync<ThemeStateDto?>("shadcnBlazor.getThemeState");
            if (savedTheme != null)
            {
                if (Enum.TryParse<ThemeMode>(savedTheme.Mode, true, out var m)) _mode = m;
                if (Enum.TryParse<ColorPreset>(savedTheme.Preset, true, out var p)) _preset = p;
                if (savedTheme.Radius > 0) _radius = savedTheme.Radius;
                _isSystemDark = savedTheme.IsSystemDark;
            }
            else
            {
                _isSystemDark = await _jsRuntime.InvokeAsync<bool>("shadcnBlazor.getSystemDarkMode");
            }
        }
        catch
        {
            // Fallback if JS runtime isn't ready during prerendering
        }

        _initialized = true;
        await ApplyThemeAsync();
        NotifyStateChanged();
    }

    public async Task SetModeAsync(ThemeMode mode)
    {
        _mode = mode;
        await ApplyThemeAsync();
        NotifyStateChanged();
    }

    public async Task SetPresetAsync(ColorPreset preset)
    {
        _preset = preset;
        await ApplyThemeAsync();
        NotifyStateChanged();
    }

    public async Task SetRadiusAsync(double radius)
    {
        _radius = Math.Clamp(radius, 0.0, 1.5);
        await ApplyThemeAsync();
        NotifyStateChanged();
    }

    private async Task ApplyThemeAsync()
    {
        try
        {
            var isDark = IsDark;
            var presetStr = _preset.ToString().ToLowerInvariant();
            await _jsRuntime.InvokeVoidAsync("shadcnBlazor.applyTheme", new
            {
                mode = _mode.ToString().ToLowerInvariant(),
                isDark,
                preset = presetStr,
                radius = _radius
            });
        }
        catch
        {
            // JS Runtime might not be available in static rendering
        }
    }

    private void NotifyStateChanged() => OnChange?.Invoke();

    public class ThemeStateDto
    {
        public string? Mode { get; set; }
        public string? Preset { get; set; }
        public double Radius { get; set; }
        public bool IsSystemDark { get; set; }
    }
}
