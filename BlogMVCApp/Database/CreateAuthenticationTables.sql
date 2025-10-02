-- ASP.NET Core Identity Tables
-- These are the standard tables used by ASP.NET Core Identity

-- Users table (extends AspNetUsers)
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(450) NOT NULL PRIMARY KEY,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset(7) NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    -- Extended properties for blog
    [FirstName] nvarchar(100) NULL,
    [LastName] nvarchar(100) NULL,
    [Biography] nvarchar(1000) NULL,
    [ProfileImageUrl] nvarchar(500) NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
    [IsActive] bit NOT NULL DEFAULT 1
);

-- Roles table
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] nvarchar(450) NOT NULL PRIMARY KEY,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [Description] nvarchar(500) NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE()
);

-- User Roles junction table
CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    [AssignedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
    [AssignedBy] nvarchar(450) NULL,
    PRIMARY KEY ([UserId], [RoleId]),
    FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
    FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    FOREIGN KEY ([AssignedBy]) REFERENCES [AspNetUsers] ([Id])
);

-- User Claims table
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

-- Role Claims table
CREATE TABLE [dbo].[AspNetRoleClaims] (
    [Id] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

-- User Logins table (for external providers like Google, Facebook)
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    PRIMARY KEY ([LoginProvider], [ProviderKey]),
    FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

-- User Tokens table (for password reset, email confirmation, etc.)
CREATE TABLE [dbo].[AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

-- Blog-specific tables that link to Users

-- Categories table
CREATE TABLE [dbo].[Categories] (
    [Id] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NULL,
    [Slug] nvarchar(100) NOT NULL UNIQUE,
    [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] nvarchar(450) NOT NULL,
    [IsActive] bit NOT NULL DEFAULT 1,
    FOREIGN KEY ([CreatedBy]) REFERENCES [AspNetUsers] ([Id])
);

-- Blog Posts table
CREATE TABLE [dbo].[Posts] (
    [Id] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Title] nvarchar(200) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [Excerpt] nvarchar(500) NULL,
    [Slug] nvarchar(200) NOT NULL UNIQUE,
    [FeaturedImageUrl] nvarchar(500) NULL,
    [CategoryId] int NULL,
    [AuthorId] nvarchar(450) NOT NULL,
    [Status] nvarchar(20) NOT NULL DEFAULT 'Draft', -- Draft, Published, Archived
    [PublishedAt] datetime2 NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
    [ViewCount] int NOT NULL DEFAULT 0,
    [MetaTitle] nvarchar(200) NULL,
    [MetaDescription] nvarchar(300) NULL,
    FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]),
    FOREIGN KEY ([AuthorId]) REFERENCES [AspNetUsers] ([Id])
);

