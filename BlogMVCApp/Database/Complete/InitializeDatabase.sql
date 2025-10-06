-- =================================================================
-- BlogMVC Complete Database Initialization Script
-- Generated from current database state
-- Description: Creates database, all tables, and seeds with current data
-- =================================================================

-- Create database if it doesn't exist
IF NOT EXISTS (
    SELECT name
    FROM sys.databases
    WHERE
        name = 'BlogMVCApp'
) BEGIN CREATE
DATABASE BlogMVCApp;

PRINT 'Database BlogMVCApp created successfully.';

END ELSE BEGIN PRINT 'Database BlogMVCApp already exists.';

END

USE [BlogMVCApp];
GO

-- =================================================================
-- ASP.NET Core Identity Tables
-- =================================================================

-- AspNetRoles
IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[AspNetRoles]')
        AND
    type in (N'U')
) BEGIN
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),
    [Description] nvarchar(500) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

END

-- AspNetUsers
IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[AspNetUsers]')
        AND
    type in (N'U')
) BEGIN
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
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
    [FullName] nvarchar(100) NULL,
    [Bio] nvarchar(500) NULL,
    [ProfilePicture] nvarchar(255) NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),
    [UpdatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),
    [FirstName] nvarchar(100) NULL,
    [LastName] nvarchar(100) NULL,
    [Biography] nvarchar(1000) NULL,
    [ProfileImageUrl] nvarchar(500) NULL,
    [IsActive] bit NOT NULL DEFAULT ((1)),
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

END

-- AspNetRoleClaims
IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[AspNetRoleClaims]')
        AND
    type in (N'U')
) BEGIN
CREATE TABLE [dbo].[AspNetRoleClaims] (
    [Id] int IDENTITY(1, 1) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id])
);

END

-- AspNetUserClaims
IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]')
        AND
    type in (N'U')
) BEGIN
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] int IDENTITY(1, 1) NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id])
);

END

-- AspNetUserLogins
IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]')
        AND
    type in (N'U')
) BEGIN
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY (
        [LoginProvider],
        [ProviderKey]
    )
);

END

-- AspNetUserRoles
IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]')
        AND
    type in (N'U')
) BEGIN
CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId])
);

END

-- AspNetUserTokens
IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[AspNetUserTokens]')
        AND
    type in (N'U')
) BEGIN
CREATE TABLE [dbo].[AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY (
        [UserId],
        [LoginProvider],
        [Name]
    )
);

END

-- =================================================================
-- Blog Application Tables
-- =================================================================

-- Categories
IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[Categories]')
        AND
    type in (N'U')
) BEGIN
CREATE TABLE [dbo].[Categories] (
    [Id] int IDENTITY(1, 1) NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NULL,
    [Slug] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),
    [UpdatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),
    [CreatedBy] nvarchar(450) NOT NULL DEFAULT ('admin-user-id-12345'),
    [IsActive] bit NOT NULL DEFAULT ((1)),
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
);

END

-- Posts
IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[Posts]')
        AND
    type in (N'U')
) BEGIN
CREATE TABLE [dbo].[Posts] (
    [Id] int IDENTITY(1, 1) NOT NULL,
    [Title] nvarchar(200) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [Excerpt] nvarchar(500) NULL,
    [Slug] nvarchar(200) NULL,
    [Status] nvarchar(50) NOT NULL DEFAULT ('Draft'),
    [FeaturedImage] nvarchar(255) NULL,
    [MetaTitle] nvarchar(200) NULL,
    [MetaDescription] nvarchar(500) NULL,
    [ViewCount] int NOT NULL DEFAULT ((0)),
    [AuthorId] nvarchar(450) NULL,
    [CategoryId] int NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),
    [UpdatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),
    [PublishedAt] datetime2 NULL,
    [FeaturedImageUrl] nvarchar(500) NULL,
    CONSTRAINT [PK_Posts] PRIMARY KEY ([Id])
);

END

-- Tags
IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[Tags]')
        AND
    type in (N'U')
) BEGIN
CREATE TABLE [dbo].[Tags] (
    [Id] int IDENTITY(1, 1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [Slug] nvarchar(50) NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),
    CONSTRAINT [PK_Tags] PRIMARY KEY ([Id])
);

END

