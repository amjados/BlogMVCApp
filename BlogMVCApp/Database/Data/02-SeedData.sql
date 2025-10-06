-- =================================================================-- =============================================

-- BlogMVC Database Seed Data Script-- BlogMVC Database Seed Data Script

-- Generated from current database state-- Inserts essential lookup data for testing

-- Description: Complete seed data for all tables-- =============================================

-- =================================================================

USE [BlogMVCApp]

USE [BlogMVCApp];

-- =============================================
GO -- =============================================

-- Seed Identity Roles

-- =================================================================-- =============================================

-- ASP.NET Core Identity Seed Data

-- =================================================================PRINT 'Seeding Identity Roles...'

-- Insert RolesINSERT INTO

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Id = '1')    [dbo].[AspNetRoles] (

BEGIN        [Id],

    INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [CreatedAt], [Description])        [Name],

    VALUES ('1', 'Admin', 'ADMIN', '9C5811DD-6863-4852-B985-64209FB74103', '2025-10-05 11:12:38.8200000', 'Full administrative access to the BlogMVC application');        [NormalizedName],

END

[ConcurrencyStamp]

GO
)

VALUES (

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Id = '2')        '1',

BEGIN        'Admin',

    INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [CreatedAt], [Description])        'ADMIN',

    VALUES ('2', 'Author', 'AUTHOR', '95F9EF2A-231F-4266-AEE9-355C5444D121', '2025-10-05 11:12:38.8200000', 'Can create, edit, and publish blog posts');        NEWID()

END

),

GO
(

        '2',

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Id = '3')        'Author',

BEGIN        'AUTHOR',

    INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [CreatedAt], [Description])        NEWID()

    VALUES ('3', 'Editor', 'EDITOR', '51FF254C-0A30-442C-939A-CE9DE9A1CD12', '2025-10-05 11:12:38.8200000', 'Can review and edit posts from authors');    ),

END

(

GO
'3',
'Editor',
IF NOT EXISTS (
    SELECT 1
    FROM AspNetRoles
    WHERE
        Id = '4'
) 'EDITOR',

BEGIN        NEWID()

    INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [CreatedAt], [Description])    ),

    VALUES ('4', 'User', 'USER', '53FA0373-506C-48EC-B65C-9B16136AE5D5', '2025-10-05 11:12:38.8200000', 'Standard user with basic access to read and comment');    ('4', 'User', 'USER', NEWID())

END

-- =============================================
GO -- =============================================

-- Seed Categories (Lookup Table)

IF NOT EXISTS (
    SELECT 1
    FROM AspNetRoles
    WHERE
        Id = 'a97a4429-e07c-4a6b-8fc5-b3a623487b47'
) -- =============================================

BEGIN

    INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [CreatedAt], [Description])PRINT 'Seeding Categories...'

    VALUES ('a97a4429-e07c-4a6b-8fc5-b3a623487b47', 'Subscriber', 'SUBSCRIBER', NULL, '2025-10-05 11:13:44.0754799', 'Can comment and subscribe');

ENDSET IDENTITY_INSERT [dbo].[Categories] ON

INSERT INTO

-- Insert Users    [dbo].[Categories] (

IF NOT EXISTS (
    SELECT 1
    FROM AspNetUsers
    WHERE
        Id = 'admin-user-id-12345'
) [Id],

BEGIN        [Name],

    INSERT INTO [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FullName], [Bio], [ProfilePicture], [CreatedAt], [UpdatedAt], [FirstName], [LastName], [Biography], [ProfileImageUrl], [IsActive])        [Description],

    VALUES ('admin-user-id-12345', 'admin@blogmvc.com', 'ADMIN@BLOGMVC.COM', 'admin@blogmvc.com', 'ADMIN@BLOGMVC.COM', 1, 'AQAAAAEAACcQAAAAEKXmgJ8+yKWS1Z2QXmF0Qj/sJ2K/9mN1hQ3FvC2rP8Xk5zY7tR6sW8eB9nT3qA1M0L==', 'ADMIN-SECURITY-STAMP', 'ADMIN-CONCURRENCY-STAMP', NULL, 0, 0, NULL, 1, 0, 'System Administrator', 'Main administrator of the BlogMVC application', NULL, '2025-10-05 11:00:08.0100000', '2025-10-05 11:00:08.0100000', 'System', 'Administrator', 'Main administrator of the BlogMVC application', NULL, 1);        [Slug],

