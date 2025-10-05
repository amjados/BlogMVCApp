# GitHub Actions Workflows

This project uses GitHub Actions for automated CI/CD. Here are the configured workflows:

## 🔄 Active Workflows

### 1. **Tests Workflow** (`tests.yml`)
**Trigger**: Push to `main` branch
**Purpose**: Run unit tests on every commit to main
**Steps**:
- ✅ Checkout code
- ✅ Setup .NET 8
- ✅ Restore dependencies  
- ✅ Build solution
- ✅ Run all unit tests

### 2. **CI/CD Pipeline** (`ci-cd.yml`)
**Trigger**: Push to `main` or `develop`, Pull requests to `main`
**Purpose**: Complete build, test, security scan, and deployment pipeline
**Jobs**:
- 🧪 **Test**: Build and run tests with coverage
- 🚀 **Build and Publish**: Create production artifacts
- 🔒 **Security Scan**: Check for vulnerabilities
- 📝 **Code Quality**: SonarCloud analysis
- 🐳 **Docker Build**: Build container image
- 🚀 **Deploy Staging**: Deploy to staging environment

### 3. **Pull Request Validation** (`pr-validation.yml`)
**Trigger**: Pull requests to `main`
**Purpose**: Validate PRs before merging
**Steps**:
- ✅ Build validation
- ✅ Test execution
- ✅ Security checks
- ✅ Summary report

## 📊 Workflow Status

You can check the status of workflows here:
- [Tests Workflow](https://github.com/amjados/BlogMVCApp/actions/workflows/tests.yml)
- [CI/CD Pipeline](https://github.com/amjados/BlogMVCApp/actions/workflows/ci-cd.yml)
- [PR Validation](https://github.com/amjados/BlogMVCApp/actions/workflows/pr-validation.yml)

## 🔧 Configuration

### Required Secrets (for full CI/CD)
If you want to use the complete CI/CD pipeline, add these secrets to your repository:

1. **`SONAR_TOKEN`** - For SonarCloud code quality analysis
2. **Deployment secrets** - For staging/production deployment

### Workflow Files Location
```
.github/
└── workflows/
    ├── tests.yml           # Simple test runner
    ├── ci-cd.yml          # Complete CI/CD pipeline
    └── pr-validation.yml  # PR validation
```

## 🚀 Quick Start

1. **Push to main** - Triggers tests automatically
2. **Create PR** - Triggers PR validation
3. **View results** - Check Actions tab in GitHub

## 📈 Features

- ✅ **Automated Testing**: Every commit runs full test suite
- ✅ **Build Validation**: Ensures code compiles successfully  
- ✅ **Security Scanning**: Checks for vulnerable packages
- ✅ **Code Coverage**: Tracks test coverage metrics
- ✅ **Artifact Generation**: Creates deployable packages
- ✅ **Multi-Environment**: Support for staging and production
- ✅ **PR Protection**: Validates changes before merge

## 🔍 Test Coverage

The workflows run all test categories:
- **Unit Tests**: Controller, Model, Utility tests
- **Integration Tests**: Service integration and caching
- **Security Tests**: Vulnerability scanning

Total: **45+ tests** across the application