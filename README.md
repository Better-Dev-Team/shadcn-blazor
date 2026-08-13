# ShadcnBlazor ✦

[![Build Status](https://github.com/Better-Dev-Team/shadcn-blazor/actions/workflows/ci.yml/badge.svg)](https://github.com/Better-Dev-Team/shadcn-blazor/actions/workflows/ci.yml)
[![Deploy with Vercel](https://vercel.com/button)](https://vercel.com/new/clone?repository-url=https%3A%2F%2Fgithub.com%2FBetter-Dev-Team%2Fshadcn-blazor)
[![License: MIT](https://img.shields.io/badge/License-MIT-black.svg)](LICENSE)

> **The shadcn/ui component ecosystem, crafted for Blazor (.NET 8 / .NET 10).**
> Beautifully designed, accessible, copy-paste and package-ready Razor components styled with Tailwind CSS and CSS variable design tokens.

---

## ⚡ Features

- **Code Ownership First**: Copy components directly into your project via the CLI (`shadcn-blazor add <component>`) or consume via Razor Class Library (RCL).
- **Tailwind CSS & Token System**: Built-in CSS variables (`--background`, `--foreground`, `--primary`, `--radius`, `--card`, etc.) with dark mode and 12 color presets (Zinc, Slate, Stone, Gray, Violet, Rose, Blue, Green, Orange, etc.).
- **High-Performance `Cn.cs` Class Merger**: C# port of `clsx` + `tailwind-merge` resolving conflicting utility classes and supporting conditional tuples/dictionaries.
- **Accessible & Responsive Headless Primitives**: Pure Blazor reactivity combined with lightweight JS Interop for popper positioning, focus trapping, and click-outside handling.
- **20+ Rich Components**:
  - `Button`, `Badge`, `Card`, `Dialog`, `AlertDialog`, `Sheet (Drawer)`, `DropdownMenu`, `Popover`, `Tooltip`, `Tabs`, `Accordion`, `Avatar`, `Input`, `Textarea`, `Checkbox`, `Switch`, `RadioGroup`, `Select`, `Table`, `Progress`, `Slider`, `Skeleton`, `Separator`, `Breadcrumb`, `Toaster / Sonner`.
- **Interactive Documentation & Live Showcase**: Complete Blazor WebAssembly documentation app with live preview sandboxes, code copy, and real-time Theme Customizer.
- **Vercel & Cloud Ready**: Zero-config deployment with `vercel.json` and static SPA rewrites.

---

## 🚀 Getting Started

### 1. Install the CLI Tool
```bash
dotnet tool install -g shadcn-blazor
```

### 2. Initialize in your Blazor Project
```bash
cd MyBlazorApp
shadcn-blazor init
```

### 3. Add Components
```bash
# Add specific components
shadcn-blazor add button card dialog tabs toast

# Or add all components
shadcn-blazor add --all
```

### 4. Register Services & Imports
In `Program.cs`:
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

## 🌐 Deploy to Vercel

You can deploy the interactive documentation and component showcase to Vercel in 1 click:

[![Deploy with Vercel](https://vercel.com/button)](https://vercel.com/new/clone?repository-url=https%3A%2F%2Fgithub.com%2FBetter-Dev-Team%2Fshadcn-blazor)

Or deploy from the CLI:
```bash
# Publish static files
npm run build

# Deploy with Vercel
npx vercel --prod
```

---

## 🧪 Testing & Verification

Run the automated test suite:
```bash
dotnet test
```

Run the documentation and showcase app locally:
```bash
dotnet run --project src/ShadcnBlazor.Docs
```

---

## 📄 License
MIT License. Open source and free for personal & commercial use.