-- PostTags (Many-to-Many relationship)
IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[PostTags]')
        AND
    type in (N'U')
) BEGIN
CREATE TABLE [dbo].[PostTags] (
    [PostId] int NOT NULL,
    [TagId] int NOT NULL,
    CONSTRAINT [PK_PostTags] PRIMARY KEY ([PostId], [TagId])
);

END

-- Comments
IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[Comments]')
        AND
    type in (N'U')
) BEGIN
CREATE TABLE [dbo].[Comments] (
    [Id] int IDENTITY(1, 1) NOT NULL,
    [Content] nvarchar(1000) NOT NULL,
    [AuthorName] nvarchar(100) NOT NULL,
    [AuthorEmail] nvarchar(255) NOT NULL,
    [AuthorWebsite] nvarchar(255) NULL,
    [IsApproved] bit NOT NULL DEFAULT ((0)),
    [PostId] int NOT NULL,
    [ParentCommentId] int NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),
    CONSTRAINT [PK_Comments] PRIMARY KEY ([Id])
);

END

-- =================================================================
-- System and Audit Tables
-- =================================================================

-- AuditLogs
IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[AuditLogs]')
        AND
    type in (N'U')
) BEGIN
CREATE TABLE [dbo].[AuditLogs] (
    [Id] int IDENTITY(1, 1) NOT NULL,
    [TableName] nvarchar(100) NOT NULL,
    [RecordId] nvarchar(450) NOT NULL,
    [Action] nvarchar(50) NOT NULL,
    [OldValues] nvarchar(max) NULL,
    [NewValues] nvarchar(max) NULL,
    [UserId] nvarchar(450) NULL,
    [IpAddress] nvarchar(45) NULL,
    [UserAgent] nvarchar(500) NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),
    CONSTRAINT [PK_AuditLogs] PRIMARY KEY ([Id])
);

END

-- UserSessions
IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[UserSessions]')
        AND
    type in (N'U')
) BEGIN
CREATE TABLE [dbo].[UserSessions] (
    [Id] int IDENTITY(1, 1) NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    [SessionId] nvarchar(450) NOT NULL,
    [IpAddress] nvarchar(45) NULL,
    [UserAgent] nvarchar(500) NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),
    [LastAccessAt] datetime2 NOT NULL DEFAULT (getutcdate()),
    [IsActive] bit NOT NULL DEFAULT ((1)),
    CONSTRAINT [PK_UserSessions] PRIMARY KEY ([Id])
);

END

-- =================================================================
-- Foreign Key Constraints
-- =================================================================

-- AspNetRoleClaims
IF NOT EXISTS (
    SELECT *
    FROM sys.foreign_keys
    WHERE
        object_id = OBJECT_ID(
            N'[dbo].[FK_AspNetRoleClaims_AspNetRoles_RoleId]'
        )
) BEGIN
ALTER TABLE [dbo].[AspNetRoleClaims]
ADD CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE;

END

-- AspNetUserClaims
IF NOT EXISTS (
    SELECT *
    FROM sys.foreign_keys
    WHERE
        object_id = OBJECT_ID(
            N'[dbo].[FK_AspNetUserClaims_AspNetUsers_UserId]'
        )
) BEGIN
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;

END

-- AspNetUserLogins
IF NOT EXISTS (
    SELECT *
    FROM sys.foreign_keys
    WHERE
        object_id = OBJECT_ID(
            N'[dbo].[FK_AspNetUserLogins_AspNetUsers_UserId]'
        )
) BEGIN
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;

END

-- AspNetUserRoles
IF NOT EXISTS (
    SELECT *
    FROM sys.foreign_keys
    WHERE
        object_id = OBJECT_ID(
            N'[dbo].[FK_AspNetUserRoles_AspNetRoles_RoleId]'
        )
) BEGIN
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE;

END

IF NOT EXISTS (
    SELECT *
    FROM sys.foreign_keys
    WHERE
        object_id = OBJECT_ID(
            N'[dbo].[FK_AspNetUserRoles_AspNetUsers_UserId]'
        )
) BEGIN
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;

END

-- AspNetUserTokens
IF NOT EXISTS (
    SELECT *
    FROM sys.foreign_keys
    WHERE
        object_id = OBJECT_ID(
            N'[dbo].[FK_AspNetUserTokens_AspNetUsers_UserId]'
        )
) BEGIN
ALTER TABLE [dbo].[AspNetUserTokens]
ADD CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;

END

