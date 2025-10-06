# BlogMVC Database Setup Guide

## 🎯 Quick Start (Recommended)

Since we've committed the database state to a Docker image, you can now start the database with all data pre-loaded:

```powershell
.\start-database.ps1
```

This will:
- Stop any existing database containers
- Start a new container with the committed image `blogmvc-sqlserver:with-data`
- Database will be ready with all data (users, roles, categories, posts, etc.)

## 📋 Available Scripts

### 1. `start-database.ps1` ⭐ (Recommended)
- **Use**: Quick startup with pre-loaded data
- **Speed**: Fast (no initialization needed)
- **Data**: Includes all current database content

### 2. `init-database.ps1` (Optional)
- **Use**: Fresh database initialization from SQL scripts
- **Speed**: Slower (runs SQL scripts)
- **Data**: Uses SQL files from `Database/` folder

## 🔧 Database Connection Details

```
Server: localhost,50506
Database: BlogMVCApp
Username: sa
Password: amjadOmar1!A
```

## 📊 What's Included in the Committed Image

- ✅ All ASP.NET Core Identity tables
- ✅ 5 Roles (Admin, Author, Editor, User, Subscriber)
- ✅ 6 Test users with proper roles
- ✅ 8 Categories
- ✅ 20 Tags
- ✅ Sample posts with relationships
- ✅ All indexes and constraints

## 🚀 Benefits of Committed Image

1. **Instant Startup** - No need to run SQL scripts
2. **Consistent Data** - Same data every time
3. **Team Sync** - Everyone gets the same database state
4. **CI/CD Ready** - Can be used in automated pipelines

## 🔄 If You Need to Update the Database

1. Make changes to your running database
2. Commit the container again:
   ```powershell
   docker commit <container-name> blogmvc-sqlserver:with-data
   ```
3. Next startup will use the updated data

## 📝 Notes

- The old `distracted_franklin` container will be automatically stopped and removed
- The new container is named `blogmvc-database`
- All database files in the `Database/` folder are still maintained for reference