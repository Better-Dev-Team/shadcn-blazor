# AGENTS.md

Guidance for AI agents and contributors working in this repository.

## Overview

ShadcnBlazor: a shadcn/ui-style component library for Blazor, plus a CLI tool for scaffolding components into user projects, and a docs website (Blazor WASM).

- `src/ShadcnBlazor` – component library (Razor Class Library, PackageId `BetterDev.ShadcnBlazor`)
- `src/ShadcnBlazor.Cli` – .NET tool (`BetterDev.ShadcnBlazor.Cli`, command: `shadcn-blazor`). Component sources from `src/ShadcnBlazor` are embedded into the tool assembly at build time (`EmbeddedResource` items in its csproj) so scaffolding works standalone.
- `src/ShadcnBlazor.Docs` – the docs site (Blazor WASM on .NET 10, Tailwind CSS via CDN)
- `tests/ShadcnBlazor.Tests` – bUnit-based unit tests

## CLI `init` integration (what it must do)

`shadcn-blazor init` is not just a file copier — it fully integrates the library into the consumer project:
- Copies the shared support layer into `Components/UI/` (Cn, Variants, services) and `wwwroot/shadcn-blazor.{css,js}`
- Adds `builder.Services.AddShadcnBlazor();` (+ `using ShadcnBlazor;`) to `Program.cs`
- Wires the stylesheet & interop script into `wwwroot/index.html` and removes the template's Bootstrap `<link>`
- Adds `@using ShadcnBlazor` to `_Imports.razor`

All steps are idempotent (skip existing). Any new integration step added to `init` must follow this pattern and update the README + docs.

## Build / test / run

```powershell
dotnet build
dotnet test
dotnet run --project src/ShadcnBlazor.Docs
dotnet pack src/ShadcnBlazor.Cli/ShadcnBlazor.Cli.csproj -c Release -o nupkg
npm run build:css   # regenerate the compiled Tailwind stylesheet (requires npm install first)
```

## Compiled stylesheet (gotcha)

`src/ShadcnBlazor/wwwroot/shadcn-blazor.css` is **generated** — edit `src/ShadcnBlazor/styles/input.css` instead, then run `npm run build:css` (Tailwind v4 CLI + `scripts/unlayer-css.cjs`).

The css is intentionally compiled **without cascade layers** (unlayered): unlayered rules beat Tailwind v4's `@layer utilities` output, so a plain unlayered stylesheet like Bootstrap would override every utility class otherwise. `scripts/unlayer-css.cjs` strips the `@layer` wrappers; never remove that step.

The CLI embeds this css and `init` also **removes the Bootstrap `<link>`** from the consumer's `index.html` to avoid utility collisions.

## Versioning is mandatory before publishing (IMPORTANT)

NuGet does not allow republishing the same version. Any change to `src/ShadcnBlazor`, `src/ShadcnBlazor.Cli`, or their pack metadata **must** include a version bump in the affected project's `Version` property (and `PackageId` must stay the same):

- Library: `src/ShadcnBlazor/ShadcnBlazor.csproj`
- CLI: `src/ShadcnBlazor.Cli/ShadcnBlazor.Cli.csproj`

Bump patch for bug fixes, minor for features, major for breaking changes. Do this in the same commit as the change. Existing published versions:
- `BetterDev.ShadcnBlazor` – 1.0.0
- `BetterDev.ShadcnBlazor.Cli` – 1.0.1

## NuGet publishing

`.github/workflows/publish-nuget.yml` publishes on GitHub release or manual `workflow_dispatch`. It uses NuGet Trusted Publishing (OIDC) via `NuGet/login@v1` with the nuget.org username `TeRiRi606`, and falls back to the `NUGET_API_KEY` secret if set. The login step's temp key output `steps.login.outputs.NUGET_API_KEY` **must** be passed to `dotnet nuget push` — do not remove that wiring. The job runs against the `production` GitHub environment; `secrets` cannot be used inside `if:` conditions (GitHub rejects it at parse time), so all auth branching happens inside the push script instead.

Docs site deploys to GitHub Pages (`deploy-pages.yml`) and Vercel (`deploy-vercel.yml`).

## Static asset paths (gotcha)

The `PackageId` values (`BetterDev.ShadcnBlazor`) override the library's static web asset base path. Library assets are therefore served at `_content/BetterDev.ShadcnBlazor/...`, **not** `_content/ShadcnBlazor/...`. Any reference in `index.html`, docs, or generated output must use `_content/BetterDev.ShadcnBlazor/shadcn-blazor.{js,css}`.

## Docs conventions

- Theme state lives in `IThemeService` (`src/ShadcnBlazor/Services`), persisted to `localStorage` via `shadcnBlazor.applyTheme` in `src/ShadcnBlazor/wwwroot/shadcn-blazor.js`.
- Components that display theme state subscribe to `IThemeService.OnChange` (with `IDisposable` + unsubscription) so UI stays in sync.
- The navbar header must not use `backdrop-blur`/`backdrop-filter`: it makes the header the containing block for `position: fixed` descendants, which traps overlays like the theme customizer's Sheet inside the navbar strip.
- `index.html` applies the saved theme inline before Blazor boots (no-flash bootstrap).

## Code style

- C#, nullable enabled, implicit usings. `.NET 10`.
- Components use Tailwind utility classes; no `.razor.css` stylesheets for shared components.
- Do not add code comments unless asked.