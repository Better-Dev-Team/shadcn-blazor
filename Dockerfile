# Multi-stage Dockerfile for ShadcnBlazor.Docs

# Stage 1: Build Blazor WebAssembly static assets
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY nuget.config ./
COPY ShadcnBlazor.slnx ./
COPY src/ShadcnBlazor/ShadcnBlazor.csproj src/ShadcnBlazor/
COPY src/ShadcnBlazor.Docs/ShadcnBlazor.Docs.csproj src/ShadcnBlazor.Docs/
COPY src/ShadcnBlazor.Cli/ShadcnBlazor.Cli.csproj src/ShadcnBlazor.Cli/
COPY tests/ShadcnBlazor.Tests/ShadcnBlazor.Tests.csproj tests/ShadcnBlazor.Tests/

# Restore dependencies
RUN dotnet restore src/ShadcnBlazor.Docs/ShadcnBlazor.Docs.csproj

# Copy source code and publish
COPY . .
RUN dotnet publish src/ShadcnBlazor.Docs/ShadcnBlazor.Docs.csproj -c Release -o /app/publish

# Stage 2: Serve with lightweight Nginx web server
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Copy nginx SPA configuration
COPY nginx.conf /etc/nginx/nginx.conf

# Copy published static files from build stage
COPY --from=build /app/publish/wwwroot .

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
