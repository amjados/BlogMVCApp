-- =================================================================-- =============================================

-- BlogMVC Database Schema Creation Script-- BlogMVC Database Schema Creation Script

-- Generated from current database state on: $(Date)-- Generated for GitHub Actions Testing

-- Description: Complete table structure with all columns, constraints, and indexes-- =============================================

-- =================================================================

USE [BlogMVCApp]

USE [BlogMVCApp];

-- Drop existing tables if they exist (for clean setup)
GO -- Drop existing tables if they exist (for clean setup)

IF OBJECT_ID('dbo.PostTags', 'U') IS NOT NULL

-- =================================================================DROP TABLE [dbo].[PostTags] IF OBJECT_ID('dbo.Comments', 'U') IS NOT NULL

-- ASP.NET Core Identity TablesDROP TABLE [dbo].[Comments] IF OBJECT_ID('dbo.Posts', 'U') IS NOT NULL

-- =================================================================DROP TABLE [dbo].[Posts] IF OBJECT_ID('dbo.AuditLogs', 'U') IS NOT NULL

DROP TABLE [dbo].[AuditLogs] IF OBJECT_ID('dbo.UserSessions', 'U') IS NOT NULL

-- AspNetRolesDROP TABLE [dbo].[UserSessions] IF OBJECT_ID('dbo.Categories', 'U') IS NOT NULL

IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[AspNetRoles]')
        AND
    type in (N'U')
)
DROP TABLE [dbo].[Categories] IF OBJECT_ID('dbo.Tags', 'U') IS NOT NULL BEGINDROP
TABLE [dbo].[Tags] IF OBJECT_ID('dbo.AspNetUserTokens', 'U') IS NOT NULL

CREATE TABLE [dbo].[AspNetRoles] (
    DROP TABLE [dbo].[AspNetUserTokens] IF OBJECT_ID('dbo.AspNetUserRoles', 'U') IS NOT NULL [Id] nvarchar(450) NOT NULL,
    DROP TABLE [dbo].[AspNetUserRoles] IF OBJECT_ID('dbo.AspNetUserLogins', 'U') IS NOT NULL [Name] nvarchar(256) NULL,
    DROP TABLE [dbo].[AspNetUserLogins] IF OBJECT_ID('dbo.AspNetUserClaims', 'U') IS NOT NULL [NormalizedName] nvarchar(256) NULL,
    DROP TABLE [dbo].[AspNetUserClaims] IF OBJECT_ID('dbo.AspNetRoleClaims', 'U') IS NOT NULL [ConcurrencyStamp] nvarchar(max) NULL,
    DROP TABLE [dbo].[AspNetRoleClaims] IF OBJECT_ID('dbo.AspNetUsers', 'U') IS NOT NULL [CreatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),
    DROP TABLE [dbo].[AspNetUsers] IF OBJECT_ID('dbo.AspNetRoles', 'U') IS NOT NULL [Description] nvarchar(500) NULL,
    DROP TABLE [dbo].[AspNetRoles] CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
-- =============================================

-- ASP.NET Core Identity Tables
END -- ASP.NET Core Identity Tables

-- =============================================
GO -- =============================================

-- AspNetUsers-- AspNetRoles Table

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND type in (N'U'))CREATE TABLE [dbo].[AspNetRoles] (

BEGIN    [Id] [nvarchar] (450) NOT NULL,

    CREATE TABLE [dbo].[AspNetUsers] (    [Name] [nvarchar] (256) NULL,

        [Id] nvarchar(450) NOT NULL,    [NormalizedName] [nvarchar] (256) NULL,

        [UserName] nvarchar(256) NULL,    [ConcurrencyStamp] [nvarchar] (max) NULL,

        [NormalizedUserName] nvarchar(256) NULL,    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)

        [Email] nvarchar(256) NULL,)

        [NormalizedEmail] nvarchar(256) NULL,

        [EmailConfirmed] bit NOT NULL,-- AspNetUsers Table

        [PasswordHash] nvarchar(max) NULL,CREATE TABLE [dbo].[AspNetUsers] (

        [SecurityStamp] nvarchar(max) NULL,    [Id] [nvarchar] (450) NOT NULL,

        [ConcurrencyStamp] nvarchar(max) NULL,    [UserName] [nvarchar] (256) NULL,

        [PhoneNumber] nvarchar(max) NULL,    [NormalizedUserName] [nvarchar] (256) NULL,

        [PhoneNumberConfirmed] bit NOT NULL,    [Email] [nvarchar] (256) NULL,

        [TwoFactorEnabled] bit NOT NULL,    [NormalizedEmail] [nvarchar] (256) NULL,

        [LockoutEnd] datetimeoffset(7) NULL,    [EmailConfirmed] [bit] NOT NULL,

        [LockoutEnabled] bit NOT NULL,    [PasswordHash] [nvarchar] (max) NULL,

        [AccessFailedCount] int NOT NULL,    [SecurityStamp] [nvarchar] (max) NULL,

        [FullName] nvarchar(100) NULL,    [ConcurrencyStamp] [nvarchar] (max) NULL,

        [Bio] nvarchar(500) NULL,    [PhoneNumber] [nvarchar] (max) NULL,

        [ProfilePicture] nvarchar(255) NULL,    [PhoneNumberConfirmed] [bit] NOT NULL,

        [CreatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),    [TwoFactorEnabled] [bit] NOT NULL,

        [UpdatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),    [LockoutEnd] [datetimeoffset] (7) NULL,

        [FirstName] nvarchar(100) NULL,    [LockoutEnabled] [bit] NOT NULL,

        [LastName] nvarchar(100) NULL,    [AccessFailedCount] [int] NOT NULL,

        [Biography] nvarchar(1000) NULL,    [FullName] [nvarchar] (100) NULL,

        [ProfileImageUrl] nvarchar(500) NULL,    [Bio] [nvarchar] (500) NULL,

        [IsActive] bit NOT NULL DEFAULT ((1)),    [ProfilePicture] [nvarchar] (255) NULL,

        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])    [CreatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),

    );    [UpdatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),

