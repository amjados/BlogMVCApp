# MOQ Testing Examples from BlogMVCApp
## Complete Guide to MOQ Usage in Your Tests

### 🧪 **MOQ Framework Overview**
MOQ is a mocking framework that allows you to create fake implementations of interfaces and classes for testing purposes, ensuring proper isolation of the unit under test.

---

## 📋 **1. MOQ Setup Examples**

### **Basic Mock Creation**
```csharp
// Creating mocks for dependencies
private readonly Mock<ILogger<HomeController>> _mockLogger;
private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;

public HomeControllerTests()
{
    // Simple interface mock
    _mockLogger = new Mock<ILogger<HomeController>>();

    // Complex dependency mock with constructor parameters
    var userStore = new Mock<IUserStore<ApplicationUser>>();
    _mockUserManager = new Mock<UserManager<ApplicationUser>>(
        userStore.Object, null!, null!, null!, null!, null!, null!, null!, null!);

    // Mock with multiple dependencies
    var contextAccessor = new Mock<IHttpContextAccessor>();
    var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
    _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
        _mockUserManager.Object, contextAccessor.Object, claimsFactory.Object, 
        null!, null!, null!, null!);
}
```

### **Injecting Mocks into Controller**
```csharp
// Using .Object property to get the mock instance
_controller = new HomeController(
    _mockLogger.Object, 
    _mockSignInManager.Object, 
    _mockUserManager.Object, 
    _context
);
```

---

## 🎯 **2. MOQ Setup Patterns**

### **A. Method Return Value Setup**
```csharp
[Fact]
public async Task Login_Post_WithInvalidUser_ShouldReturnViewWithError()
{
    // Arrange - Setup mock to return null (user not found)
    _mockUserManager.Setup(x => x.FindByEmailAsync("test@example.com"))
        .ReturnsAsync((ApplicationUser?)null);

    // Act
    var result = await _controller.Login("test@example.com", "password", false, null);

    // Assert
    result.Should().BeOfType<ViewResult>();
    var viewResult = result as ViewResult;
    viewResult!.ViewData["Error"].Should().Be("Invalid email or password.");
}
```

### **B. Method with Object Return Setup**
```csharp
[Fact]
public async Task Login_Post_WithValidCredentials_ShouldRedirectToLandPage()
{
    // Arrange - Setup mock to return a real user object
    var user = new ApplicationUser { Email = "test@example.com" };
    
    _mockUserManager.Setup(x => x.FindByEmailAsync("test@example.com"))
        .ReturnsAsync(user);
    
    _mockSignInManager.Setup(x => x.PasswordSignInAsync(user, "password", false, false))
        .ReturnsAsync(SignInResult.Success);

    // Act
    var result = await _controller.Login("test@example.com", "password", false, null);

    // Assert
    result.Should().BeOfType<RedirectToActionResult>();
    var redirectResult = result as RedirectToActionResult;
    redirectResult!.ActionName.Should().Be("LandPage");
}
```

### **C. Different Return Values for Different Scenarios**
```csharp
[Fact]
public async Task Login_Post_WithLockedOutAccount_ShouldReturnViewWithError()
{
    // Arrange - Same user setup but different sign-in result
    var user = new ApplicationUser { Email = "test@example.com" };
    
    _mockUserManager.Setup(x => x.FindByEmailAsync("test@example.com"))
        .ReturnsAsync(user);
    
    // Different return value for sign-in
    _mockSignInManager.Setup(x => x.PasswordSignInAsync(user, "password", false, false))
        .ReturnsAsync(SignInResult.LockedOut);  // ← Different result

    // Act
    var result = await _controller.Login("test@example.com", "password", false, null);

    // Assert
    result.Should().BeOfType<ViewResult>();
    var viewResult = result as ViewResult;
    viewResult!.ViewData["Error"].Should().Be("Account is locked out.");
}
```

---

## ✅ **3. MOQ Verification Patterns**

### **A. Verify Method Was Called**
```csharp
[Fact]
public async Task Logout_ShouldSignOutAndRedirectToIndex()
{
    // Act
    var result = await _controller.Logout();

    // Assert - Verify the mock method was called exactly once
    _mockSignInManager.Verify(x => x.SignOutAsync(), Times.Once);
    
    result.Should().BeOfType<RedirectToActionResult>();
    var redirectResult = result as RedirectToActionResult;
    redirectResult!.ActionName.Should().Be("Index");
}
```

### **B. Verify with Different Times Options**
```csharp
// Verify method called exactly once
_mockSignInManager.Verify(x => x.SignOutAsync(), Times.Once);

// Verify method never called
_mockUserManager.Verify(x => x.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Never);

// Verify method called at least once
_mockLogger.Verify(x => x.LogError(It.IsAny<string>()), Times.AtLeastOnce);

// Verify method called exactly N times
_mockService.Verify(x => x.ProcessData(), Times.Exactly(3));
```

---

## 🎭 **4. MOQ Parameter Matching**

### **A. Exact Parameter Matching**
```csharp
// Setup expects exact string match
_mockUserManager.Setup(x => x.FindByEmailAsync("test@example.com"))
    .ReturnsAsync(user);
```

### **B. Any Parameter Matching**
```csharp
// Setup accepts any ClaimsPrincipal parameter
_mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
    .ReturnsAsync(user);
```

### **C. Complex Parameter Matching**
```csharp
// Example of matching specific parameter patterns
_mockUserManager.Setup(x => x.FindByEmailAsync(It.Is<string>(email => email.Contains("@"))))
    .ReturnsAsync(user);

// Match parameters with conditions
_mockService.Setup(x => x.ProcessUser(It.Is<ApplicationUser>(u => u.IsActive)))
    .Returns(true);
```

