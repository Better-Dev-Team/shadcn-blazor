# ShadcnBlazor ✦

[![Build Status](https://github.com/Better-Dev-Team/shadcn-blazor/actions/workflows/ci.yml/badge.svg)](https://github.com/Better-Dev-Team/shadcn-blazor/actions/workflows/ci.yml)
[![Live Documentation](https://img.shields.io/badge/docs-live-brightgreen.svg)](https://better-dev-team.github.io/shadcn-blazor/)
[![License: MIT](https://img.shields.io/badge/License-MIT-black.svg)](LICENSE)

> **The shadcn/ui component ecosystem, crafted for Blazor (.NET 8 / .NET 10).**
> Beautifully designed, accessible, copy-paste and package-ready Razor components styled with Tailwind CSS and CSS variable design tokens.

---

## ⚡ Features

- **Code Ownership First**: Copy components directly into your project via the CLI (`shadcn-blazor add <component>`) or consume via the `BetterDev.ShadcnBlazor` Razor Class Library.
- **Tailwind CSS & Token System**: Built-in CSS variables (`--background`, `--foreground`, `--primary`, `--radius`, `--card`, etc.) with dark mode and 12 color presets (Zinc, Slate, Stone, Gray, Violet, Rose, Blue, Green, Orange, etc.).
- **High-Performance `Cn.cs` Class Merger**: C# port of `clsx` + `tailwind-merge` resolving conflicting utility classes and supporting conditional tuples/dictionaries.
- **Accessible & Responsive Headless Primitives**: Pure Blazor reactivity combined with lightweight JS Interop for popper positioning, focus trapping, and click-outside handling.
- **20+ Rich Components**:
  - `Button`, `Badge`, `Card`, `Dialog`, `AlertDialog`, `Sheet (Drawer)`, `DropdownMenu`, `Popover`, `Tooltip`, `Tabs`, `Accordion`, `Avatar`, `Input`, `Textarea`, `Checkbox`, `Switch`, `RadioGroup`, `Select`, `Table`, `Progress`, `Slider`, `Skeleton`, `Separator`, `Breadcrumb`, `Toaster / Sonner`.
- **Interactive Documentation & Live Showcase**: Complete Blazor WebAssembly documentation app with live preview sandboxes, code copy, and real-time Theme Customizer.
- **Live Demo Site**: [https://better-dev-team.github.io/shadcn-blazor/](https://better-dev-team.github.io/shadcn-blazor/)

---

## 🚀 Getting Started

### Option 1: Install the CLI Scaffolder Tool
```bash
dotnet tool install -g BetterDev.ShadcnBlazor.Cli
```

Then in your Blazor project directory:
```bash
# Initialize project tokens and config
shadcn-blazor init

# Add specific components
shadcn-blazor add button card dialog tabs toast

# Or add all components
shadcn-blazor add --all
```

---

### Option 2: Add the NuGet Package Directly
```bash
dotnet add package BetterDev.ShadcnBlazor
```

Register services in `Program.cs`:
```csharp
using ShadcnBlazor;

builder.Services.AddShadcnBlazor();
```

In `_Imports.razor`:
```razor
@using ShadcnBlazor
```

---

## 🎨 Theming & Color Presets

Switch between light/dark mode and color presets programmatically or via the UI:

```csharp
@inject IThemeService ThemeService

// Switch mode
await ThemeService.SetModeAsync(ThemeMode.Dark);

// Switch palette preset
await ThemeService.SetPresetAsync(ColorPreset.Violet);

// Adjust border radius (rem)
await ThemeService.SetRadiusAsync(0.75);
```

---

## 🧪 Testing & Local Development

Run the automated test suite:
```bash
dotnet test
```

Run the documentation and showcase app locally:
```bash
dotnet run --project src/ShadcnBlazor.Docs
```

Or run via Docker:
```bash
docker compose up --build
```

---

## 📄 License
MIT License. Open source and free for personal & commercial use.