END

CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)

GO
)

-- AspNetRoleClaims-- AspNetRoleClaims Table

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoleClaims]') AND type in (N'U'))CREATE TABLE [dbo].[AspNetRoleClaims] (

BEGIN    [Id] [int] IDENTITY(1, 1) NOT NULL,

    CREATE TABLE [dbo].[AspNetRoleClaims] (    [RoleId] [nvarchar] (450) NOT NULL,

        [Id] int IDENTITY(1,1) NOT NULL,    [ClaimType] [nvarchar] (max) NULL,

        [RoleId] nvarchar(450) NOT NULL,    [ClaimValue] [nvarchar] (max) NULL,

        [ClaimType] nvarchar(max) NULL,    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED ([Id] ASC),

        [ClaimValue] nvarchar(max) NULL,    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE

        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]))

    );

END-- AspNetUserClaims Table


GOCREATE TABLE [dbo].[AspNetUserClaims] (

    [Id] [int] IDENTITY(1, 1) NOT NULL,

-- AspNetUserClaims    [UserId] [nvarchar] (450) NOT NULL,

IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]')
        AND
    type in (N'U')
) [ClaimType] [nvarchar] (max) NULL,

BEGIN    [ClaimValue] [nvarchar] (max) NULL,

    CREATE TABLE [dbo].[AspNetUserClaims] (    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),

        [Id] int IDENTITY(1,1) NOT NULL,    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE

        [UserId] nvarchar(450) NOT NULL,)

        [ClaimType] nvarchar(max) NULL,

        [ClaimValue] nvarchar(max) NULL,-- AspNetUserLogins Table

        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id])CREATE TABLE [dbo].[AspNetUserLogins] (

    );    [LoginProvider] [nvarchar] (450) NOT NULL,

END

[ProviderKey] [nvarchar] (450) NOT NULL,

GO
[ProviderDisplayName] [nvarchar] (max) NULL,
[UserId] [nvarchar] (450) NOT NULL,