END

[CreatedAt],

GO
[UpdatedAt]

    )

IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE Id = 'author-user-id-12345')VALUES (

BEGIN        1,

    INSERT INTO [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FullName], [Bio], [ProfilePicture], [CreatedAt], [UpdatedAt], [FirstName], [LastName], [Biography], [ProfileImageUrl], [IsActive])        'Technology',

    VALUES ('author-user-id-12345', 'author@blogmvc.com', 'AUTHOR@BLOGMVC.COM', 'author@blogmvc.com', 'AUTHOR@BLOGMVC.COM', 1, 'AQAAAAEAACcQAAAAEKXmgJ8+yKWS1Z2QXmF0Qj/sJ2K/9mN1hQ3FvC2rP8Xk5zY7tR6sW8eB9nT3qA1M0L==', 'AUTHOR-SECURITY-STAMP', 'AUTHOR-CONCURRENCY-STAMP', NULL, 0, 0, NULL, 1, 0, 'John Doe', 'Senior software developer and technical writer', NULL, '2025-10-05 11:00:08.0100000', '2025-10-05 11:00:08.0100000', 'John', 'Doe', 'Senior software developer and technical writer', NULL, 1);        'Articles about technology, software, and programming',

END

'technology',

GO
GETUTCDATE(),

        GETUTCDATE()

IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE Id = 'editor-user-id-12345')    ),

BEGIN    (

    INSERT INTO [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FullName], [Bio], [ProfilePicture], [CreatedAt], [UpdatedAt], [FirstName], [LastName], [Biography], [ProfileImageUrl], [IsActive])        2,

    VALUES ('editor-user-id-12345', 'editor@blogmvc.com', 'EDITOR@BLOGMVC.COM', 'editor@blogmvc.com', 'EDITOR@BLOGMVC.COM', 1, 'AQAAAAEAACcQAAAAEKXmgJ8+yKWS1Z2QXmF0Qj/sJ2K/9mN1hQ3FvC2rP8Xk5zY7tR6sW8eB9nT3qA1M0L==', 'EDITOR-SECURITY-STAMP', 'EDITOR-CONCURRENCY-STAMP', NULL, 0, 0, NULL, 1, 0, 'Jane Smith', 'Content editor and blog manager', NULL, '2025-10-05 11:00:08.0100000', '2025-10-05 11:00:08.0100000', 'Jane', 'Smith', 'Content editor and blog manager', NULL, 1);        'Lifestyle',

END

'Personal stories, life experiences, and lifestyle tips',

GO
'lifestyle',
GETUTCDATE(),
IF NOT EXISTS (
    SELECT 1
    FROM AspNetUsers
    WHERE
        Id = 'user-user-id-123456'
) GETUTCDATE()

