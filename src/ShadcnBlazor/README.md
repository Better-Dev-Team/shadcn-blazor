# BetterDev.ShadcnBlazor ✦

> Beautifully designed, accessible, copy-paste and package-ready Razor components for Blazor (.NET 8 & .NET 10) inspired by shadcn/ui.

[![Live Documentation](https://img.shields.io/badge/docs-live-brightgreen.svg)](https://better-dev-team.github.io/shadcn-blazor/)
[![License: MIT](https://img.shields.io/badge/License-MIT-black.svg)](https://github.com/Better-Dev-Team/shadcn-blazor/blob/master/LICENSE)

---

## ⚡ Installation

Install via the .NET CLI:
```bash
dotnet add package BetterDev.ShadcnBlazor
```

Or via the Package Manager Console:
```powershell
Install-Package BetterDev.ShadcnBlazor
```

---

## 🚀 Quick Setup

### 1. Register Services
In your `Program.cs`:
```csharp
using ShadcnBlazor;

builder.Services.AddShadcnBlazor();
```

### 2. Add Imports
In your `_Imports.razor`:
```razor
@using ShadcnBlazor
```

### 3. Add CSS & JS references
In your `App.razor` or `index.html`:
```html
<link rel="stylesheet" href="_content/ShadcnBlazor/shadcn-blazor.css" />
<script src="_content/ShadcnBlazor/shadcn-blazor.js"></script>
```

---

## 🎨 Components Included

- **Buttons & Indicators**: `Button`, `Badge`
- **Layout & Cards**: `Card`, `CardHeader`, `CardTitle`, `CardDescription`, `CardContent`, `CardFooter`, `Separator`
- **Modals & Overlays**: `Dialog`, `AlertDialog`, `Sheet`, `Popover`, `Tooltip`, `DropdownMenu`
- **Forms & Controls**: `Input`, `Textarea`, `Label`, `Checkbox`, `Switch`, `RadioGroup`, `Select`, `Slider`
- **Navigation & Feedback**: `Tabs`, `Accordion`, `Breadcrumb`, `Table`, `Progress`, `Skeleton`, `Avatar`, `Toaster`

---

## 🧪 Example Usage

```razor
<Card Class="w-[350px]">
    <CardHeader>
        <CardTitle>Create project</CardTitle>
        <CardDescription>Deploy your new project in one click.</CardDescription>
    </CardHeader>
    <CardContent>
        <div class="space-y-2">
            <Label>Project Name</Label>
            <Input Placeholder="my-blazor-app" />
        </div>
    </CardContent>
    <CardFooter Class="flex justify-between">
        <Button Variant="ButtonVariant.Outline">Cancel</Button>
        <Button>Deploy</Button>
    </CardFooter>
</Card>
```

---

## 📖 Full Documentation & Interactive Demos

Visit the official documentation:  
👉 **[https://better-dev-team.github.io/shadcn-blazor/](https://better-dev-team.github.io/shadcn-blazor/)**