-- AspNetUserLogins    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED (

IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]')
        AND
    type in (N'U')
) [LoginProvider] ASC,

BEGIN        [ProviderKey] ASC

    CREATE TABLE [dbo].[AspNetUserLogins] (    ),

        [LoginProvider] nvarchar(450) NOT NULL,    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE

        [ProviderKey] nvarchar(450) NOT NULL,)

        [ProviderDisplayName] nvarchar(max) NULL,

        [UserId] nvarchar(450) NOT NULL,-- AspNetUserRoles Table

        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey])CREATE TABLE [dbo].[AspNetUserRoles] (

    );    [UserId] [nvarchar] (450) NOT NULL,

END

[RoleId] [nvarchar] (450) NOT NULL,

GO
CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,

-- AspNetUserRoles    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]') AND type in (N'U')))

BEGIN

    CREATE TABLE [dbo].[AspNetUserRoles] (-- AspNetUserTokens Table

        [UserId] nvarchar(450) NOT NULL,CREATE TABLE [dbo].[AspNetUserTokens] (

        [RoleId] nvarchar(450) NOT NULL,    [UserId] [nvarchar] (450) NOT NULL,

        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId])    [LoginProvider] [nvarchar] (450) NOT NULL,

    );    [Name] [nvarchar] (450) NOT NULL,

END

[Value] [nvarchar] (max) NULL,

GO
CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED (

        [UserId] ASC,

-- AspNetUserTokens        [LoginProvider] ASC,

IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[AspNetUserTokens]')
        AND
    type in (N'U')
) [Name] ASC

BEGIN    ),

    CREATE TABLE [dbo].[AspNetUserTokens] (    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE

        [UserId] nvarchar(450) NOT NULL,)

        [LoginProvider] nvarchar(450) NOT NULL,

        [Name] nvarchar(450) NOT NULL,-- =============================================

        [Value] nvarchar(max) NULL,-- Blog Application Tables

        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name])-- =============================================

    );

END-- Categories Table


GOCREATE TABLE [dbo].[Categories] (

    [Id] [int] IDENTITY(1, 1) NOT NULL,

-- =================================================================    [Name] [nvarchar] (100) NOT NULL,

-- Blog Application Tables    [Description] [nvarchar] (500) NULL,

-- =================================================================    [Slug] [nvarchar] (100) NULL,

[CreatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),

-- Categories    [UpdatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),

IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[Categories]')
        AND
    type in (N'U')
) CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([Id] ASC)

BEGIN)

    CREATE TABLE [dbo].[Categories] (

        [Id] int IDENTITY(1,1) NOT NULL,-- Tags Table

        [Name] nvarchar(100) NOT NULL,CREATE TABLE [dbo].[Tags] (

        [Description] nvarchar(500) NULL,    [Id] [int] IDENTITY(1, 1) NOT NULL,

        [Slug] nvarchar(100) NULL,    [Name] [nvarchar] (50) NOT NULL,

        [CreatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),    [Slug] [nvarchar] (50) NULL,

        [UpdatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),    [CreatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),

        [CreatedBy] nvarchar(450) NOT NULL DEFAULT ('admin-user-id-12345'),    CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED ([Id] ASC)

        [IsActive] bit NOT NULL DEFAULT ((1)),)

        CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])

    );-- Posts Table

ENDCREATE TABLE [dbo].[Posts] (

[Id] [int] IDENTITY(1, 1) NOT NULL,
[Title] [nvarchar] (200) NOT NULL,

-- Posts    [Content] [nvarchar] (max) NOT NULL,

IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[Posts]')
        AND
    type in (N'U')
) [Excerpt] [nvarchar] (500) NULL,