BEGIN    ),

    INSERT INTO [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FullName], [Bio], [ProfilePicture], [CreatedAt], [UpdatedAt], [FirstName], [LastName], [Biography], [ProfileImageUrl], [IsActive])    (

    VALUES ('user-user-id-123456', 'user@blogmvc.com', 'USER@BLOGMVC.COM', 'user@blogmvc.com', 'USER@BLOGMVC.COM', 1, 'AQAAAAEAACcQAAAAEKXmgJ8+yKWS1Z2QXmF0Qj/sJ2K/9mN1hQ3FvC2rP8Xk5zY7tR6sW8eB9nT3qA1M0L==', 'USER-SECURITY-STAMP', 'USER-CONCURRENCY-STAMP', NULL, 0, 0, NULL, 1, 0, 'Bob Wilson', 'Regular blog reader and occasional commenter', NULL, '2025-10-05 11:00:08.0100000', '2025-10-05 11:00:08.0100000', 'Bob', 'Wilson', 'Regular blog reader and occasional commenter', NULL, 1);        3,

END

'Programming',

GO
'Programming tutorials, code examples, and development tips',
'programming',

-- New users from actual database        GETUTCDATE(),

IF NOT EXISTS (
    SELECT 1
    FROM AspNetUsers
    WHERE
        Id = 'bed97bae-1caa-4740-8593-29930bcb5609'
) GETUTCDATE()

BEGIN    ),

    INSERT INTO [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FullName], [Bio], [ProfilePicture], [CreatedAt], [UpdatedAt], [FirstName], [LastName], [Biography], [ProfileImageUrl], [IsActive])    (

    VALUES ('bed97bae-1caa-4740-8593-29930bcb5609', 'jane.smith@blogmvc.com', 'JANE.SMITH@BLOGMVC.COM', 'jane.smith@blogmvc.com', 'JANE.SMITH@BLOGMVC.COM', 1, 'AQAAAAEAACcQAAAAEKXmgJ8+yKWS1Z2QXmF0Qj/sJ2K/9mN1hQ3FvC2rP8Xk5zY7tR6sW8eB9nT3qA1M0L==', 'JANE-SECURITY-STAMP', 'JANE-CONCURRENCY-STAMP', NULL, 0, 0, NULL, 1, 0, 'Jane Smith', 'Content editor specializing in technical writing', NULL, '2025-09-05 11:13:44.1698412', '2025-09-05 11:13:44.1698412', 'Jane', 'Smith', 'Content editor specializing in technical writing and developer tutorials.', NULL, 1);        4,

END

'Web Development',

GO
'Frontend, backend, and full-stack web development',
'web-development',
IF NOT EXISTS (
    SELECT 1
    FROM AspNetUsers
    WHERE
        Id = 'd5a48b65-fb2c-4f9d-9676-e9f64c91fa23'
) GETUTCDATE(),

BEGIN        GETUTCDATE()

    INSERT INTO [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FullName], [Bio], [ProfilePicture], [CreatedAt], [UpdatedAt], [FirstName], [LastName], [Biography], [ProfileImageUrl], [IsActive])    ),

    VALUES ('d5a48b65-fb2c-4f9d-9676-e9f64c91fa23', 'john.doe@blogmvc.com', 'JOHN.DOE@BLOGMVC.COM', 'john.doe@blogmvc.com', 'JOHN.DOE@BLOGMVC.COM', 1, 'AQAAAAEAACcQAAAAEKXmgJ8+yKWS1Z2QXmF0Qj/sJ2K/9mN1hQ3FvC2rP8Xk5zY7tR6sW8eB9nT3qA1M0L==', 'JOHNDOE-SECURITY-STAMP', 'JOHNDOE-CONCURRENCY-STAMP', NULL, 0, 0, NULL, 1, 0, 'John Doe', 'Passionate software developer and tech blogger', NULL, '2025-09-05 11:12:02.7329048', '2025-09-05 11:12:02.7329048', 'John', 'Doe', 'Passionate software developer and tech blogger with over 5 years of experience in .NET development.', NULL, 1);    (

END

5,

GO
'News', 'Latest news and updates in the tech world',

-- User Role Assignments        'news',

IF NOT EXISTS (
    SELECT 1
    FROM AspNetUserRoles
    WHERE
        UserId = 'admin-user-id-12345'
        AND RoleId = '1'
) GETUTCDATE(),

BEGIN        GETUTCDATE()

    INSERT INTO [AspNetUserRoles] ([UserId], [RoleId]) VALUES ('admin-user-id-12345', '1'); -- Admin role    ),

END

(

GO
6,
'Tutorials',
IF NOT EXISTS (
    SELECT 1
    FROM AspNetUserRoles
    WHERE
        UserId = 'author-user-id-12345'
        AND RoleId = '2'
) 'Step-by-step guides and how-to articles',

BEGIN        'tutorials',

    INSERT INTO [AspNetUserRoles] ([UserId], [RoleId]) VALUES ('author-user-id-12345', '2'); -- Author role        GETUTCDATE(),

END

GETUTCDATE()

GO
),

    (

IF NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId = 'editor-user-id-12345' AND RoleId = '3')        7,

BEGIN        'Reviews',

    INSERT INTO [AspNetUserRoles] ([UserId], [RoleId]) VALUES ('editor-user-id-12345', '3'); -- Editor role        'Product reviews, book reviews, and recommendations',

END

'reviews',

GO
GETUTCDATE(),

        GETUTCDATE()

IF NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId = 'user-user-id-123456' AND RoleId = '4')    ),

