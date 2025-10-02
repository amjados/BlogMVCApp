-- =============================================
-- BlogMVC Complete Database Initialization Script
-- Creates database, schema, and seed data for testing
-- =============================================

-- Drop and recreate database for clean testing environment
USE [master]

-- Terminate existing connections
IF EXISTS (
    SELECT name
    FROM sys.databases
    WHERE
        name = N'BlogMVCTestDB'
) BEGIN ALTER
DATABASE [BlogMVCTestDB]
SET
    SINGLE_USER
WITH
    ROLLBACK IMMEDIATE DROP
DATABASE [BlogMVCTestDB] PRINT 'Dropped existing BlogMVCTestDB database' END

-- Create fresh database
CREATE DATABASE [BlogMVCTestDB]

PRINT 'Created BlogMVCTestDB database'

-- Switch to the new database
USE [BlogMVCTestDB]

-- =============================================
-- SCHEMA CREATION
-- =============================================

-- Drop existing tables if they exist (for clean setup)
IF OBJECT_ID('dbo.PostTags', 'U') IS NOT NULL
DROP TABLE [dbo].[PostTags] IF OBJECT_ID('dbo.Comments', 'U') IS NOT NULL
DROP TABLE [dbo].[Comments] IF OBJECT_ID('dbo.Posts', 'U') IS NOT NULL
DROP TABLE [dbo].[Posts] IF OBJECT_ID('dbo.AuditLogs', 'U') IS NOT NULL
DROP TABLE [dbo].[AuditLogs] IF OBJECT_ID('dbo.UserSessions', 'U') IS NOT NULL
DROP TABLE [dbo].[UserSessions] IF OBJECT_ID('dbo.Categories', 'U') IS NOT NULL
DROP TABLE [dbo].[Categories] IF OBJECT_ID('dbo.Tags', 'U') IS NOT NULL
DROP TABLE [dbo].[Tags] IF OBJECT_ID('dbo.AspNetUserTokens', 'U') IS NOT NULL
DROP TABLE [dbo].[AspNetUserTokens] IF OBJECT_ID('dbo.AspNetUserRoles', 'U') IS NOT NULL
DROP TABLE [dbo].[AspNetUserRoles] IF OBJECT_ID('dbo.AspNetUserLogins', 'U') IS NOT NULL
DROP TABLE [dbo].[AspNetUserLogins] IF OBJECT_ID('dbo.AspNetUserClaims', 'U') IS NOT NULL
DROP TABLE [dbo].[AspNetUserClaims] IF OBJECT_ID('dbo.AspNetRoleClaims', 'U') IS NOT NULL
DROP TABLE [dbo].[AspNetRoleClaims] IF OBJECT_ID('dbo.AspNetUsers', 'U') IS NOT NULL
DROP TABLE [dbo].[AspNetUsers] IF OBJECT_ID('dbo.AspNetRoles', 'U') IS NOT NULL
DROP TABLE [dbo].[AspNetRoles]

PRINT 'Creating database schema...'

-- =============================================
-- ASP.NET Core Identity Tables
-- =============================================

-- AspNetRoles Table
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] [nvarchar] (450) NOT NULL,
    [Name] [nvarchar] (256) NULL,
    [NormalizedName] [nvarchar] (256) NULL,
    [ConcurrencyStamp] [nvarchar] (max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
)

-- AspNetUsers Table
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] [nvarchar] (450) NOT NULL,
    [UserName] [nvarchar] (256) NULL,
    [NormalizedUserName] [nvarchar] (256) NULL,
    [Email] [nvarchar] (256) NULL,
    [NormalizedEmail] [nvarchar] (256) NULL,
    [EmailConfirmed] [bit] NOT NULL,
    [PasswordHash] [nvarchar] (max) NULL,
    [SecurityStamp] [nvarchar] (max) NULL,
    [ConcurrencyStamp] [nvarchar] (max) NULL,
    [PhoneNumber] [nvarchar] (max) NULL,
    [PhoneNumberConfirmed] [bit] NOT NULL,
    [TwoFactorEnabled] [bit] NOT NULL,
    [LockoutEnd] [datetimeoffset] (7) NULL,
    [LockoutEnabled] [bit] NOT NULL,
    [AccessFailedCount] [int] NOT NULL,
    [FullName] [nvarchar] (100) NULL,
    [Bio] [nvarchar] (500) NULL,
    [ProfilePicture] [nvarchar] (255) NULL,
    [CreatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),
    [UpdatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
)