-- Posts foreign keys
IF NOT EXISTS (
    SELECT *
    FROM sys.foreign_keys
    WHERE
        object_id = OBJECT_ID(
            N'[dbo].[FK_Posts_AspNetUsers_AuthorId]'
        )
) BEGIN
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [FK_Posts_AspNetUsers_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[AspNetUsers] ([Id]);

END

IF NOT EXISTS (
    SELECT *
    FROM sys.foreign_keys
    WHERE
        object_id = OBJECT_ID(
            N'[dbo].[FK_Posts_Categories_CategoryId]'
        )
) BEGIN
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [FK_Posts_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id]);

END

-- PostTags foreign keys
IF NOT EXISTS (
    SELECT *
    FROM sys.foreign_keys
    WHERE
        object_id = OBJECT_ID(
            N'[dbo].[FK_PostTags_Posts_PostId]'
        )
) BEGIN
ALTER TABLE [dbo].[PostTags]
ADD CONSTRAINT [FK_PostTags_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Posts] ([Id]) ON DELETE CASCADE;

END

IF NOT EXISTS (
    SELECT *
    FROM sys.foreign_keys
    WHERE
        object_id = OBJECT_ID(
            N'[dbo].[FK_PostTags_Tags_TagId]'
        )
) BEGIN
ALTER TABLE [dbo].[PostTags]
ADD CONSTRAINT [FK_PostTags_Tags_TagId] FOREIGN KEY ([TagId]) REFERENCES [dbo].[Tags] ([Id]) ON DELETE CASCADE;

END

-- Comments foreign keys
IF NOT EXISTS (
    SELECT *
    FROM sys.foreign_keys
    WHERE
        object_id = OBJECT_ID(
            N'[dbo].[FK_Comments_Posts_PostId]'
        )
) BEGIN
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_Comments_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Posts] ([Id]) ON DELETE CASCADE;

END

IF NOT EXISTS (
    SELECT *
    FROM sys.foreign_keys
    WHERE
        object_id = OBJECT_ID(
            N'[dbo].[FK_Comments_Comments_ParentCommentId]'
        )
) BEGIN
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_Comments_Comments_ParentCommentId] FOREIGN KEY ([ParentCommentId]) REFERENCES [dbo].[Comments] ([Id]);

END

