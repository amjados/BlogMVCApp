# Auto-generated Dockerfile by MSBuild
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BlogMVCApp.csproj", "."]
RUN dotnet restore "./BlogMVCApp.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "BlogMVCApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlogMVCApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy the published application
COPY --from=publish /app/publish .

# Create a non-root user for security
RUN groupadd -r appuser && useradd --no-log-init -r -g appuser appuser
RUN chown -R appuser:appuser /app
USER appuser

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/Home/Index || exit 1

ENTRYPOINT ["dotnet", "BlogMVCApp.dll"]
