# BetterDev.ShadcnBlazor.Cli ✦

> Official CLI tool for scaffolding ShadcnBlazor components directly into your Blazor projects with full source code ownership.

[![Live Documentation](https://img.shields.io/badge/docs-live-brightgreen.svg)](https://better-dev-team.github.io/shadcn-blazor/)
[![License: MIT](https://img.shields.io/badge/License-MIT-black.svg)](https://github.com/Better-Dev-Team/shadcn-blazor/blob/master/LICENSE)

---

## ⚡ Installation

Install as a global .NET CLI tool:
```bash
dotnet tool install -g BetterDev.ShadcnBlazor.Cli
```

To update to the latest version:
```bash
dotnet tool update -g BetterDev.ShadcnBlazor.Cli
```

---

## 🚀 Usage

### 1. Initialize a Blazor Project
Run inside your Blazor project folder:
```bash
shadcn-blazor init
```
This creates the necessary folders, configuration, and CSS token setups.

---

### 2. List Available Components
```bash
shadcn-blazor list
```

---

### 3. Add Components Directly to Your Project
Scaffold pure Razor source code directly into `Components/UI/`:
```bash
# Add specific components
shadcn-blazor add button card dialog tabs toast

# Add all components
shadcn-blazor add --all
```

---

## 📖 Full Documentation & Showcase

👉 **[https://better-dev-team.github.io/shadcn-blazor/](https://better-dev-team.github.io/shadcn-blazor/)**
