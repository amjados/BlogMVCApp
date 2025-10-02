-- =============================================
-- BlogMVC Database Seed Data Script
-- Inserts essential lookup data for testing
-- =============================================

USE [BlogMVCTestDB]

-- =============================================
-- Seed Identity Roles
-- =============================================

PRINT 'Seeding Identity Roles...'

INSERT INTO
    [dbo].[AspNetRoles] (
        [Id],
        [Name],
        [NormalizedName],
        [ConcurrencyStamp]
    )
VALUES (
        '1',
        'Admin',
        'ADMIN',
        NEWID()
    ),
    (
        '2',
        'Author',
        'AUTHOR',
        NEWID()
    ),
    (
        '3',
        'Editor',
        'EDITOR',
        NEWID()
    ),
    ('4', 'User', 'USER', NEWID())

-- =============================================
-- Seed Categories (Lookup Table)
-- =============================================

PRINT 'Seeding Categories...'

SET IDENTITY_INSERT [dbo].[Categories] ON

INSERT INTO
    [dbo].[Categories] (
        [Id],
        [Name],
        [Description],
        [Slug],
        [CreatedAt],
        [UpdatedAt]
    )
VALUES (
        1,
        'Technology',
        'Articles about technology, software, and programming',
        'technology',
        GETUTCDATE(),
        GETUTCDATE()
    ),
    (
        2,
        'Lifestyle',
        'Personal stories, life experiences, and lifestyle tips',
        'lifestyle',
        GETUTCDATE(),
        GETUTCDATE()
    ),
    (
        3,
        'Programming',
        'Programming tutorials, code examples, and development tips',
        'programming',
        GETUTCDATE(),
        GETUTCDATE()
    ),
    (
        4,
        'Web Development',
        'Frontend, backend, and full-stack web development',
        'web-development',
        GETUTCDATE(),
        GETUTCDATE()
    ),
    (
        5,
        'News',
        'Latest news and updates in the tech world',
        'news',
        GETUTCDATE(),
        GETUTCDATE()
    ),
    (
        6,
        'Tutorials',
        'Step-by-step guides and how-to articles',
        'tutorials',
        GETUTCDATE(),
        GETUTCDATE()
    ),
    (
        7,
        'Reviews',
        'Product reviews, book reviews, and recommendations',
        'reviews',
        GETUTCDATE(),
        GETUTCDATE()
    ),
    (
        8,
        'DevOps',
        'DevOps practices, CI/CD, and infrastructure',
        'devops',
        GETUTCDATE(),
        GETUTCDATE()
    )

SET IDENTITY_INSERT [dbo].[Categories] OFF

-- =============================================
-- Seed Tags (Lookup Table)
-- =============================================

PRINT 'Seeding Tags...'

SET IDENTITY_INSERT [dbo].[Tags] ON

INSERT INTO
    [dbo].[Tags] (
        [Id],
        [Name],
        [Slug],
        [CreatedAt]
    )
VALUES (
        1,
        'ASP.NET Core',
        'aspnet-core',
        GETUTCDATE()
    ),
    (
        2,
        'C#',
        'csharp',
        GETUTCDATE()
    ),
    (
        3,
        'Entity Framework',
        'entity-framework',
        GETUTCDATE()
    ),
    (
        4,
        'Docker',
        'docker',
        GETUTCDATE()
    ),
    (
        5,
        'GitHub Actions',
        'github-actions',
        GETUTCDATE()
    ),
    (
        6,
        'SQL Server',
        'sql-server',
        GETUTCDATE()
    ),
    (
        7,
        'JavaScript',
        'javascript',
        GETUTCDATE()
    ),
    (
        8,
        'React',
        'react',
        GETUTCDATE()
    ),
    (
        9,
        'Vue.js',
        'vuejs',
        GETUTCDATE()
    ),
    (
        10,
        'Angular',
        'angular',
        GETUTCDATE()
    ),
    (
        11,
        'Bootstrap',
        'bootstrap',
        GETUTCDATE()
    ),
    (
        12,
        'CSS',
        'css',
        GETUTCDATE()
    ),
    (
        13,
        'HTML',
        'html',
        GETUTCDATE()
    ),
    (
        14,
        'Azure',
        'azure',
        GETUTCDATE()
    ),
    (
        15,
        'AWS',
        'aws',
        GETUTCDATE()
    ),
    (
        16,
        'Kubernetes',
        'kubernetes',
        GETUTCDATE()
    ),
    (
        17,
        'CI/CD',
        'cicd',
        GETUTCDATE()
    ),
    (
        18,
        'Testing',
        'testing',
        GETUTCDATE()
    ),
    (
        19,
        'Unit Testing',
        'unit-testing',
        GETUTCDATE()
    ),
    (
        20,
        'Integration Testing',
        'integration-testing',
        GETUTCDATE()
    )