BEGIN    (

    INSERT INTO [AspNetUserRoles] ([UserId], [RoleId]) VALUES ('user-user-id-123456', '4'); -- User role        8,

END

'DevOps',

GO
'DevOps practices, CI/CD, and infrastructure', 'devops',

-- =================================================================        GETUTCDATE(),

-- Blog Application Seed Data        GETUTCDATE()

-- =================================================================    )

-- Insert CategoriesSET IDENTITY_INSERT [dbo].[Categories] OFF

SET IDENTITY_INSERT [Categories] ON;

-- =============================================
GO -- =============================================

-- Seed Tags (Lookup Table)

IF NOT EXISTS (
    SELECT 1
    FROM Categories
    WHERE
        Id = 1
) -- =============================================

BEGIN

    INSERT INTO [Categories] ([Id], [Name], [Description], [Slug], [CreatedAt], [UpdatedAt], [CreatedBy], [IsActive])PRINT 'Seeding Tags...'

    VALUES (1, 'Technology', 'Articles about technology, software, and programming', 'technology', '2025-10-05 11:00:08.0100000', '2025-10-05 11:00:08.0100000', 'admin-user-id-12345', 1);

ENDSET IDENTITY_INSERT [dbo].[Tags] ON



IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = 2)INSERT INTO

BEGIN    [dbo].[Tags] (

    INSERT INTO [Categories] ([Id], [Name], [Description], [Slug], [CreatedAt], [UpdatedAt], [CreatedBy], [IsActive])        [Id],

    VALUES (2, 'Lifestyle', 'Personal stories, life experiences, and lifestyle tips', 'lifestyle', '2025-10-05 11:00:08.0100000', '2025-10-05 11:00:08.0100000', 'admin-user-id-12345', 1);        [Name],

END        [Slug],

        [CreatedAt]

IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = 3)    )

BEGINVALUES (

    INSERT INTO [Categories] ([Id], [Name], [Description], [Slug], [CreatedAt], [UpdatedAt], [CreatedBy], [IsActive])        1,

    VALUES (3, 'Programming', 'Programming tutorials, code examples, and development tips', 'programming', '2025-10-05 11:00:08.0100000', '2025-10-05 11:00:08.0100000', 'admin-user-id-12345', 1);        'ASP.NET Core',

END

'aspnet-core',

        GETUTCDATE()

IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = 4)    ),

BEGIN    (

    INSERT INTO [Categories] ([Id], [Name], [Description], [Slug], [CreatedAt], [UpdatedAt], [CreatedBy], [IsActive])        2,

    VALUES (4, 'Web Development', 'Frontend, backend, and full-stack web development', 'web-development', '2025-10-05 11:00:08.0100000', '2025-10-05 11:00:08.0100000', 'admin-user-id-12345', 1);        'C#',

END

'csharp',

        GETUTCDATE()

IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = 5)    ),

BEGIN    (

    INSERT INTO [Categories] ([Id], [Name], [Description], [Slug], [CreatedAt], [UpdatedAt], [CreatedBy], [IsActive])        3,

    VALUES (5, 'News', 'Latest news and updates in the tech world', 'news', '2025-10-05 11:00:08.0100000', '2025-10-05 11:00:08.0100000', 'admin-user-id-12345', 1);        'Entity Framework',

END

'entity-framework',

        GETUTCDATE()

IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = 6)    ),

BEGIN    (

    INSERT INTO [Categories] ([Id], [Name], [Description], [Slug], [CreatedAt], [UpdatedAt], [CreatedBy], [IsActive])        4,

    VALUES (6, 'Tutorials', 'Step-by-step guides and how-to articles', 'tutorials', '2025-10-05 11:00:08.0100000', '2025-10-05 11:00:08.0100000', 'admin-user-id-12345', 1);        'Docker',

END

'docker',

        GETUTCDATE()

IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = 7)    ),

BEGIN    (

    INSERT INTO [Categories] ([Id], [Name], [Description], [Slug], [CreatedAt], [UpdatedAt], [CreatedBy], [IsActive])        5,

    VALUES (7, 'Reviews', 'Product reviews, book reviews, and recommendations', 'reviews', '2025-10-05 11:00:08.0100000', '2025-10-05 11:00:08.0100000', 'admin-user-id-12345', 1);        'GitHub Actions',

END

'github-actions',

        GETUTCDATE()

IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = 8)    ),