BEGIN    [Slug] [nvarchar] (200) NULL,

    CREATE TABLE [dbo].[Posts] (    [Status] [nvarchar] (50) NOT NULL DEFAULT ('Draft'),

        [Id] int IDENTITY(1,1) NOT NULL,    [FeaturedImage] [nvarchar] (255) NULL,

        [Title] nvarchar(200) NOT NULL,    [MetaTitle] [nvarchar] (200) NULL,

        [Content] nvarchar(max) NOT NULL,    [MetaDescription] [nvarchar] (500) NULL,

        [Excerpt] nvarchar(500) NULL,    [ViewCount] [int] NOT NULL DEFAULT (0),

        [Slug] nvarchar(200) NULL,    [AuthorId] [nvarchar] (450) NULL,

        [Status] nvarchar(50) NOT NULL DEFAULT ('Draft'),    [CategoryId] [int] NULL,

        [FeaturedImage] nvarchar(255) NULL,    [CreatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),

        [MetaTitle] nvarchar(200) NULL,    [UpdatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),

        [MetaDescription] nvarchar(500) NULL,    [PublishedAt] [datetime2] (7) NULL,

        [ViewCount] int NOT NULL DEFAULT ((0)),    CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED ([Id] ASC),

        [AuthorId] nvarchar(450) NULL,    CONSTRAINT [FK_Posts_AspNetUsers_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[AspNetUsers] ([Id]),

        [CategoryId] int NULL,    CONSTRAINT [FK_Posts_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id])

        [CreatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),)

        [UpdatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),

        [PublishedAt] datetime2 NULL,-- PostTags Junction Table

        [FeaturedImageUrl] nvarchar(500) NULL,CREATE TABLE [dbo].[PostTags] (

        CONSTRAINT [PK_Posts] PRIMARY KEY ([Id])    [PostId] [int] NOT NULL,

    );    [TagId] [int] NOT NULL,

END

CONSTRAINT [PK_PostTags] PRIMARY KEY CLUSTERED ([PostId] ASC, [TagId] ASC),

GO
CONSTRAINT [FK_PostTags_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Posts] ([Id]) ON DELETE CASCADE,
CONSTRAINT [FK_PostTags_Tags_TagId] FOREIGN KEY ([TagId]) REFERENCES [dbo].[Tags] ([Id]) ON DELETE CASCADE

-- Tags)

IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[Tags]')
        AND
    type in (N'U')
)

-- Comments Table
BEGIN-- Comments Table

    CREATE TABLE [dbo].[Tags] (CREATE TABLE [dbo].[Comments] (

        [Id] int IDENTITY(1,1) NOT NULL,    [Id] [int] IDENTITY(1, 1) NOT NULL,

        [Name] nvarchar(50) NOT NULL,    [Content] [nvarchar] (1000) NOT NULL,

        [Slug] nvarchar(50) NULL,    [AuthorName] [nvarchar] (100) NOT NULL,

        [CreatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),    [AuthorEmail] [nvarchar] (255) NOT NULL,

        CONSTRAINT [PK_Tags] PRIMARY KEY ([Id])    [AuthorWebsite] [nvarchar] (255) NULL,

    );    [IsApproved] [bit] NOT NULL DEFAULT (0),

END

[PostId] [int] NOT NULL,

GO
[ParentCommentId] [int] NULL,
[CreatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),

-- PostTags (Many-to-Many relationship)    CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED ([Id] ASC),

IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[PostTags]')
        AND
    type in (N'U')
) CONSTRAINT [FK_Comments_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Posts] ([Id]) ON DELETE CASCADE,

BEGIN    CONSTRAINT [FK_Comments_Comments_ParentCommentId] FOREIGN KEY ([ParentCommentId]) REFERENCES [dbo].[Comments] ([Id])

    CREATE TABLE [dbo].[PostTags] ()

        [PostId] int NOT NULL,

        [TagId] int NOT NULL,-- UserSessions Table

        CONSTRAINT [PK_PostTags] PRIMARY KEY ([PostId], [TagId])CREATE TABLE [dbo].[UserSessions] (

    );    [Id] [int] IDENTITY(1, 1) NOT NULL,

END

[UserId] [nvarchar] (450) NOT NULL,

GO
[SessionId] [nvarchar] (450) NOT NULL,
[IpAddress] [nvarchar] (45) NULL,

-- Comments    [UserAgent] [nvarchar] (500) NULL,

IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[Comments]')
        AND
    type in (N'U')
) [CreatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),

BEGIN    [LastAccessAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),

    CREATE TABLE [dbo].[Comments] (    [IsActive] [bit] NOT NULL DEFAULT (1),

        [Id] int IDENTITY(1,1) NOT NULL,    CONSTRAINT [PK_UserSessions] PRIMARY KEY CLUSTERED ([Id] ASC),

        [Content] nvarchar(1000) NOT NULL,    CONSTRAINT [FK_UserSessions_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE

        [AuthorName] nvarchar(100) NOT NULL,)

        [AuthorEmail] nvarchar(255) NOT NULL,

        [AuthorWebsite] nvarchar(255) NULL,-- AuditLogs Table

        [IsApproved] bit NOT NULL DEFAULT ((0)),CREATE TABLE [dbo].[AuditLogs] (

        [PostId] int NOT NULL,    [Id] [int] IDENTITY(1, 1) NOT NULL,

        [ParentCommentId] int NULL,    [TableName] [nvarchar] (100) NOT NULL,

        [CreatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),    [RecordId] [nvarchar] (450) NOT NULL,

        CONSTRAINT [PK_Comments] PRIMARY KEY ([Id])    [Action] [nvarchar] (50) NOT NULL,

    );    [OldValues] [nvarchar] (max) NULL,

END

[NewValues] [nvarchar] (max) NULL,

GO
[UserId] [nvarchar] (450) NULL,
[IpAddress] [nvarchar] (45) NULL,

-- =================================================================    [UserAgent] [nvarchar] (500) NULL,

-- System and Audit Tables    [CreatedAt] [datetime2] (7) NOT NULL DEFAULT (GETUTCDATE()),

-- =================================================================    CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED ([Id] ASC),

CONSTRAINT [FK_AuditLogs_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])

-- AuditLogs)

IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[AuditLogs]')
        AND
    type in (N'U')
)

-- =============================================
BEGIN-- =============================================

    CREATE TABLE [dbo].[AuditLogs] (-- Create Indexes for Performance

        [Id] int IDENTITY(1,1) NOT NULL,-- =============================================

        [TableName] nvarchar(100) NOT NULL,

        [RecordId] nvarchar(450) NOT NULL,-- Identity Indexes

        [Action] nvarchar(50) NOT NULL,CREATE NONCLUSTERED

        [OldValues] nvarchar(max) NULL,INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims] ([RoleId])

        [NewValues] nvarchar(max) NULL,

        [UserId] nvarchar(450) NULL,CREATE UNIQUE NONCLUSTERED

        [IpAddress] nvarchar(45) NULL,INDEX [RoleNameIndex] ON [dbo].[AspNetRoles] ([NormalizedName])

        [UserAgent] nvarchar(500) NULL,WHERE ([NormalizedName] IS NOT NULL)

        [CreatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),

        CONSTRAINT [PK_AuditLogs] PRIMARY KEY ([Id])CREATE NONCLUSTERED

    );INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims] ([UserId])

END

GOCREATE NONCLUSTERED
INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins] ([UserId])

-- UserSessions

IF NOT EXISTS (
    SELECT *
    FROM sys.objects
    WHERE
        object_id = OBJECT_ID(N'[dbo].[UserSessions]')
        AND
    type in (N'U')
) CREATE NONCLUSTERED BEGININDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles] ([RoleId])

CREATE TABLE [dbo].[UserSessions] (
    [Id] int IDENTITY(1, 1) NOT NULL,
    CREATE NONCLUSTERED [UserId] nvarchar(450) NOT NULL,
    INDEX [EmailIndex] ON [dbo].[AspNetUsers] ([NormalizedEmail]) [SessionId] nvarchar(450) NOT NULL,
    [IpAddress] nvarchar(45) NULL,
    CREATE UNIQUE NONCLUSTERED [UserAgent] nvarchar(500) NULL,
    INDEX [UserNameIndex] ON [dbo].[AspNetUsers] ([NormalizedUserName]) [CreatedAt] datetime2 NOT NULL DEFAULT (getutcdate()),
    WHERE (
            [LastAccessAt] datetime2 NOT NULL DEFAULT (getutcdate()),
            [NormalizedUserName] IS NOT NULL [IsActive] bit NOT NULL DEFAULT ((1)),
        ) CONSTRAINT [PK_UserSessions] PRIMARY KEY ([Id])
);
-- Blog Application Indexes