-- AspNetRoleClaims Table
CREATE TABLE [dbo].[AspNetRoleClaims] (
    [Id] [int] IDENTITY(1, 1) NOT NULL,
    [RoleId] [nvarchar] (450) NOT NULL,
    [ClaimType] [nvarchar] (max) NULL,
    [ClaimValue] [nvarchar] (max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
)

-- AspNetUserClaims Table
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] [int] IDENTITY(1, 1) NOT NULL,
    [UserId] [nvarchar] (450) NOT NULL,
    [ClaimType] [nvarchar] (max) NULL,
    [ClaimValue] [nvarchar] (max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
)

-- AspNetUserLogins Table
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] [nvarchar] (450) NOT NULL,
    [ProviderKey] [nvarchar] (450) NOT NULL,
    [ProviderDisplayName] [nvarchar] (max) NULL,
    [UserId] [nvarchar] (450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED (
        [LoginProvider] ASC,
        [ProviderKey] ASC
    ),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
)

-- AspNetUserRoles Table
CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] [nvarchar] (450) NOT NULL,
    [RoleId] [nvarchar] (450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
)

-- AspNetUserTokens Table
CREATE TABLE [dbo].[AspNetUserTokens] (
    [UserId] [nvarchar] (450) NOT NULL,
    [LoginProvider] [nvarchar] (450) NOT NULL,
    [Name] [nvarchar] (450) NOT NULL,
    [Value] [nvarchar] (max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED (
        [UserId] ASC,
        [LoginProvider] ASC,
        [Name] ASC
    ),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
)

-- Categories Table
CREATE TABLE [dbo].[Categories] (
    [Id] [int] IDENTITY(1, 1) NOT NULL,
    [Name] [nvarchar] (100) NOT NULL,
    [Description] [nvarchar] (500) NULL,
    [Slug] [nvarchar] (100) NULL,
    [CreatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),
    [UpdatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([Id] ASC)
)

-- Tags Table
CREATE TABLE [dbo].[Tags] (
    [Id] [int] IDENTITY(1, 1) NOT NULL,
    [Name] [nvarchar] (50) NOT NULL,
    [Slug] [nvarchar] (50) NULL,
    [CreatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED ([Id] ASC)
)

-- Posts Table
CREATE TABLE [dbo].[Posts] (
    [Id] [int] IDENTITY(1, 1) NOT NULL,
    [Title] [nvarchar] (200) NOT NULL,
    [Content] [nvarchar] (max) NOT NULL,
    [Excerpt] [nvarchar] (500) NULL,
    [Slug] [nvarchar] (200) NULL,
    [Status] [nvarchar] (50) NOT NULL DEFAULT ('Draft'),
    [FeaturedImage] [nvarchar] (255) NULL,
    [MetaTitle] [nvarchar] (200) NULL,
    [MetaDescription] [nvarchar] (500) NULL,
    [ViewCount] [int] NOT NULL DEFAULT (0),
    [AuthorId] [nvarchar] (450) NULL,
    [CategoryId] [int] NULL,
    [CreatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),
    [UpdatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),
    [PublishedAt] [datetime2] (7) NULL,
    CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Posts_AspNetUsers_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Posts_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id])
)

-- PostTags Junction Table
CREATE TABLE [dbo].[PostTags] (
    [PostId] [int] NOT NULL,
    [TagId] [int] NOT NULL,
    CONSTRAINT [PK_PostTags] PRIMARY KEY CLUSTERED ([PostId] ASC, [TagId] ASC),
    CONSTRAINT [FK_PostTags_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Posts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PostTags_Tags_TagId] FOREIGN KEY ([TagId]) REFERENCES [dbo].[Tags] ([Id]) ON DELETE CASCADE
)

-- Comments Table
CREATE TABLE [dbo].[Comments] (
    [Id] [int] IDENTITY(1, 1) NOT NULL,
    [Content] [nvarchar] (1000) NOT NULL,
    [AuthorName] [nvarchar] (100) NOT NULL,
    [AuthorEmail] [nvarchar] (255) NOT NULL,
    [AuthorWebsite] [nvarchar] (255) NULL,
    [IsApproved] [bit] NOT NULL DEFAULT (0),
    [PostId] [int] NOT NULL,
    [ParentCommentId] [int] NULL,
    [CreatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Comments_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Posts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Comments_Comments_ParentCommentId] FOREIGN KEY ([ParentCommentId]) REFERENCES [dbo].[Comments] ([Id])
)

-- UserSessions Table
CREATE TABLE [dbo].[UserSessions] (
    [Id] [int] IDENTITY(1, 1) NOT NULL,
    [UserId] [nvarchar] (450) NOT NULL,
    [SessionId] [nvarchar] (450) NOT NULL,
    [IpAddress] [nvarchar] (45) NULL,
    [UserAgent] [nvarchar] (500) NULL,
    [CreatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),
    [LastAccessAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),
    [IsActive] [bit] NOT NULL DEFAULT (1),
    CONSTRAINT [PK_UserSessions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserSessions_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
)

-- AuditLogs Table
CREATE TABLE [dbo].[AuditLogs] (
    [Id] [int] IDENTITY(1, 1) NOT NULL,
    [TableName] [nvarchar] (100) NOT NULL,
    [RecordId] [nvarchar] (450) NOT NULL,
    [Action] [nvarchar] (50) NOT NULL,
    [OldValues] [nvarchar] (max) NULL,
    [NewValues] [nvarchar] (max) NULL,
    [UserId] [nvarchar] (450) NULL,
    [IpAddress] [nvarchar] (45) NULL,
    [UserAgent] [nvarchar] (500) NULL,
    [CreatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AuditLogs_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
)

-- Create Indexes
CREATE NONCLUSTERED
INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims] ([RoleId]) CREATE UNIQUE NONCLUSTERED
INDEX [RoleNameIndex] ON [dbo].[AspNetRoles] ([NormalizedName])
WHERE ([NormalizedName] IS NOT NULL) CREATE NONCLUSTERED
INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims] ([UserId]) CREATE NONCLUSTERED
INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins] ([UserId]) CREATE NONCLUSTERED
INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles] ([RoleId]) CREATE NONCLUSTERED
INDEX [EmailIndex] ON [dbo].[AspNetUsers] ([NormalizedEmail]) CREATE UNIQUE NONCLUSTERED
INDEX [UserNameIndex] ON [dbo].[AspNetUsers] ([NormalizedUserName])
WHERE (
        [NormalizedUserName] IS NOT NULL
    ) CREATE NONCLUSTERED