-- Comments table
CREATE TABLE [dbo].[Comments] (
    [Id] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [PostId] int NOT NULL,
    [AuthorId] nvarchar(450) NULL, -- NULL for anonymous comments
    [AuthorName] nvarchar(100) NOT NULL, -- For anonymous or display name
    [AuthorEmail] nvarchar(256) NULL,
    [Content] nvarchar(1000) NOT NULL,
    [ParentCommentId] int NULL, -- For nested comments
    [IsApproved] bit NOT NULL DEFAULT 0,
    [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
    [IpAddress] nvarchar(45) NULL,
    FOREIGN KEY ([PostId]) REFERENCES [Posts] ([Id]) ON DELETE CASCADE,
    FOREIGN KEY ([AuthorId]) REFERENCES [AspNetUsers] ([Id]),
    FOREIGN KEY ([ParentCommentId]) REFERENCES [Comments] ([Id])
);

-- Tags table
CREATE TABLE [dbo].[Tags] (
    [Id] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] nvarchar(50) NOT NULL UNIQUE,
    [Slug] nvarchar(50) NOT NULL UNIQUE,
    [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE()
);

-- Post Tags junction table
CREATE TABLE [dbo].[PostTags] (
    [PostId] int NOT NULL,
    [TagId] int NOT NULL,
    PRIMARY KEY ([PostId], [TagId]),
    FOREIGN KEY ([PostId]) REFERENCES [Posts] ([Id]) ON DELETE CASCADE,
    FOREIGN KEY ([TagId]) REFERENCES [Tags] ([Id]) ON DELETE CASCADE
);

-- User Sessions table (optional - for session management)
CREATE TABLE [dbo].[UserSessions] (
    [Id] uniqueidentifier NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [UserId] nvarchar(450) NOT NULL,
    [SessionToken] nvarchar(500) NOT NULL UNIQUE,
    [DeviceInfo] nvarchar(500) NULL,
    [IpAddress] nvarchar(45) NULL,
    [UserAgent] nvarchar(1000) NULL,
    [IsActive] bit NOT NULL DEFAULT 1,
    [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
    [LastAccessedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
    [ExpiresAt] datetime2 NOT NULL,
    FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

-- Audit Log table (for tracking user actions)
CREATE TABLE [dbo].[AuditLogs] (
    [Id] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] nvarchar(450) NULL,
    [Action] nvarchar(100) NOT NULL,
    [EntityType] nvarchar(100) NULL,
    [EntityId] nvarchar(50) NULL,
    [OldValues] nvarchar(max) NULL,
    [NewValues] nvarchar(max) NULL,
    [IpAddress] nvarchar(45) NULL,
    [UserAgent] nvarchar(1000) NULL,
    [Timestamp] datetime2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
);

-- Create indexes for better performance
CREATE NONCLUSTERED INDEX [IX_AspNetUsers_NormalizedUserName] ON [AspNetUsers] ([NormalizedUserName]);
CREATE NONCLUSTERED INDEX [IX_AspNetUsers_NormalizedEmail] ON [AspNetUsers] ([NormalizedEmail]);
CREATE NONCLUSTERED INDEX [IX_AspNetRoles_NormalizedName] ON [AspNetRoles] ([NormalizedName]);
CREATE NONCLUSTERED INDEX [IX_Posts_AuthorId] ON [Posts] ([AuthorId]);
CREATE NONCLUSTERED INDEX [IX_Posts_CategoryId] ON [Posts] ([CategoryId]);
CREATE NONCLUSTERED INDEX [IX_Posts_Status] ON [Posts] ([Status]);
CREATE NONCLUSTERED INDEX [IX_Posts_PublishedAt] ON [Posts] ([PublishedAt]);
CREATE NONCLUSTERED INDEX [IX_Comments_PostId] ON [Comments] ([PostId]);
CREATE NONCLUSTERED INDEX [IX_Comments_AuthorId] ON [Comments] ([AuthorId]);

-- Insert default roles
INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [Description]) VALUES
(NEWID(), 'Admin', 'ADMIN', 'Full system administration access'),
(NEWID(), 'Editor', 'EDITOR', 'Can create and edit posts'),
(NEWID(), 'Author', 'AUTHOR', 'Can create own posts'),
(NEWID(), 'Subscriber', 'SUBSCRIBER', 'Can comment and subscribe'),
(NEWID(), 'Guest', 'GUEST', 'Basic read access');

-- Insert default categories
INSERT INTO [Categories] ([Name], [Description], [Slug], [CreatedBy])
SELECT 'General', 'General blog posts', 'general', u.[Id]
FROM [AspNetUsers] u
WHERE u.[NormalizedEmail] = 'ADMIN@EXAMPLE.COM';

INSERT INTO [Categories] ([Name], [Description], [Slug], [CreatedBy])
SELECT 'Technology', 'Technology-related posts', 'technology', u.[Id]
FROM [AspNetUsers] u
WHERE u.[NormalizedEmail] = 'ADMIN@EXAMPLE.COM';

INSERT INTO [Categories] ([Name], [Description], [Slug], [CreatedBy])
SELECT 'Tutorials', 'How-to guides and tutorials', 'tutorials', u.[Id]
FROM [AspNetUsers] u
WHERE u.[NormalizedEmail] = 'ADMIN@EXAMPLE.COM';