BEGIN    (

    INSERT INTO [Categories] ([Id], [Name], [Description], [Slug], [CreatedAt], [UpdatedAt], [CreatedBy], [IsActive])        6,

    VALUES (8, 'DevOps', 'DevOps practices, CI/CD, and infrastructure', 'devops', '2025-10-05 11:00:08.0100000', '2025-10-05 11:00:08.0100000', 'admin-user-id-12345', 1);        'SQL Server',

END

'sql-server', GETUTCDATE()

SET IDENTITY_INSERT [Categories] OFF;

),

GO
( 7,

-- Insert Tags        'JavaScript',

SET IDENTITY_INSERT [Tags] ON;

'javascript',

GO
GETUTCDATE()

    ),

INSERT INTO [Tags] ([Id], [Name], [Slug], [CreatedAt]) VALUES    (

(1, 'ASP.NET Core', 'aspnet-core', '2025-10-05 11:00:08.0100000'),        8,

(2, 'C#', 'csharp', '2025-10-05 11:00:08.0100000'),        'React',

(3, 'Entity Framework', 'entity-framework', '2025-10-05 11:00:08.0100000'),        'react',

(4, 'Docker', 'docker', '2025-10-05 11:00:08.0100000'),        GETUTCDATE()

(5, 'GitHub Actions', 'github-actions', '2025-10-05 11:00:08.0100000'),    ),

(6, 'SQL Server', 'sql-server', '2025-10-05 11:00:08.0100000'),    (

(7, 'JavaScript', 'javascript', '2025-10-05 11:00:08.0100000'),        9,

(8, 'React', 'react', '2025-10-05 11:00:08.0100000'),        'Vue.js',

(9, 'Vue.js', 'vuejs', '2025-10-05 11:00:08.0100000'),        'vuejs',

(10, 'Angular', 'angular', '2025-10-05 11:00:08.0100000'),        GETUTCDATE()

(11, 'Bootstrap', 'bootstrap', '2025-10-05 11:00:08.0100000'),    ),

(12, 'CSS', 'css', '2025-10-05 11:00:08.0100000'),    (

(13, 'HTML', 'html', '2025-10-05 11:00:08.0100000'),        10,

(14, 'Azure', 'azure', '2025-10-05 11:00:08.0100000'),        'Angular',

(15, 'AWS', 'aws', '2025-10-05 11:00:08.0100000'),        'angular',

(16, 'Kubernetes', 'kubernetes', '2025-10-05 11:00:08.0100000'),        GETUTCDATE()

(17, 'CI/CD', 'cicd', '2025-10-05 11:00:08.0100000'),    ),

(18, 'Testing', 'testing', '2025-10-05 11:00:08.0100000'),    (

(19, 'Unit Testing', 'unit-testing', '2025-10-05 11:00:08.0100000'),        11,

(20, 'Integration Testing', 'integration-testing', '2025-10-05 11:00:08.0100000');

'Bootstrap', 'bootstrap', SET IDENTITY_INSERT [Tags] OFF;

GETUTCDATE()

GO
), (

-- Insert Posts        12,

SET IDENTITY_INSERT [Posts] ON;

'CSS',

GO
'css',

        GETUTCDATE()

IF NOT EXISTS (SELECT 1 FROM Posts WHERE Id = 1)    ),