SET IDENTITY_INSERT [dbo].[Tags] OFF

-- =============================================
-- Seed Test Users
-- =============================================

PRINT 'Seeding Test Users...'

-- Admin User
INSERT INTO
    [dbo].[AspNetUsers] (
        [Id],
        [UserName],
        [NormalizedUserName],
        [Email],
        [NormalizedEmail],
        [EmailConfirmed],
        [PasswordHash],
        [SecurityStamp],
        [ConcurrencyStamp],
        [PhoneNumberConfirmed],
        [TwoFactorEnabled],
        [LockoutEnabled],
        [AccessFailedCount],
        [FullName],
        [Bio],
        [CreatedAt],
        [UpdatedAt]
    )
VALUES (
        'admin-user-id-12345',
        'admin@blogmvc.com',
        'ADMIN@BLOGMVC.COM',
        'admin@blogmvc.com',
        'ADMIN@BLOGMVC.COM',
        1,
        'AQAAAAEAACcQAAAAEJ7+1qkF8mQY9sG2VlnJ8RJK7bF5s8K4pZ6X3yF7qN2mL9kW8vB5j4A7tX1uC0rE6g==', -- Password: Admin@123
        NEWID(),
        NEWID(),
        0,
        0,
        1,
        0,
        'System Administrator',
        'Main administrator of the BlogMVC application',
        GETUTCDATE(),
        GETUTCDATE()
    ),

-- Author User
(
    'author-user-id-12345',
    'author@blogmvc.com',
    'AUTHOR@BLOGMVC.COM',
    'author@blogmvc.com',
    'AUTHOR@BLOGMVC.COM',
    1,
    'AQAAAAEAACcQAAAAEK8+2qkF9mQY0sG3VlnJ9RJK8bF6s9K5pZ7X4yF8qN3mL0kW9vB6j5A8tX2uC1rE7h==', -- Password: Author@123
    NEWID(),
    NEWID(),
    0,
    0,
    1,
    0,
    'John Doe',
    'Senior software developer and technical writer',
    GETUTCDATE(),
    GETUTCDATE()
),

-- Editor User
(
    'editor-user-id-12345',
    'editor@blogmvc.com',
    'EDITOR@BLOGMVC.COM',
    'editor@blogmvc.com',
    'EDITOR@BLOGMVC.COM',
    1,
    'AQAAAAEAACcQAAAAEL9+3qkF0mQY1sG4VlnJ0RJK9bF7s0K6pZ8X5yF9qN4mL1kW0vB7j6A9tX3uC2rE8i==', -- Password: Editor@123
    NEWID(),
    NEWID(),
    0,
    0,
    1,
    0,
    'Jane Smith',
    'Content editor and blog manager',
    GETUTCDATE(),
    GETUTCDATE()
),

-- Regular User
(
    'user-user-id-123456',
    'user@blogmvc.com',
    'USER@BLOGMVC.COM',
    'user@blogmvc.com',
    'USER@BLOGMVC.COM',
    1,
    'AQAAAAEAACcQAAAAEM0+4qkF1mQY2sG5VlnJ1RJK0bF8s1K7pZ9X6yF0qN5mL2kW1vB8j7A0tX4uC3rE9j==', -- Password: User@123
    NEWID(),
    NEWID(),
    0,
    0,
    1,
    0,
    'Bob Wilson',
    'Regular blog reader and occasional commenter',
    GETUTCDATE(),
    GETUTCDATE()
)

-- =============================================
-- Assign Roles to Users
-- =============================================

PRINT 'Assigning Roles to Users...'

INSERT INTO
    [dbo].[AspNetUserRoles] ([UserId], [RoleId])
VALUES ('admin-user-id-12345', '1'), -- Admin role
    ('author-user-id-12345', '2'), -- Author role
    ('editor-user-id-12345', '3'), -- Editor role
    ('user-user-id-123456', '4') -- User role

-- =============================================
-- Seed Sample Blog Posts
-- =============================================

PRINT 'Seeding Sample Blog Posts...'

SET IDENTITY_INSERT [dbo].[Posts] ON

INSERT INTO
    [dbo].[Posts] (
        [Id],
        [Title],
        [Content],
        [Excerpt],
        [Slug],
        [Status],
        [MetaTitle],
        [MetaDescription],
        [ViewCount],
        [AuthorId],
        [CategoryId],
        [CreatedAt],
        [UpdatedAt],
        [PublishedAt]
    )
