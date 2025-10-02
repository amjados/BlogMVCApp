-- =============================================
-- BlogMVC Database Schema Creation Script
-- Generated for GitHub Actions Testing
-- =============================================

USE [BlogMVCTestDB]

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

-- =============================================
-- Blog Application Tables
-- =============================================

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

-- =============================================
-- Create Indexes for Performance
-- =============================================

-- Identity Indexes
CREATE NONCLUSTERED
INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims] ([RoleId])

CREATE UNIQUE NONCLUSTERED
INDEX [RoleNameIndex] ON [dbo].[AspNetRoles] ([NormalizedName])
WHERE ([NormalizedName] IS NOT NULL)

CREATE NONCLUSTERED
INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims] ([UserId])

CREATE NONCLUSTERED
INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins] ([UserId])

CREATE NONCLUSTERED
INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles] ([RoleId])

CREATE NONCLUSTERED
INDEX [EmailIndex] ON [dbo].[AspNetUsers] ([NormalizedEmail])

CREATE UNIQUE NONCLUSTERED
INDEX [UserNameIndex] ON [dbo].[AspNetUsers] ([NormalizedUserName])
WHERE (
        [NormalizedUserName] IS NOT NULL
    )

-- Blog Application Indexes
CREATE NONCLUSTERED
INDEX [IX_Posts_AuthorId] ON [dbo].[Posts] ([AuthorId])

CREATE NONCLUSTERED
INDEX [IX_Posts_CategoryId] ON [dbo].[Posts] ([CategoryId])

CREATE NONCLUSTERED
INDEX [IX_Posts_Status] ON [dbo].[Posts] ([Status])

CREATE NONCLUSTERED
INDEX [IX_Posts_PublishedAt] ON [dbo].[Posts] ([PublishedAt])

CREATE NONCLUSTERED INDEX [IX_Posts_Slug] ON [dbo].[Posts] ([Slug])

CREATE NONCLUSTERED
INDEX [IX_Comments_PostId] ON [dbo].[Comments] ([PostId])

CREATE NONCLUSTERED
INDEX [IX_Comments_ParentCommentId] ON [dbo].[Comments] ([ParentCommentId])

CREATE NONCLUSTERED
INDEX [IX_Comments_IsApproved] ON [dbo].[Comments] ([IsApproved])

CREATE NONCLUSTERED
INDEX [IX_UserSessions_UserId] ON [dbo].[UserSessions] ([UserId])

CREATE NONCLUSTERED
INDEX [IX_UserSessions_SessionId] ON [dbo].[UserSessions] ([SessionId])

CREATE NONCLUSTERED
INDEX [IX_AuditLogs_UserId] ON [dbo].[AuditLogs] ([UserId])

CREATE NONCLUSTERED
INDEX [IX_AuditLogs_TableName] ON [dbo].[AuditLogs] ([TableName])

CREATE NONCLUSTERED
INDEX [IX_Categories_Slug] ON [dbo].[Categories] ([Slug])

CREATE NONCLUSTERED INDEX [IX_Tags_Slug] ON [dbo].[Tags] ([Slug])

PRINT 'Database schema created successfully!'