BEGIN    (

    INSERT INTO [Posts] ([Id], [Title], [Content], [Excerpt], [Slug], [Status], [FeaturedImage], [MetaTitle], [MetaDescription], [ViewCount], [AuthorId], [CategoryId], [CreatedAt], [UpdatedAt], [PublishedAt], [FeaturedImageUrl])        13,

    VALUES (1, 'Welcome to BlogMVC',         'HTML',

        'Welcome to BlogMVC, a modern blogging platform built with ASP.NET Core. This platform demonstrates the power of modern web development technologies including Entity Framework Core, Docker containerization, and automated CI/CD workflows.',        'html',

        'A comprehensive introduction to the BlogMVC platform and its features.',        GETUTCDATE()

        'welcome-to-blogmvc',    ),

        'Published',    (

        NULL,        14,

        'Welcome to BlogMVC - Modern Blogging Platform',        'Azure',

        'Discover BlogMVC, a cutting-edge blogging platform built with ASP.NET Core, Entity Framework, and modern web technologies.',        'azure',

        0,        GETUTCDATE()

        'author-user-id-12345',    ),

        1,    (

        '2025-09-28 11:00:08.0133333',        15,

        '2025-09-28 11:00:08.0133333',        'AWS',

        '2025-09-28 11:00:08.0133333',        'aws',

        NULL);        GETUTCDATE()

END

),

    (

IF NOT EXISTS (SELECT 1 FROM Posts WHERE Id = 2)        16,

BEGIN        'Kubernetes',

    INSERT INTO [Posts] ([Id], [Title], [Content], [Excerpt], [Slug], [Status], [FeaturedImage], [MetaTitle], [MetaDescription], [ViewCount], [AuthorId], [CategoryId], [CreatedAt], [UpdatedAt], [PublishedAt], [FeaturedImageUrl])        'kubernetes',

    VALUES (2, 'Getting Started with ASP.NET Core Development',        GETUTCDATE()

        'ASP.NET Core is a cross-platform, high-performance framework for building modern, cloud-enabled applications. In this comprehensive guide, we''ll explore the fundamentals of ASP.NET Core development and set up your first application.',    ),

        'Learn the basics of ASP.NET Core development and create your first application.',    (

        'getting-started-aspnet-core',        17,

        'Published',        'CI/CD',

        NULL,        'cicd',

        'ASP.NET Core Development Guide',        GETUTCDATE()

        'Complete beginner''s guide to ASP.NET Core development with practical examples and best practices.',    ),

        0,    (

        'author-user-id-12345',        18,

        3,        'Testing',

        '2025-09-30 11:00:08.0133333',        'testing',

        '2025-09-30 11:00:08.0133333',        GETUTCDATE()

        '2025-09-30 11:00:08.0133333',    ),

        NULL);    (

END

19,
'Unit Testing',
IF NOT EXISTS (
    SELECT 1
    FROM Posts
    WHERE
        Id = 3
) 'unit-testing',

BEGIN        GETUTCDATE()

    INSERT INTO [Posts] ([Id], [Title], [Content], [Excerpt], [Slug], [Status], [FeaturedImage], [MetaTitle], [MetaDescription], [ViewCount], [AuthorId], [CategoryId], [CreatedAt], [UpdatedAt], [PublishedAt], [FeaturedImageUrl])    ),

    VALUES (3, 'Docker Containerization Best Practices',    (

        'Docker has revolutionized application deployment and development workflows. This article covers essential best practices for containerizing your applications, optimizing Docker images, and implementing secure container strategies.',        20,

        'Master Docker containerization with these essential best practices and security guidelines.',        'Integration Testing',

        'docker-containerization-best-practices',        'integration-testing',

        'Published',        GETUTCDATE()

        NULL,    )

        'Docker Best Practices Guide',

        'Learn Docker containerization best practices, security guidelines, and optimization techniques for modern applications.',SET IDENTITY_INSERT [dbo].[Tags] OFF

        0,

        'author-user-id-12345',-- =============================================

        8,-- Seed Test Users

        '2025-10-02 11:00:08.0133333',-- =============================================

        '2025-10-02 11:00:08.0133333',

        '2025-10-02 11:00:08.0133333',PRINT 'Seeding Test Users...'

        NULL);

END-- Admin User


INSERT INTO