VALUES (
        1,
        'Welcome to BlogMVC',
        'Welcome to BlogMVC, a modern blogging platform built with ASP.NET Core 8! This application demonstrates best practices in web development including clean architecture, automated testing, and CI/CD deployment. 

Features include:
- User authentication and authorization
- Rich text editor for creating posts
- Category and tag management
- Comment system with moderation
- Admin dashboard
- Responsive design with Bootstrap
- Docker containerization
- GitHub Actions CI/CD

This is your first post! Feel free to edit or delete it and start creating your own content.',
        'Welcome to BlogMVC - a modern blogging platform built with ASP.NET Core 8 with features like authentication, rich editor, and CI/CD.',
        'welcome-to-blogmvc',
        'Published',
        'Welcome to BlogMVC - Modern Blogging Platform',
        'Get started with BlogMVC, a feature-rich blogging platform built with ASP.NET Core 8',
        156,
        'author-user-id-12345',
        1,
        DATEADD(day, -7, GETUTCDATE()),
        DATEADD(day, -7, GETUTCDATE()),
        DATEADD(day, -7, GETUTCDATE())
    ),
    (
        2,
        'Getting Started with ASP.NET Core Development',
        'ASP.NET Core is a cross-platform, high-performance framework for building modern web applications. In this comprehensive guide, we''ll explore the fundamentals of ASP.NET Core development.

## What is ASP.NET Core?

ASP.NET Core is a free, open-source web framework developed by Microsoft. It''s designed to build cloud-ready, internet-connected applications.

### Key Benefits:
1. **Cross-Platform**: Runs on Windows, Linux, and macOS
2. **High Performance**: One of the fastest web frameworks
3. **Cloud-Ready**: Built for scalability and deployment
4. **Open Source**: Community-driven development

## Setting Up Your Development Environment

To get started, you''ll need:
- .NET 8 SDK
- Visual Studio 2022 or VS Code
- SQL Server (or SQL Server LocalDB)

## Creating Your First Application

```bash
dotnet new webapp -n MyBlogApp
cd MyBlogApp
dotnet run
```

This tutorial will be expanded with more detailed examples and best practices.',
        'Learn the fundamentals of ASP.NET Core development with this comprehensive beginner''s guide covering setup, key concepts, and first application.',
        'getting-started-aspnet-core',
        'Published',
        'Getting Started with ASP.NET Core - Complete Guide',
        'Comprehensive guide to ASP.NET Core development for beginners and intermediate developers',
        342,
        'author-user-id-12345',
        3,
        DATEADD(day, -5, GETUTCDATE()),
        DATEADD(day, -5, GETUTCDATE()),
        DATEADD(day, -5, GETUTCDATE())
    ),
    (
        3,
        'Docker Containerization Best Practices',
        'Docker has revolutionized application deployment by providing consistent, portable environments. Here are essential best practices for containerizing your applications.

## Why Use Docker?

Docker containers provide:
- **Consistency**: Same environment across dev, test, and production
- **Portability**: Run anywhere Docker is supported
- **Scalability**: Easy horizontal scaling
- **Isolation**: Applications run in isolated environments

## Dockerfile Best Practices

### 1. Use Multi-Stage Builds
Multi-stage builds help reduce image size and improve security:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MyApp.dll"]
```

### 2. Use Specific Tags
Always use specific version tags instead of "latest":
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
```

### 3. Run as Non-Root User
Create and use a non-root user for security:
```dockerfile
RUN adduser --disabled-password --gecos '''' appuser
USER appuser
```

## Docker Compose for Development

Use Docker Compose to orchestrate multi-container applications:

```yaml
services:
  web:
    build: .
    ports:
      - "8080:80"
    depends_on:
      - db
  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourPassword123!
```

Stay tuned for more advanced Docker topics!',
        'Essential Docker containerization best practices including multi-stage builds, security considerations, and Docker Compose orchestration.',
        'docker-containerization-best-practices',
        'Published',
        'Docker Best Practices - Complete Guide',
        'Learn Docker containerization best practices for secure, efficient application deployment',
        278,
        'author-user-id-12345',
        8,
        DATEADD(day, -3, GETUTCDATE()),
        DATEADD(day, -3, GETUTCDATE()),
        DATEADD(day, -3, GETUTCDATE())
    ),
    (
        4,
        'Setting Up CI/CD with GitHub Actions',
        'Continuous Integration and Continuous Deployment (CI/CD) automate your development workflow. GitHub Actions provides a powerful, integrated solution.

## What is CI/CD?

CI/CD consists of:
- **Continuous Integration**: Automated building and testing
- **Continuous Deployment**: Automated deployment to production

## GitHub Actions Workflow

Here''s a basic workflow for .NET applications:

```yaml
name: CI/CD Pipeline

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v4
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: 8.0.x
    - name: Restore dependencies
      run: dotnet restore
    - name: Build
      run: dotnet build --no-restore
    - name: Test
      run: dotnet test --no-build --verbosity normal
```

## Benefits of Automation

1. **Faster Feedback**: Immediate notification of issues
2. **Consistency**: Same process every time
3. **Quality**: Automated testing catches bugs early
4. **Deployment**: Reduced manual errors

This is a draft post that will be expanded with more examples.',
        'Learn how to set up CI/CD pipelines with GitHub Actions for automated testing, building, and deployment of .NET applications.',
        'cicd-github-actions',
        'Draft',
        'CI/CD with GitHub Actions - Complete Setup Guide',
        'Step-by-step guide to implementing CI/CD pipelines with GitHub Actions for .NET applications',
        89,
        'author-user-id-12345',
        8,
        DATEADD(day, -1, GETUTCDATE()),
        DATEADD(day, -1, GETUTCDATE()),
        NULL
    )

