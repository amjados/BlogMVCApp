using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using BlogMVCApp.Controllers;
using BlogMVCApp.Models;
using BlogMVCApp.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BlogMVCApp.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly ApplicationDbContext _context;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            _mockLogger = new Mock<ILogger<HomeController>>();

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                userStore.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                _mockUserManager.Object, contextAccessor.Object, claimsFactory.Object, null!, null!, null!, null!);

            _controller = new HomeController(_mockLogger.Object, _mockSignInManager.Object, _mockUserManager.Object, _context);

            // Setup controller context
            var context = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = context
            };

            // Setup TempData
            var tempDataProvider = new Mock<ITempDataProvider>();
            var tempDataDictionary = new TempDataDictionary(context, tempDataProvider.Object);
            _controller.TempData = tempDataDictionary;
        }

        [Fact]
        public async Task Index_ShouldReturnViewResult()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void Privacy_ShouldReturnViewResult()
        {
            // Act
            var result = _controller.Privacy();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void Login_Get_ShouldReturnViewWithReturnUrl()
        {
            // Arrange
            var returnUrl = "/Dashboard";

            // Act
            var result = _controller.Login(returnUrl);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult!.ViewData["ReturnUrl"].Should().Be(returnUrl);
        }

        [Fact]
        public async Task Login_Post_WithEmptyEmail_ShouldReturnViewWithError()
        {
            // Act
            var result = await _controller.Login("", "password", false, null);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult!.ViewData["Error"].Should().Be("Email and password are required.");
        }

        [Fact]
        public async Task Login_Post_WithEmptyPassword_ShouldReturnViewWithError()
        {
            // Act
            var result = await _controller.Login("test@example.com", "", false, null);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult!.ViewData["Error"].Should().Be("Email and password are required.");
        }

        [Fact]
        public async Task Login_Post_WithInvalidUser_ShouldReturnViewWithError()
        {
            // Arrange
            _mockUserManager.Setup(x => x.FindByEmailAsync("test@example.com"))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            var result = await _controller.Login("test@example.com", "password", false, null);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult!.ViewData["Error"].Should().Be("Invalid email or password.");
        }

        [Fact]
        public async Task Login_Post_WithValidCredentials_ShouldRedirectToLandPage()
        {
            // Arrange
            var user = new ApplicationUser { Email = "test@example.com" };
            _mockUserManager.Setup(x => x.FindByEmailAsync("test@example.com")).ReturnsAsync(user);
            _mockSignInManager.Setup(x => x.PasswordSignInAsync(user, "password", false, false))
                .ReturnsAsync(SignInResult.Success);

            // Act
            var result = await _controller.Login("test@example.com", "password", false, null);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult!.ActionName.Should().Be("LandPage");
        }

        [Fact]
        public async Task Login_Post_WithValidCredentialsAndReturnUrl_ShouldRedirectToReturnUrl()
        {
            // Arrange
            var user = new ApplicationUser { Email = "test@example.com" };
            var returnUrl = "/Dashboard";
            _mockUserManager.Setup(x => x.FindByEmailAsync("test@example.com")).ReturnsAsync(user);
            _mockSignInManager.Setup(x => x.PasswordSignInAsync(user, "password", false, false))
                .ReturnsAsync(SignInResult.Success);

            // Setup URL helper
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

        [Fact]
        public async Task Login_Post_WithLockedOutAccount_ShouldReturnViewWithError()
        {
            // Arrange
            var user = new ApplicationUser { Email = "test@example.com" };
            _mockUserManager.Setup(x => x.FindByEmailAsync("test@example.com")).ReturnsAsync(user);
            _mockSignInManager.Setup(x => x.PasswordSignInAsync(user, "password", false, false))
                .ReturnsAsync(SignInResult.LockedOut);

            // Act
            var result = await _controller.Login("test@example.com", "password", false, null);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult!.ViewData["Error"].Should().Be("Account is locked out.");
        }

        [Fact]
        public async Task Logout_ShouldSignOutAndRedirectToIndex()
        {
            // Act
            var result = await _controller.Logout();

            // Assert
            _mockSignInManager.Verify(x => x.SignOutAsync(), Times.Once);
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult!.ActionName.Should().Be("Index");
        }

        [Fact]
        public void LandPage_ShouldReturnViewResult()
        {
            // Act
            var result = _controller.LandPage();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task WritePost_ShouldReturnViewResult()
        {
            // Act
            var result = await _controller.WritePost();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task ViewStats_ShouldReturnViewWithUserData()
        {
            // Arrange
            var user = new ApplicationUser
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.ViewStats();

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult!.ViewData["UserName"].Should().Be("John Doe");
            viewResult.ViewData["UserEmail"].Should().Be("john.doe@example.com");
        }

        [Fact]
        public async Task SavePost_WithEmptyTitle_ShouldRedirectWithError()
        {
            // Set up a mock user
            var testUser = new ApplicationUser { Id = "test-user-id", UserName = "testuser", Email = "test@example.com" };
            _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(testUser);

            // Act
            var result = await _controller.SavePost("", "excerpt", "category", "tags",
                "content", "image", "draft", null!, "meta", "description", "slug");

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            _controller.TempData["Error"].Should().Be("Title and content are required.");
        }

        [Fact]
        public async Task SavePost_WithEmptyContent_ShouldRedirectWithError()
        {
            // Set up a mock user
            var testUser = new ApplicationUser { Id = "test-user-id", UserName = "testuser", Email = "test@example.com" };
            _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(testUser);

            // Act
            var result = await _controller.SavePost("title", "excerpt", "category", "tags",
                "", "image", "draft", null!, "meta", "description", "slug");

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            _controller.TempData["Error"].Should().Be("Title and content are required.");
        }

        [Fact]
        public async Task SavePost_WithValidData_ShouldRedirectWithSuccess()
        {
            // Arrange
            var user = new ApplicationUser { Email = "test@example.com" };
            _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.SavePost("Test Title", "excerpt", "category", "tags",
                "Test Content", "image", "published", null!, "meta", "description", "slug");

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            _controller.TempData["Success"].Should().Be("Post 'Test Title' has been published successfully!");
        }

        [Fact]
        public void PreviewPost_ShouldReturnViewWithData()
        {
            // Act
            var result = _controller.PreviewPost("Test Title", "Test Content", "Test Excerpt", "Technology");

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult!.ViewData["Title"].Should().Be("Test Title");
            viewResult.ViewData["Content"].Should().Be("Test Content");
            viewResult.ViewData["Excerpt"].Should().Be("Test Excerpt");
            viewResult.ViewData["Category"].Should().Be("Technology");
        }

        [Fact]
        public void PreviewPost_WithNullValues_ShouldUseDefaults()
        {
            // Act
            var result = _controller.PreviewPost(null!, null!, null!, null!);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult!.ViewData["Title"].Should().Be("Untitled Post");
            viewResult.ViewData["Content"].Should().Be("No content available.");
            viewResult.ViewData["Category"].Should().Be("Uncategorized");
        }

        [Fact]
        public async Task ViewAllPosts_ShouldReturnViewResult()
        {
            // Set up a mock user
            var testUser = new ApplicationUser { Id = "test-user-id", UserName = "testuser", Email = "test@example.com" };
            _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(testUser);

            // Act
            var result = await _controller.ViewAllPosts();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void Error_ShouldReturnViewWithErrorModel()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.TraceIdentifier = "test-trace-id";
            _controller.ControllerContext.HttpContext = context;

            // Act
            var result = _controller.Error();

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            var model = viewResult!.Model as ErrorViewModel;
            model!.RequestId.Should().Be("test-trace-id");
        }
    }
}