---

## 🔧 **5. Advanced MOQ Patterns**

### **A. Multiple Mock Setups in One Test**
```csharp
[Fact]
public async Task Login_Post_WithValidCredentialsAndReturnUrl_ShouldRedirectToReturnUrl()
{
    // Arrange - Multiple mock setups
    var user = new ApplicationUser { Email = "test@example.com" };
    var returnUrl = "/Dashboard";
    
    // Setup UserManager mock
    _mockUserManager.Setup(x => x.FindByEmailAsync("test@example.com"))
        .ReturnsAsync(user);
    
    // Setup SignInManager mock
    _mockSignInManager.Setup(x => x.PasswordSignInAsync(user, "password", false, false))
        .ReturnsAsync(SignInResult.Success);

    // Setup URL helper mock
    var mockUrlHelper = new Mock<IUrlHelper>();
    mockUrlHelper.Setup(x => x.IsLocalUrl(returnUrl)).Returns(true);
    _controller.Url = mockUrlHelper.Object;

    // Act
    var result = await _controller.Login("test@example.com", "password", false, returnUrl);

    // Assert
    result.Should().BeOfType<RedirectResult>();
    var redirectResult = result as RedirectResult;
    redirectResult!.Url.Should().Be(returnUrl);
}
```

### **B. Mock Setup with Complex Objects**
```csharp
[Fact]
public async Task ViewStats_ShouldReturnViewWithUserData()
{
    // Arrange - Create complex test object
    var user = new ApplicationUser
    {
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@example.com"
    };

    // Setup mock to return complex object
    _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
        .ReturnsAsync(user);

    // Act
    var result = await _controller.ViewStats();

    // Assert - Verify object properties are used correctly
    result.Should().BeOfType<ViewResult>();
    var viewResult = result as ViewResult;
    viewResult!.ViewData["UserName"].Should().Be("John Doe");  // Uses FullName property
    viewResult.ViewData["UserEmail"].Should().Be("john.doe@example.com");
}
```

---

## 📊 **6. MOQ Best Practices from Your Tests**

### **✅ Good Practices You're Using**

1. **Proper Mock Initialization**
```csharp
// ✅ Good: Initialize mocks in constructor
public HomeControllerTests()
{
    _mockLogger = new Mock<ILogger<HomeController>>();
    // ... other mocks
}
```

2. **Using .Object Property**
```csharp
// ✅ Good: Pass mock.Object to constructor
_controller = new HomeController(_mockLogger.Object, _mockSignInManager.Object, ...);
```

3. **Clear Arrange-Act-Assert Pattern**
```csharp
// ✅ Good: Clear test structure
[Fact]
public async Task Method_Scenario_ExpectedResult()
{
    // Arrange - Setup mocks and test data
    _mockService.Setup(x => x.Method()).ReturnsAsync(result);
    
    // Act - Call the method under test
    var result = await _controller.Method();
    
    // Assert - Verify results and mock calls
    result.Should().BeOfType<ViewResult>();
    _mockService.Verify(x => x.Method(), Times.Once);
}
```

4. **Descriptive Test Names**
```csharp
// ✅ Good: Test names describe scenario and expected outcome
Login_Post_WithInvalidUser_ShouldReturnViewWithError()
Login_Post_WithValidCredentials_ShouldRedirectToLandPage()
SavePost_WithEmptyTitle_ShouldRedirectWithError()
```

---

## 🚀 **7. MOQ Extensions for Your Filter Tests**

### **Example: Testing Filter Behavior with MOQ**
```csharp
[Fact]
public void ConfigurationBasedFilterProvider_ShouldApplyFiltersFromConfig()
{
    // Arrange
    var mockFilterManager = new Mock<FilterManager>();
    var mockServiceProvider = new Mock<IServiceProvider>();
    var mockLogger = new Mock<ILogger<ConfigurationBasedFilterProvider>>();
    
    var filterDefinitions = new List<FilterDefinition>
    {
        new FilterDefinition 
        { 
            FilterType = "RateLimit", 
            Order = 200, 
            Enabled = true 
        }
    };
    
    mockFilterManager.Setup(x => x.GetFiltersForAction("Home", "Index"))
        .Returns(filterDefinitions);
    
    var provider = new ConfigurationBasedFilterProvider(
        mockFilterManager.Object, 
        mockServiceProvider.Object, 
        mockLogger.Object
    );
    
    // Act & Assert - Test filter application logic
    // ... test implementation
}
```

---

## 📈 **Summary: MOQ Usage in Your Project**

### **Current MOQ Strengths** ✅
- **Complex Dependency Mocking**: UserManager, SignInManager with multiple constructor parameters
- **Async Method Mocking**: Proper use of `ReturnsAsync()` for async methods
- **Parameter Matching**: Both exact matching and `It.IsAny<T>()` patterns
- **Verification**: Using `Verify()` to ensure methods are called
- **Clean Test Structure**: Proper Arrange-Act-Assert pattern

### **MOQ Patterns You Use** 🎯
1. **Return Value Mocking**: `.ReturnsAsync()`, `.Returns()`
2. **Method Call Verification**: `.Verify(x => x.Method(), Times.Once)`
3. **Parameter Matching**: `It.IsAny<ClaimsPrincipal>()`
4. **Complex Object Mocking**: Creating and returning full entity objects
5. **Conditional Setup**: Different return values for different scenarios

Your MOQ usage demonstrates **professional-level testing practices** with proper isolation, clear test scenarios, and comprehensive verification of both return values and method calls.