-- UserSessions foreign key
IF NOT EXISTS (
    SELECT *
    FROM sys.foreign_keys
    WHERE
        object_id = OBJECT_ID(
            N'[dbo].[FK_UserSessions_AspNetUsers_UserId]'
        )
) BEGIN
ALTER TABLE [dbo].[UserSessions]
ADD CONSTRAINT [FK_UserSessions_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;

END

-- =================================================================
-- Indexes for Performance
-- =================================================================

-- AspNetRoles indexes
IF NOT EXISTS (
    SELECT *
    FROM sys.indexes
    WHERE
        object_id = OBJECT_ID(N'[dbo].[AspNetRoles]')
        AND name = N'RoleNameIndex'
) BEGIN CREATE UNIQUE NONCLUSTERED
INDEX [RoleNameIndex] ON [dbo].[AspNetRoles] ([NormalizedName])
WHERE ([NormalizedName] IS NOT NULL);

END

-- AspNetUsers indexes
IF NOT EXISTS (
    SELECT *
    FROM sys.indexes
    WHERE
        object_id = OBJECT_ID(N'[dbo].[AspNetUsers]')
        AND name = N'EmailIndex'
) BEGIN CREATE NONCLUSTERED
INDEX [EmailIndex] ON [dbo].[AspNetUsers] ([NormalizedEmail]);

END

IF NOT EXISTS (
    SELECT *
    FROM sys.indexes
    WHERE
        object_id = OBJECT_ID(N'[dbo].[AspNetUsers]')
        AND name = N'UserNameIndex'
) BEGIN CREATE UNIQUE NONCLUSTERED
INDEX [UserNameIndex] ON [dbo].[AspNetUsers] ([NormalizedUserName])
WHERE (
        [NormalizedUserName] IS NOT NULL
    );

END

-- Blog table indexes
IF NOT EXISTS (
    SELECT *
    FROM sys.indexes
    WHERE
        object_id = OBJECT_ID(N'[dbo].[Posts]')
        AND name = N'IX_Posts_AuthorId'
) BEGIN CREATE NONCLUSTERED
INDEX [IX_Posts_AuthorId] ON [dbo].[Posts] ([AuthorId]);

END

IF NOT EXISTS (
    SELECT *
    FROM sys.indexes
    WHERE
        object_id = OBJECT_ID(N'[dbo].[Posts]')
        AND name = N'IX_Posts_CategoryId'
) BEGIN CREATE NONCLUSTERED
INDEX [IX_Posts_CategoryId] ON [dbo].[Posts] ([CategoryId]);

END

IF NOT EXISTS (
    SELECT *
    FROM sys.indexes
    WHERE
        object_id = OBJECT_ID(N'[dbo].[Posts]')
        AND name = N'IX_Posts_Status_PublishedAt'
) BEGIN CREATE NONCLUSTERED
INDEX [IX_Posts_Status_PublishedAt] ON [dbo].[Posts] ([Status], [PublishedAt] DESC);

END

IF NOT EXISTS (
    SELECT *
    FROM sys.indexes
    WHERE
        object_id = OBJECT_ID(N'[dbo].[Posts]')
        AND name = N'IX_Posts_Slug'
) BEGIN CREATE UNIQUE NONCLUSTERED
INDEX [IX_Posts_Slug] ON [dbo].[Posts] ([Slug])
WHERE ([Slug] IS NOT NULL);

END

IF NOT EXISTS (
    SELECT *
    FROM sys.indexes
    WHERE
        object_id = OBJECT_ID(N'[dbo].[Comments]')
        AND name = N'IX_Comments_PostId'
) BEGIN CREATE NONCLUSTERED
INDEX [IX_Comments_PostId] ON [dbo].[Comments] ([PostId]);

END

IF NOT EXISTS (
    SELECT *
    FROM sys.indexes
    WHERE
        object_id = OBJECT_ID(N'[dbo].[Comments]')
        AND name = N'IX_Comments_ParentCommentId'
) BEGIN CREATE NONCLUSTERED
INDEX [IX_Comments_ParentCommentId] ON [dbo].[Comments] ([ParentCommentId]);

END

IF NOT EXISTS (
    SELECT *
    FROM sys.indexes
    WHERE
        object_id = OBJECT_ID(N'[dbo].[Categories]')
        AND name = N'IX_Categories_Slug'
) BEGIN CREATE UNIQUE NONCLUSTERED
INDEX [IX_Categories_Slug] ON [dbo].[Categories] ([Slug])
WHERE ([Slug] IS NOT NULL);

END

IF NOT EXISTS (
    SELECT *
    FROM sys.indexes
    WHERE
        object_id = OBJECT_ID(N'[dbo].[Tags]')
        AND name = N'IX_Tags_Name'
) BEGIN CREATE UNIQUE NONCLUSTERED
INDEX [IX_Tags_Name] ON [dbo].[Tags] ([Name]);

END

IF NOT EXISTS (
    SELECT *
    FROM sys.indexes
    WHERE
        object_id = OBJECT_ID(N'[dbo].[Tags]')
        AND name = N'IX_Tags_Slug'
) BEGIN CREATE UNIQUE NONCLUSTERED
INDEX [IX_Tags_Slug] ON [dbo].[Tags] ([Slug])
WHERE ([Slug] IS NOT NULL);

END

-- UserSessions indexes
IF NOT EXISTS (
    SELECT *
    FROM sys.indexes
    WHERE
        object_id = OBJECT_ID(N'[dbo].[UserSessions]')
        AND name = N'IX_UserSessions_UserId'
) BEGIN CREATE NONCLUSTERED
INDEX [IX_UserSessions_UserId] ON [dbo].[UserSessions] ([UserId]);

END

IF NOT EXISTS (
    SELECT *
    FROM sys.indexes
    WHERE
        object_id = OBJECT_ID(N'[dbo].[UserSessions]')
        AND name = N'IX_UserSessions_SessionId'
) BEGIN CREATE NONCLUSTERED
INDEX [IX_UserSessions_SessionId] ON [dbo].[UserSessions] ([SessionId]);

END

-- AuditLogs indexes
IF NOT EXISTS (
    SELECT *
    FROM sys.indexes
    WHERE
        object_id = OBJECT_ID(N'[dbo].[AuditLogs]')
        AND name = N'IX_AuditLogs_TableName_RecordId'
) BEGIN CREATE NONCLUSTERED
INDEX [IX_AuditLogs_TableName_RecordId] ON [dbo].[AuditLogs] ([TableName], [RecordId]);

END

IF NOT EXISTS (
    SELECT *
    FROM sys.indexes
    WHERE
        object_id = OBJECT_ID(N'[dbo].[AuditLogs]')
        AND name = N'IX_AuditLogs_CreatedAt'
) BEGIN CREATE NONCLUSTERED
INDEX [IX_AuditLogs_CreatedAt] ON [dbo].[AuditLogs] ([CreatedAt] DESC);

END

-- =================================================================
-- Seed Data
-- =================================================================

-- Insert Roles
IF NOT EXISTS (
    SELECT 1
    FROM AspNetRoles
    WHERE
        Id = '1'
) BEGIN
INSERT INTO
    [AspNetRoles] (
        [Id],
        [Name],
        [NormalizedName],
        [ConcurrencyStamp],
        [CreatedAt],
        [Description]
    )
VALUES (
        '1',
        'Admin',
        'ADMIN',
        '9C5811DD-6863-4852-B985-64209FB74103',
        '2025-10-05 11:12:38.8200000',
        'Full administrative access to the BlogMVC application'
    );

END IF NOT EXISTS (
    SELECT 1
    FROM AspNetRoles
    WHERE
        Id = '2'
) BEGIN
INSERT INTO
    [AspNetRoles] (
        [Id],
        [Name],
        [NormalizedName],
        [ConcurrencyStamp],
        [CreatedAt],
        [Description]
    )
VALUES (
        '2',
        'Author',
        'AUTHOR',
        '95F9EF2A-231F-4266-AEE9-355C5444D121',
        '2025-10-05 11:12:38.8200000',
        'Can create, edit, and publish blog posts'
    );

END IF NOT EXISTS (
    SELECT 1
    FROM AspNetRoles
    WHERE
        Id = '3'
) BEGIN
INSERT INTO
    [AspNetRoles] (
        [Id],
        [Name],
        [NormalizedName],
        [ConcurrencyStamp],
        [CreatedAt],
        [Description]
    )
VALUES (
        '3',
        'Editor',
        'EDITOR',
        '51FF254C-0A30-442C-939A-CE9DE9A1CD12',
        '2025-10-05 11:12:38.8200000',
        'Can review and edit posts from authors'
    );

END IF NOT EXISTS (
    SELECT 1
    FROM AspNetRoles
    WHERE
        Id = '4'
) BEGIN
INSERT INTO
    [AspNetRoles] (
        [Id],
        [Name],
        [NormalizedName],
        [ConcurrencyStamp],
        [CreatedAt],
        [Description]
    )
VALUES (
        '4',
        'User',
        'USER',
        '53FA0373-506C-48EC-B65C-9B16136AE5D5',
        '2025-10-05 11:12:38.8200000',
        'Standard user with basic access to read and comment'
    );

END IF NOT EXISTS (
    SELECT 1
    FROM AspNetRoles
    WHERE
        Id = 'a97a4429-e07c-4a6b-8fc5-b3a623487b47'
) BEGIN
INSERT INTO
    [AspNetRoles] (
        [Id],
        [Name],
        [NormalizedName],
        [ConcurrencyStamp],
        [CreatedAt],
        [Description]
    )
VALUES (
        'a97a4429-e07c-4a6b-8fc5-b3a623487b47',
        'Subscriber',
        'SUBSCRIBER',
        NULL,
        '2025-10-05 11:13:44.0754799',
        'Can comment and subscribe'
    );

END

-- Insert Users (passwords are hashed for 'BlogMVC123!')
IF NOT EXISTS (
    SELECT 1
    FROM AspNetUsers
    WHERE
        Id = 'admin-user-id-12345'
) BEGIN
INSERT INTO
    [AspNetUsers] (
        [Id],
        [UserName],
        [NormalizedUserName],
        [Email],
        [NormalizedEmail],
        [EmailConfirmed],
        [PasswordHash],
        [SecurityStamp],
        [ConcurrencyStamp],
        [PhoneNumber],
        [PhoneNumberConfirmed],
        [TwoFactorEnabled],
        [LockoutEnd],
        [LockoutEnabled],
        [AccessFailedCount],
        [FullName],
        [Bio],
        [ProfilePicture],
        [CreatedAt],
        [UpdatedAt],
        [FirstName],
        [LastName],
        [Biography],
        [ProfileImageUrl],
        [IsActive]
    )
VALUES (
        'admin-user-id-12345',
        'admin@blogmvc.com',
        'ADMIN@BLOGMVC.COM',
        'admin@blogmvc.com',
        'ADMIN@BLOGMVC.COM',
        1,
        'AQAAAAEAACcQAAAAEKXmgJ8+yKWS1Z2QXmF0Qj/sJ2K/9mN1hQ3FvC2rP8Xk5zY7tR6sW8eB9nT3qA1M0L==',
        'ADMIN-SECURITY-STAMP',
        'ADMIN-CONCURRENCY-STAMP',
        NULL,
        0,
        0,
        NULL,
        1,
        0,
        'System Administrator',
        'Main administrator of the BlogMVC application',
        NULL,
        '2025-10-05 11:00:08.0100000',
        '2025-10-05 11:00:08.0100000',
        'System',
        'Administrator',
        'Main administrator of the BlogMVC application',
        NULL,
        1
    );

END IF NOT EXISTS (
    SELECT 1
    FROM AspNetUsers
    WHERE
        Id = 'author-user-id-12345'
) BEGIN
INSERT INTO
    [AspNetUsers] (
        [Id],
        [UserName],
        [NormalizedUserName],
        [Email],
        [NormalizedEmail],
        [EmailConfirmed],
        [PasswordHash],
        [SecurityStamp],
        [ConcurrencyStamp],
        [PhoneNumber],
        [PhoneNumberConfirmed],
        [TwoFactorEnabled],
        [LockoutEnd],
        [LockoutEnabled],
        [AccessFailedCount],
        [FullName],
        [Bio],
        [ProfilePicture],
        [CreatedAt],
        [UpdatedAt],
        [FirstName],
        [LastName],
        [Biography],
        [ProfileImageUrl],
        [IsActive]
    )
VALUES (
        'author-user-id-12345',
        'author@blogmvc.com',
        'AUTHOR@BLOGMVC.COM',
        'author@blogmvc.com',
        'AUTHOR@BLOGMVC.COM',
        1,
        'AQAAAAEAACcQAAAAEKXmgJ8+yKWS1Z2QXmF0Qj/sJ2K/9mN1hQ3FvC2rP8Xk5zY7tR6sW8eB9nT3qA1M0L==',
        'AUTHOR-SECURITY-STAMP',
        'AUTHOR-CONCURRENCY-STAMP',
        NULL,
        0,
        0,
        NULL,
        1,
        0,
        'John Doe',
        'Senior software developer and technical writer',
        NULL,
        '2025-10-05 11:00:08.0100000',
        '2025-10-05 11:00:08.0100000',
        'John',
        'Doe',
        'Senior software developer and technical writer',
        NULL,
        1
    );

END

-- User Role Assignments
IF NOT EXISTS (
    SELECT 1
    FROM AspNetUserRoles
    WHERE
        UserId = 'admin-user-id-12345'
        AND RoleId = '1'
) BEGIN
INSERT INTO
    [AspNetUserRoles] ([UserId], [RoleId])
VALUES ('admin-user-id-12345', '1');
-- Admin role
END IF NOT EXISTS (
    SELECT 1
    FROM AspNetUserRoles
    WHERE
        UserId = 'author-user-id-12345'
        AND RoleId = '2'
) BEGIN
INSERT INTO
    [AspNetUserRoles] ([UserId], [RoleId])
VALUES ('author-user-id-12345', '2');
-- Author role
END

-- Insert Categories
SET IDENTITY_INSERT [Categories] ON;

IF NOT EXISTS (
    SELECT 1
    FROM Categories
    WHERE
        Id = 1
) BEGIN
INSERT INTO
    [Categories] (
        [Id],
        [Name],
        [Description],
        [Slug],
        [CreatedAt],
        [UpdatedAt],
        [CreatedBy],
        [IsActive]
    )
VALUES (
        1,
        'Technology',
        'Articles about technology, software, and programming',
        'technology',
        '2025-10-05 11:00:08.0100000',
        '2025-10-05 11:00:08.0100000',
        'admin-user-id-12345',
        1
    );

END IF NOT EXISTS (
    SELECT 1
    FROM Categories
    WHERE
        Id = 3
) BEGIN
INSERT INTO
    [Categories] (
        [Id],
        [Name],
        [Description],
        [Slug],
        [CreatedAt],
        [UpdatedAt],
        [CreatedBy],
        [IsActive]
    )
VALUES (
        3,
        'Programming',
        'Programming tutorials, code examples, and development tips',
        'programming',
        '2025-10-05 11:00:08.0100000',
        '2025-10-05 11:00:08.0100000',
        'admin-user-id-12345',
        1
    );

END IF NOT EXISTS (
    SELECT 1
    FROM Categories
    WHERE
        Id = 8
) BEGIN
INSERT INTO
    [Categories] (
        [Id],
        [Name],
        [Description],
        [Slug],
        [CreatedAt],
        [UpdatedAt],
        [CreatedBy],
        [IsActive]
    )
VALUES (
        8,
        'DevOps',
        'DevOps practices, CI/CD, and infrastructure',
        'devops',
        '2025-10-05 11:00:08.0100000',
        '2025-10-05 11:00:08.0100000',
        'admin-user-id-12345',
        1
    );

END

SET IDENTITY_INSERT [Categories] OFF;
GO

-- Insert Tags
SET IDENTITY_INSERT [Tags] ON;

INSERT INTO
    [Tags] (
        [Id],
        [Name],
        [Slug],
        [CreatedAt]
    )
VALUES (
        1,
        'ASP.NET Core',
        'aspnet-core',
        '2025-10-05 11:00:08.0100000'
    ),
    (
        2,
        'C#',
        'csharp',
        '2025-10-05 11:00:08.0100000'
    ),
    (
        3,
        'Entity Framework',
        'entity-framework',
        '2025-10-05 11:00:08.0100000'
    ),
    (
        4,
        'Docker',
        'docker',
        '2025-10-05 11:00:08.0100000'
    ),
    (
        5,
        'GitHub Actions',
        'github-actions',
        '2025-10-05 11:00:08.0100000'
    ),
    (
        16,
        'Kubernetes',
        'kubernetes',
        '2025-10-05 11:00:08.0100000'
    ),
    (
        17,
        'CI/CD',
        'cicd',
        '2025-10-05 11:00:08.0100000'
    ),
    (
        18,
        'Testing',
        'testing',
        '2025-10-05 11:00:08.0100000'
    );

SET IDENTITY_INSERT [Tags] OFF;
GO

-- Insert Posts
SET IDENTITY_INSERT [Posts] ON;

IF NOT EXISTS (
    SELECT 1
    FROM Posts
    WHERE
        Id = 1
) BEGIN
INSERT INTO
    [Posts] (
        [Id],
        [Title],
        [Content],
        [Excerpt],
        [Slug],
        [Status],
        [FeaturedImage],
        [MetaTitle],
        [MetaDescription],
        [ViewCount],
        [AuthorId],
        [CategoryId],
        [CreatedAt],
        [UpdatedAt],
        [PublishedAt],
        [FeaturedImageUrl]
    )
VALUES (
        1,
        'Welcome to BlogMVC',
        'Welcome to BlogMVC, a modern blogging platform built with ASP.NET Core. This platform demonstrates the power of modern web development technologies including Entity Framework Core, Docker containerization, and automated CI/CD workflows.',
        'A comprehensive introduction to the BlogMVC platform and its features.',
        'welcome-to-blogmvc',
        'Published',
        NULL,
        'Welcome to BlogMVC - Modern Blogging Platform',
        'Discover BlogMVC, a cutting-edge blogging platform built with ASP.NET Core, Entity Framework, and modern web technologies.',
        0,
        'author-user-id-12345',
        1,
        '2025-09-28 11:00:08.0133333',
        '2025-09-28 11:00:08.0133333',
        '2025-09-28 11:00:08.0133333',
        NULL
    );

END

SET IDENTITY_INSERT [Posts] OFF;
GO

-- Insert Post-Tag relationships
IF NOT EXISTS (
    SELECT 1
    FROM PostTags
    WHERE
        PostId = 1
        AND TagId = 1
) BEGIN
INSERT INTO
    [PostTags] ([PostId], [TagId])
VALUES (1, 1);
-- Welcome post - ASP.NET Core
INSERT INTO [PostTags] ([PostId], [TagId]) VALUES (1, 2);
-- Welcome post - C#
INSERT INTO [PostTags] ([PostId], [TagId]) VALUES (1, 3);
-- Welcome post - Entity Framework
END

PRINT 'BlogMVC complete database initialization completed successfully!';
GO