SET IDENTITY_INSERT [dbo].[Posts] OFF

-- =============================================
-- Associate Posts with Tags
-- =============================================

PRINT 'Associating Posts with Tags...'

INSERT INTO
    [dbo].[PostTags] ([PostId], [TagId])
VALUES
    -- Welcome post tags
    (1, 1),
    (1, 2),
    (1, 3), -- ASP.NET Core, C#, Entity Framework
    (1, 4),
    (1, 18), -- Docker, Testing

-- ASP.NET Core tutorial tags
(2, 1),
(2, 2),
(2, 3), -- ASP.NET Core, C#, Entity Framework
(2, 6),
(2, 18),
(2, 19), -- SQL Server, Testing, Unit Testing

-- Docker best practices tags
(3, 4),
(3, 5),
(3, 16), -- Docker, GitHub Actions, Kubernetes
(3, 17), -- CI/CD

-- CI/CD with GitHub Actions tags
(4, 5),
(4, 17),
(4, 18), -- GitHub Actions, CI/CD, Testing
(4, 4) -- Docker

-- =============================================
-- Seed Sample Comments
-- =============================================

PRINT 'Seeding Sample Comments...'

SET IDENTITY_INSERT [dbo].[Comments] ON

INSERT INTO
    [dbo].[Comments] (
        [Id],
        [Content],
        [AuthorName],
        [AuthorEmail],
        [AuthorWebsite],
        [IsApproved],
        [PostId],
        [ParentCommentId],
        [CreatedAt]
    )
VALUES (
        1,
        'Great introduction to the platform! Looking forward to more tutorials.',
        'Alice Johnson',
        'alice.johnson@example.com',
        'https://alicecodes.com',
        1,
        1,
        NULL,
        DATEADD(day, -6, GETUTCDATE())
    ),
    (
        2,
        'Thanks Alice! More tutorials are coming soon. Any specific topics you''d like to see?',
        'John Doe',
        'author@blogmvc.com',
        NULL,
        1,
        1,
        1,
        DATEADD(day, -6, GETUTCDATE())
    ),
    (
        3,
        'This is exactly what I needed! The setup instructions are very clear.',
        'Mike Developer',
        'mike@devstudio.com',
        'https://devstudio.com',
        1,
        2,
        NULL,
        DATEADD(day, -4, GETUTCDATE())
    ),
    (
        4,
        'Docker has been a game-changer for our deployment process. These best practices will be very helpful.',
        'Sarah DevOps',
        'sarah@cloudtech.io',
        NULL,
        1,
        3,
        NULL,
        DATEADD(day, -2, GETUTCDATE())
    ),
    (
        5,
        'When will you cover Kubernetes deployment strategies?',
        'Tech Enthusiast',
        'enthusiast@techworld.com',
        NULL,
        0,
        3,
        4,
        DATEADD(day, -2, GETUTCDATE())
    )

SET IDENTITY_INSERT [dbo].[Comments] OFF

-- =============================================
-- Final Statistics
-- =============================================

PRINT '' PRINT '===== DATABASE SEEDING COMPLETED =====' PRINT ''

SELECT 'Roles' AS Entity, COUNT(*) AS Count
FROM AspNetRoles
UNION ALL
SELECT 'Users', COUNT(*)
FROM AspNetUsers
UNION ALL
SELECT 'Categories', COUNT(*)
FROM Categories
UNION ALL
SELECT 'Tags', COUNT(*)
FROM Tags
UNION ALL
SELECT 'Posts', COUNT(*)
FROM Posts
UNION ALL
SELECT 'Comments', COUNT(*)
FROM Comments
UNION ALL
SELECT 'Post-Tag Relations', COUNT(*)
FROM PostTags

PRINT 'Database seeding completed successfully!' PRINT 'Test data is ready for integration testing.'