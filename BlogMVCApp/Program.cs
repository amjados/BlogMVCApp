using Microsoft.Extensions.Hosting; // <- brings IsStaging(), IsDevelopment(), IsProduction()
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BlogMVCApp.Data;
using BlogMVCApp.Models;
using BlogMVCApp.Services;
using BlogMVCApp.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Enhanced logging configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

if (builder.Environment.IsDevelopment())
{
    builder.Logging.AddDebug();
    builder.Logging.SetMinimumLevel(LogLevel.Debug);
}
else
{
    builder.Logging.SetMinimumLevel(LogLevel.Information);
}

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // Global filter registration with order
    options.Filters.Add<BlogMVCApp.Filters.OrderedLogActionAttribute>(order: 100);

    // Add configuration-based filter provider
    // This will be configured after services are built
});

// Add Memory Caching
builder.Services.AddMemoryCache();

// Add Response Caching
builder.Services.AddResponseCaching();

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

    // Sign in settings
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure application cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Home/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.ReturnUrlParameter = "returnUrl";
});

// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Filter settings
builder.Services.Configure<BlogMVCApp.Configuration.FilterConfiguration>(
    builder.Configuration.GetSection(BlogMVCApp.Configuration.FilterConfiguration.SectionName));

// Configure Filter Order settings
builder.Services.Configure<BlogMVCApp.Configuration.FilterOrderConfiguration>(
    builder.Configuration.GetSection(BlogMVCApp.Configuration.FilterOrderConfiguration.SectionName));

// Register Filter Manager
builder.Services.AddScoped<BlogMVCApp.Configuration.FilterManager>();

// Add Memory Cache for Action Filter caching
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 1024; // Set size limit for cache entries
});

// Register custom services
builder.Services.AddScoped<ICacheService, CacheService>();

// Register BlogService using decorator pattern for caching
builder.Services.AddScoped<BlogService>(); // Register concrete implementation first
builder.Services.AddScoped<IBlogService>(provider =>
{
    var concreteService = provider.GetRequiredService<BlogService>();
    var cacheService = provider.GetRequiredService<ICacheService>();
    var logger = provider.GetRequiredService<ILogger<CachedBlogService>>();
    return new CachedBlogService(concreteService, cacheService, logger);
});

builder.Services.AddScoped<ICacheWarmupService, CacheWarmupService>();

// Add background services
builder.Services.AddHostedService<RateLimitCleanupService>();

var app = builder.Build();

// MIDDLEWARE PIPELINE - Order is critical!

// 1. Security & Performance Monitoring (First)
app.UseMiddleware<SecurityLoggingMiddleware>();
app.UseMiddleware<PerformanceMonitoringMiddleware>();

// 2. Request/Response Logging (Before global exception handling)
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<RequestResponseLoggingMiddleware>();
}

// 3. Global Exception Handling (Before standard error handling)
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

// 4. Standard ASP.NET Core Error Handling
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment() || app.Environment.IsStaging()
    || app.Environment.IsEnvironment("Testing"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseResponseCaching();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Initialize database and seed data
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Seed data safely
        await DataSeeder.SeedAsync(context, userManager, roleManager, logger);

        // Warm up cache after seeding
        var cacheWarmupService = scope.ServiceProvider.GetRequiredService<ICacheWarmupService>();
        await cacheWarmupService.WarmupCacheAsync();

        logger.LogInformation("🚀 Application started successfully with database initialized, cache warmed up, and middleware pipeline configured.");
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "❌ An error occurred while initializing the database.");
        // Don't throw - let the application start even if seeding fails
    }
}

// Log application startup completion
var startupLogger = app.Services.GetRequiredService<ILogger<Program>>();
startupLogger.LogInformation("✅ BlogMVCApp startup completed with global middleware pipeline enabled");

app.Run();