INDEX [IX_Posts_AuthorId] ON [dbo].[Posts] ([AuthorId]) CREATE NONCLUSTERED
INDEX [IX_Posts_CategoryId] ON [dbo].[Posts] ([CategoryId]) CREATE NONCLUSTERED
INDEX [IX_Posts_Status] ON [dbo].[Posts] ([Status]) CREATE NONCLUSTERED
INDEX [IX_Comments_PostId] ON [dbo].[Comments] ([PostId])

PRINT 'Database schema created successfully!'

-- =============================================
-- SEED DATA
-- =============================================

PRINT 'Seeding database with test data...'

-- Seed Identity Roles
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

-- Seed Categories
SET
IDENTITY_INSERT [dbo].[Categories] ON
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
        'DevOps',
        'DevOps practices, CI/CD, and infrastructure',
        'devops',
        GETUTCDATE(),
        GETUTCDATE()
    )
SET
IDENTITY_INSERT [dbo].[Categories] OFF

-- Seed Tags
SET
IDENTITY_INSERT [dbo].[Tags] ON
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
        'Testing',
        'testing',
        GETUTCDATE()
    )
SET
IDENTITY_INSERT [dbo].[Tags] OFF

-- Seed Test Users
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
        'AQAAAAEAACcQAAAAEJ7+1qkF8mQY9sG2VlnJ8RJK7bF5s8K4pZ6X3yF7qN2mL9kW8vB5j4A7tX1uC0rE6g==',
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
    (
        'author-user-id-12345',
        'author@blogmvc.com',
        'AUTHOR@BLOGMVC.COM',
        'author@blogmvc.com',
        'AUTHOR@BLOGMVC.COM',
        1,
        'AQAAAAEAACcQAAAAEK8+2qkF9mQY0sG3VlnJ9RJK8bF6s9K5pZ7X4yF8qN3mL0kW9vB6j5A8tX2uC1rE7h==',
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
    )