IF NOT EXISTS (SELECT 1 FROM Posts WHERE Id = 4)    [dbo].[AspNetUsers] (

BEGIN        [Id],

    INSERT INTO [Posts] ([Id], [Title], [Content], [Excerpt], [Slug], [Status], [FeaturedImage], [MetaTitle], [MetaDescription], [ViewCount], [AuthorId], [CategoryId], [CreatedAt], [UpdatedAt], [PublishedAt], [FeaturedImageUrl])        [UserName],

    VALUES (4, 'Setting Up CI/CD with GitHub Actions',        [NormalizedUserName],

        'Continuous Integration and Continuous Deployment (CI/CD) are essential practices in modern software development. This tutorial will guide you through setting up automated workflows using GitHub Actions for your ASP.NET Core applications.',        [Email],

        'Learn how to implement CI/CD pipelines using GitHub Actions for automated testing and deployment.',        [NormalizedEmail],

        'github-actions-cicd-setup',        [EmailConfirmed],

        'Draft',        [PasswordHash],

        NULL,        [SecurityStamp],

        'GitHub Actions CI/CD Tutorial',        [ConcurrencyStamp],

        'Step-by-step guide to implementing CI/CD pipelines with GitHub Actions for ASP.NET Core applications.',        [PhoneNumberConfirmed],

        0,        [TwoFactorEnabled],

        'author-user-id-12345',        [LockoutEnabled],

        8,        [AccessFailedCount],

        '2025-10-04 11:00:08.0133333',        [FullName],

        '2025-10-04 11:00:08.0133333',        [Bio],

        NULL,        [CreatedAt],

        NULL);        [UpdatedAt]

END

) VALUES ( SET IDENTITY_INSERT [Posts] OFF;

'admin-user-id-12345',

GO
'admin@blogmvc.com', 'ADMIN@BLOGMVC.COM',

-- Insert Post-Tag relationships        'admin@blogmvc.com',

IF NOT EXISTS (
    SELECT 1
    FROM PostTags
    WHERE
        PostId = 1
        AND TagId = 1
) 'ADMIN@BLOGMVC.COM',

BEGIN        1,

    INSERT INTO [PostTags] ([PostId], [TagId]) VALUES (1, 1); -- Welcome post - ASP.NET Core        'AQAAAAEAACcQAAAAEJ7+1qkF8mQY9sG2VlnJ8RJK7bF5s8K4pZ6X3yF7qN2mL9kW8vB5j4A7tX1uC0rE6g==', -- Password: Admin@123

    INSERT INTO [PostTags] ([PostId], [TagId]) VALUES (1, 2); -- Welcome post - C#        NEWID(),

    INSERT INTO [PostTags] ([PostId], [TagId]) VALUES (1, 3); -- Welcome post - Entity Framework        NEWID(),

END

0,
0,
IF NOT EXISTS (
    SELECT 1
    FROM PostTags
    WHERE
        PostId = 2
        AND TagId = 1
) 1,

BEGIN        0,

    INSERT INTO [PostTags] ([PostId], [TagId]) VALUES (2, 1); -- ASP.NET Core post - ASP.NET Core        'System Administrator',

    INSERT INTO [PostTags] ([PostId], [TagId]) VALUES (2, 2); -- ASP.NET Core post - C#        'Main administrator of the BlogMVC application',

    INSERT INTO [PostTags] ([PostId], [TagId]) VALUES (2, 18); -- ASP.NET Core post - Testing        GETUTCDATE(),

END

GETUTCDATE()

    ),

IF NOT EXISTS (SELECT 1 FROM PostTags WHERE PostId = 3 AND TagId = 4)

-- Author User
BEGIN-- Author User

    INSERT INTO [PostTags] ([PostId], [TagId]) VALUES (3, 4); -- Docker post - Docker(

    INSERT INTO [PostTags] ([PostId], [TagId]) VALUES (3, 17); -- Docker post - CI/CD    'author-user-id-12345',

    INSERT INTO [PostTags] ([PostId], [TagId]) VALUES (3, 16); -- Docker post - Kubernetes    'author@blogmvc.com',

END

'AUTHOR@BLOGMVC.COM',
'author@blogmvc.com',
IF NOT EXISTS (
    SELECT 1
    FROM PostTags
    WHERE
        PostId = 4
        AND TagId = 5
) 'AUTHOR@BLOGMVC.COM',

BEGIN    1,

    INSERT INTO [PostTags] ([PostId], [TagId]) VALUES (4, 5); -- GitHub Actions post - GitHub Actions    'AQAAAAEAACcQAAAAEK8+2qkF9mQY0sG3VlnJ9RJK8bF6s9K5pZ7X4yF8qN3mL0kW9vB6j5A8tX2uC1rE7h==', -- Password: Author@123

    INSERT INTO [PostTags] ([PostId], [TagId]) VALUES (4, 17); -- GitHub Actions post - CI/CD    NEWID(),

    INSERT INTO [PostTags] ([PostId], [TagId]) VALUES (4, 1); -- GitHub Actions post - ASP.NET Core    NEWID(),

END

0,

GO
0, 1, PRINT 'Database seed data inserted successfully.';

0,

GO
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