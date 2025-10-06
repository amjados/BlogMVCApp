# BlogMVCApp - Docker Deployment with MSBuild

This project features a complete Docker deployment automation system using MSBuild custom tasks and targets, externalized for better maintainability.

## 📂 Project Structure

```
BlogMVCApp/
├── Build/                      # Build configuration directory
│   └── Deploy.targets          # External MSBuild deployment targets
├── Controllers/                # MVC Controllers
├── Data/                      # Entity Framework context and entities
├── Models/                    # Application models
├── Views/                     # Razor views
├── wwwroot/                   # Static files
├── BlogMVCApp.csproj          # Clean project file (imports Deploy.targets)
├── Dockerfile                 # Auto-generated Docker configuration
├── docker-compose.yml         # Auto-generated orchestration
└── README.md                  # This file
```

## 🚀 Deployment Commands

### Quick Deployment
```bash
dotnet msbuild -t:QuickDeploy
```
- Generates Docker files and deploys immediately
- Fastest option for development

### Full Deployment Pipeline
```bash
dotnet msbuild -t:Deploy
```
- Complete build, cleanup, and deployment
- Includes health checks and database migrations
- Recommended for production deployments

### Production Deployment
```bash
dotnet msbuild -t:DeployProd
```
- Full deployment with extended health validation
- Production-ready with comprehensive checks

## 🛠️ Management Commands

### Status & Monitoring
```bash
# Check container status and resource usage
dotnet msbuild -t:DockerStatus

# View container logs (last 50 lines)
dotnet msbuild -t:DockerLogs

# Follow logs in real-time
dotnet msbuild -t:DockerLogsTail
```

### Container Management
```bash
# Stop all services
dotnet msbuild -t:DockerStop

# Clean Docker resources
dotnet msbuild -t:DockerClean
```

### Development Setup
```bash
# Setup local development environment
dotnet msbuild -t:DevSetup
```

## 🗄️ Database Configuration

The application connects to an existing SQL Server container:
- **Container Name**: `distracted_franklin`
- **Host**: `host.docker.internal`
- **Port**: `54426`
- **Database**: `test1`
- **Authentication**: SQL Server authentication

### Database Connection String
```
Server=host.docker.internal,54426;Database=test1;User Id=sa;Password=amjadOmar1!A;TrustServerCertificate=true;Encrypt=false;
```

## 🐳 Docker Configuration

### Application Container
- **Container Name**: `blogmvc-app-container`
- **Port**: `8080`
- **Health Checks**: Enabled with 30s intervals
- **Security**: Non-root user execution
- **Base Images**: 
  - Runtime: `mcr.microsoft.com/dotnet/aspnet:8.0`
  - Build: `mcr.microsoft.com/dotnet/sdk:8.0`

### Generated Files
The MSBuild system automatically generates:
- `Dockerfile` - Multi-stage build with security hardening
- `docker-compose.yml` - Container orchestration configuration

## ⚙️ MSBuild Configuration

### External Configuration
MSBuild deployment logic is externalized to `Build/Deploy.targets`:
- **Custom Tasks**: Dockerfile and docker-compose generation
- **Deployment Targets**: Full pipeline automation
- **Health Checks**: Application readiness validation
- **Resource Management**: Container lifecycle management

### Properties
Key configuration properties defined in `Deploy.targets`:
- `DockerImageName`: blogmvc-app
- `DockerContainerName`: blogmvc-app-container
- `DockerPort`: 8080
- `UseExistingDatabase`: true
- `ExistingDatabaseContainer`: distracted_franklin

## 🔧 Customization

### Modifying Configuration
1. Edit `Build/Deploy.targets` for deployment logic changes
2. Properties can be overridden via command line:
   ```bash
   dotnet msbuild -t:Deploy -p:DockerPort=9090
   ```

### Adding New Targets
Add custom targets to `Build/Deploy.targets`:
```xml
<Target Name="CustomTarget">
  <Message Text="Custom deployment step..." Importance="high" />
  <!-- Your custom logic here -->
</Target>
```

## 🏥 Health Checks

The deployment includes comprehensive health monitoring:
- **Application Health**: HTTP endpoint checks
- **Container Health**: Docker health check integration
- **Database Connectivity**: Connection validation
- **Resource Monitoring**: CPU and memory usage tracking

## 🌐 Access Points

After successful deployment:
- **Application**: http://localhost:8080
- **Health Status**: Included in deployment output
- **Logs**: Accessible via Docker commands

## 📊 Monitoring

### Container Status
```bash
# Quick status check
dotnet msbuild -t:DockerStatus

# Docker native commands
docker compose ps
docker stats --no-stream
```

### Application Logs
```bash
# MSBuild log viewing
dotnet msbuild -t:DockerLogs

# Docker native commands
docker compose logs -f blogmvc-app-container
```

## 🔒 Security Features

- **Non-root execution**: Application runs as `appuser`
- **Minimal attack surface**: Optimized base images
- **Health monitoring**: Automated health checks
- **Resource limits**: Configurable container constraints
- **Secure communication**: TLS-ready configuration

## 🚨 Troubleshooting

### Common Issues
1. **Port conflicts**: Change `DockerPort` property
2. **Database connection**: Verify existing SQL Server container
3. **Health check failures**: Check application startup logs
4. **Build failures**: Ensure .NET 8 SDK is installed

### Debug Commands
```bash
# Check container logs
dotnet msbuild -t:DockerLogs

# Inspect container status
dotnet msbuild -t:DockerStatus

# Verify database connectivity
docker exec blogmvc-app-container dotnet ef database update
```

## 📈 Performance

### Build Optimization
- Multi-stage Docker builds minimize image size
- Cached layers for faster subsequent builds
- Optimized restore and build processes

### Runtime Optimization
- Health check integration
- Resource monitoring
- Graceful shutdown handling
- Connection pooling for database access

---

**Built with ❤️ using .NET 8, Docker, and MSBuild automation**