ENDCREATE NONCLUSTERED GOINDEX [IX_Posts_AuthorId] ON [dbo].[Posts] ([AuthorId])

-- =================================================================CREATE NONCLUSTERED

-- Foreign Key ConstraintsINDEX [IX_Posts_CategoryId] ON [dbo].[Posts] ([CategoryId])

-- =================================================================

CREATE NONCLUSTERED

-- AspNetRoleClaimsINDEX [IX_Posts_Status] ON [dbo].[Posts] ([Status])

IF NOT EXISTS (
    SELECT *
    FROM sys.foreign_keys
    WHERE
        object_id = OBJECT_ID(
            N'[dbo].[FK_AspNetRoleClaims_AspNetRoles_RoleId]'
        )
) BEGINCREATE NONCLUSTERED

ALTER TABLE [dbo].[AspNetRoleClaims]
ADD CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
INDEX [IX_Posts_PublishedAt] ON [dbo].[Posts] ([PublishedAt]) FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE;

ENDCREATE NONCLUSTERED
INDEX [IX_Posts_Slug] ON [dbo].[Posts] ([Slug])

GO

CREATE NONCLUSTERED

-- AspNetUserClaimsINDEX [IX_Comments_PostId] ON [dbo].[Comments] ([PostId])

IF NOT EXISTS (
    SELECT *
    FROM sys.foreign_keys
    WHERE
        object_id = OBJECT_ID(
            N'[dbo].[FK_AspNetUserClaims_AspNetUsers_UserId]'
        )
) BEGINCREATE NONCLUSTERED

ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
INDEX [IX_Comments_ParentCommentId] ON [dbo].[Comments] ([ParentCommentId]) FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;

ENDCREATE NONCLUSTERED GOINDEX [IX_Comments_IsApproved] ON [dbo].[Comments] ([IsApproved])

-- AspNetUserLoginsCREATE NONCLUSTERED

IF NOT EXISTS (
    SELECT *
    FROM sys.foreign_keys
    WHERE
        object_id = OBJECT_ID(
            N'[dbo].[FK_AspNetUserLogins_AspNetUsers_UserId]'
        )
)
INDEX [IX_UserSessions_UserId] ON [dbo].[UserSessions] ([UserId])

BEGIN

    ALTER TABLE [dbo].[AspNetUserLogins] ADD CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] CREATE NONCLUSTERED

    FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;INDEX [IX_UserSessions_SessionId] ON [dbo].[UserSessions] ([SessionId])

END

GOCREATE NONCLUSTERED
INDEX [IX_AuditLogs_UserId] ON [dbo].[AuditLogs] ([UserId])

-- AspNetUserRoles

IF NOT EXISTS (
    SELECT *
    FROM sys.foreign_keys
    WHERE
        object_id = OBJECT_ID(
            N'[dbo].[FK_AspNetUserRoles_AspNetRoles_RoleId]'
        )
) CREATE NONCLUSTERED BEGININDEX [IX_AuditLogs_TableName] ON [dbo].[AuditLogs] ([TableName])

ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE;

CREATE NONCLUSTERED ENDINDEX [IX_Categories_Slug] ON [dbo].[Categories] ([Slug])

GO

CREATE NONCLUSTERED
INDEX [IX_Tags_Slug] ON [dbo].[Tags] ([Slug]) IF NOT EXISTS (
    SELECT *
    FROM sys.foreign_keys
    WHERE
        object_id = OBJECT_ID(
            N'[dbo].[FK_AspNetUserRoles_AspNetUsers_UserId]'
        )
) BEGINPRINT 'Database schema created successfully!'
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

PRINT 'Database schema creation completed successfully.';
GO