-- Assign Roles to Users
INSERT INTO
    [dbo].[AspNetUserRoles] ([UserId], [RoleId])
VALUES ('admin-user-id-12345', '1'),
    ('author-user-id-12345', '2')

-- Seed Sample Posts
SET
IDENTITY_INSERT [dbo].[Posts] ON
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
        'Welcome to BlogMVC, a modern blogging platform built with ASP.NET Core 8! This application demonstrates best practices in web development.',
        'Welcome to BlogMVC - a modern blogging platform built with ASP.NET Core 8',
        'welcome-to-blogmvc',
        'Published',
        'Welcome to BlogMVC',
        'Modern blogging platform with ASP.NET Core 8',
        100,
        'author-user-id-12345',
        1,
        GETUTCDATE(),
        GETUTCDATE(),
        GETUTCDATE()
    ),
    (
        2,
        'Getting Started with Docker',
        'Learn how to containerize your ASP.NET Core applications with Docker for consistent deployments.',
        'Complete guide to containerizing ASP.NET Core applications with Docker',
        'getting-started-docker',
        'Published',
        'Docker Guide',
        'Learn Docker containerization for ASP.NET Core',
        75,
        'author-user-id-12345',
        5,
        GETUTCDATE(),
        GETUTCDATE(),
        GETUTCDATE()
    )
SET
IDENTITY_INSERT [dbo].[Posts] OFF

-- Associate Posts with Tags
INSERT INTO
    [dbo].[PostTags] ([PostId], [TagId])
VALUES (1, 1),
    (1, 2),
    (1, 3), -- ASP.NET Core, C#, Entity Framework
    (2, 4),
    (2, 5),
    (2, 6) -- Docker, GitHub Actions, Testing

PRINT '' PRINT '===== DATABASE INITIALIZATION COMPLETED =====' PRINT ''

-- Display final statistics
SELECT 'Database' AS Entity, 'BlogMVCTestDB' AS Name, 'Ready' AS Status
UNION ALL
SELECT 'Roles', CAST(COUNT(*) AS VARCHAR(10)), 'Created'
FROM AspNetRoles
UNION ALL
SELECT 'Users', CAST(COUNT(*) AS VARCHAR(10)), 'Created'
FROM AspNetUsers
UNION ALL
SELECT 'Categories', CAST(COUNT(*) AS VARCHAR(10)), 'Created'
FROM Categories
UNION ALL
SELECT 'Tags', CAST(COUNT(*) AS VARCHAR(10)), 'Created'
FROM Tags
UNION ALL
SELECT 'Posts', CAST(COUNT(*) AS VARCHAR(10)), 'Created'
FROM Posts

PRINT 'BlogMVC test database is ready for GitHub Actions CI/CD!'