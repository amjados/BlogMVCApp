# BlogMVCApp - ASP.NET Core Blog Application

[![Build and Test](https://github.com/amjados/BlogMVCApp/actions/workflows/tests.yml/badge.svg)](https://github.com/amjados/BlogMVCApp/actions/workflows/tests.yml)
[![CI/CD Pipeline](https://github.com/amjados/BlogMVCApp/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/amjados/BlogMVCApp/actions/workflows/ci-cd.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-purple.svg)](https://docs.microsoft.com/aspnet/core)

A comprehensive blog application built with **ASP.NET Core 8.0** featuring advanced filter management, caching, rate limiting, and authentication.

## 📸 Screenshots

> **Coming Soon!** 🚧 Screenshots are being prepared to showcase the application's features.
> 
> To generate screenshots for this repository, follow the [Screenshots Guide](docs/SCREENSHOTS_GUIDE.md) or run:
> ```bash
> # Windows PowerShell
> .\scripts\take-screenshots.ps1
> 
> # Linux/Mac Bash
> ./scripts/take-screenshots.sh
> ```

<!-- 
Uncomment and use these sections once screenshots are captured:

### 🏠 Homepage
![Homepage](docs/screenshots/home/homepage.png)
*Clean, responsive homepage with navigation and welcome content*

### 📝 Blog Management
![Write Post](docs/screenshots/blog/write-post.png)
*Advanced blog post creation interface with rich editing capabilities*

### 👑 Admin Panel
![User Management](docs/screenshots/admin/user-management.png)
*Comprehensive user and role management with claims support*

### 🔧 Configuration-Based Filters
![API Response](docs/screenshots/features/api-response.png)
*JSON API responses showing filter configuration in action*

### 📱 Responsive Design
<img src="docs/screenshots/responsive/mobile-home.png" width="300" alt="Mobile Homepage">
*Fully responsive design that works beautifully on all devices*
-->

## 🚀 Features

### **Core Functionality**
- ✅ **Blog Management**: Create, edit, and manage blog posts
- ✅ **User Authentication**: ASP.NET Core Identity integration
- ✅ **Admin Panel**: User and role management
- ✅ **Comment System**: Nested comments with approval workflow
- ✅ **Category & Tag Management**: Organize content effectively
- ✅ **Caching System**: Multi-layer caching for performance

### **Advanced Features**
- 🎯 **Configuration-Based Filters**: Centralized filter management via `appsettings.json`
- 🔒 **Rate Limiting**: Configurable rate limiting per endpoint
- 📊 **Audit Logging**: Comprehensive action and change tracking
- 🎨 **Responsive Design**: Bootstrap-based responsive UI
- 🔍 **SEO Optimization**: Meta tags, structured URLs, and sitemaps

## 🏗️ Architecture

### **Technology Stack**
- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: Entity Framework Core with SQL Server
- **Authentication**: ASP.NET Core Identity
- **Caching**: In-Memory Cache with Redis-ready abstraction
- **Testing**: xUnit, MOQ, FluentAssertions
- **Frontend**: Bootstrap 5, jQuery, HTML5/CSS3

### **Project Structure**
```
BlogMVCApp/
├── Controllers/           # MVC Controllers
├── Models/               # Domain models and ViewModels
├── Views/                # Razor views and layouts
├── Data/                 # Entity Framework DbContext and entities
├── Services/             # Business logic services
├── Filters/              # Custom action filters
├── Configuration/        # Filter configuration management
├── Providers/            # Custom filter providers
├── wwwroot/             # Static files (CSS, JS, images)
└── Database/            # SQL scripts and migrations

BlogMVCApp.Tests/
├── Controllers/         # Controller unit tests
├── Models/              # Model tests
├── Integration/         # Integration tests
├── Utilities/           # Utility function tests
└── Data/               # Data layer tests
```

## ⚙️ Configuration-Based Filter System

This project features an **innovative configuration-based filter system** that allows centralized management of filters without code changes.

### **Supported Filters**
- **Rate Limiting**: Configurable request limits per endpoint
- **Caching**: Response caching with custom duration and vary rules
- **Authorization**: Role-based access control
- **Validation**: Model validation with custom error handling
- **Logging**: Action logging with performance metrics
- **Exception Handling**: Centralized exception management

### **Configuration Example**
```json
{
  "FilterOrder": {
    "DefaultOrder": {
      "AuthorizationOrder": 100,
      "RateLimitOrder": 200,
      "ValidationOrder": 300,
      "LoggingOrder": 400,
      "CachingOrder": 500
    },
    "ControllerFilters": {
      "Home": [
        {
          "FilterType": "ConfigurableRateLimit",
          "Order": 200,
          "Enabled": true,
          "Parameters": { "actionKey": "default" }
        }
      ]
    },
    "ActionFilters": {
      "Home.Login": [
        {
          "FilterType": "ConfigurableRateLimit",
          "Order": 150,
          "Parameters": { "actionKey": "Login" }
        }
      ]
    }
  }
}
```

## 🚀 Getting Started

### **Prerequisites**
- .NET 8.0 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### **Installation**

1. **Clone the repository**
```bash
git clone https://github.com/yourusername/BlogMVCApp.git
cd BlogMVCApp
```

2. **Restore dependencies**
```bash
dotnet restore
```

3. **Update database connection string**
Edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BlogMVCApp;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

4. **Run database migrations**
```bash
dotnet ef database update
```

5. **Build and run**
```bash
dotnet build
dotnet run
```

6. **Access the application**
- Navigate to `https://localhost:5001`
- Register a new account or use seeded admin credentials

## 🧪 Testing

### **Run All Tests**
```bash
dotnet test
```

### **Test Coverage**
The project includes comprehensive test coverage:
- **Unit Tests**: Controllers, Models, Utilities (~45 tests)
- **Integration Tests**: Service integration and caching
- **MOQ Framework**: Mocking ASP.NET Core Identity components
- **FluentAssertions**: Readable and expressive test assertions

### **Test Categories**
- ✅ **Controller Tests**: Authentication, authorization, CRUD operations
- ✅ **Model Tests**: Entity validation, computed properties, relationships
- ✅ **Integration Tests**: Caching, service interactions
- ✅ **Utility Tests**: URL slug generation, string manipulation

## 📊 Performance Features

### **Caching Strategy**
- **Response Caching**: Configurable per endpoint
- **Memory Caching**: Service-level caching for database queries
- **Cache Invalidation**: Smart cache invalidation on data changes
- **Cache Warming**: Preload frequently accessed data

### **Rate Limiting**
- **Per-User Limits**: Authenticated user rate limiting
- **IP-Based Limits**: Anonymous user protection
- **Endpoint-Specific**: Different limits per action
- **Configurable Windows**: Flexible time window configuration

## 🔧 Development

### **Key Design Patterns**
- **Repository Pattern**: Data access abstraction
- **Service Layer**: Business logic separation
- **Filter Pipeline**: Configurable request processing
- **Dependency Injection**: Loose coupling and testability

### **Code Quality**
- **Clean Architecture**: Separation of concerns
- **SOLID Principles**: Maintainable and extensible code
- **Comprehensive Testing**: High test coverage with quality assertions
- **Documentation**: Inline documentation and README guides

## 📝 Documentation

- **[Filter Configuration Guide](BlogMVCApp/README_FILTER_ORDER.md)**: Complete filter system documentation
- **[Best Practices](BlogMVCApp/README_BEST_PRACTICES.md)**: Development guidelines and patterns
- **[Test Coverage Report](BlogMVCApp.Tests/TEST_COVERAGE_REPORT.md)**: Testing strategy and coverage
- **[MOQ Examples](BlogMVCApp.Tests/MOQ_EXAMPLES_GUIDE.md)**: Testing patterns and examples

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **ASP.NET Core Team**: For the excellent framework
- **Entity Framework Core**: For robust ORM capabilities
- **xUnit & MOQ**: For comprehensive testing tools
- **Bootstrap**: For responsive UI components

## 📞 Contact

- **Project Link**: [https://github.com/yourusername/BlogMVCApp](https://github.com/yourusername/BlogMVCApp)
- **Issues**: [https://github.com/yourusername/BlogMVCApp/issues](https://github.com/yourusername/BlogMVCApp/issues)

---

**Built with ❤️ using ASP.NET Core 8.0**