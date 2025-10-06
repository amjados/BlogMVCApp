# Database SQL Scripts

This folder contains SQL scripts for database schema and data management.

## 📁 Folder Structure

```
Database/
├── Schema/
│   └── 01-CreateTables.sql           # Complete database schema creation
├── Data/
│   └── 02-SeedData.sql              # Seed data for initial setup
└── Complete/
    └── InitializeDatabase.sql        # Combined schema + data script
```

## 📋 Script Descriptions

### Schema/01-CreateTables.sql
- **Purpose**: Creates all database tables, constraints, and indexes
- **Contains**: 14 tables (Identity + Blog tables)
- **Use**: Database schema documentation and manual setup

### Data/02-SeedData.sql
- **Purpose**: Populates database with initial data
- **Contains**: Roles, users, categories, tags, and sample posts
- **Use**: Test data setup and development environment initialization

### Complete/InitializeDatabase.sql
- **Purpose**: Complete database initialization (schema + data)
- **Contains**: Full database setup from scratch
- **Use**: Fresh database deployment and CI/CD pipelines

## 🚀 Usage Scenarios

### For Development (Recommended)
Use the committed Docker image:
```powershell
.\start-database.ps1
```

### For Manual Setup
Run scripts in order:
1. `Schema/01-CreateTables.sql`
2. `Data/02-SeedData.sql`

### For Fresh Deployment
Use the complete script:
```sql
Complete/InitializeDatabase.sql
```

## 📝 Notes

- Scripts are kept in sync with Entity Framework models
- All scripts use `IF NOT EXISTS` checks for safe execution
- Files serve as documentation for the current database schema
- Generated from actual working database state (October 2025)

## 🔄 Maintenance

These files are maintained alongside Entity Framework models and should be updated when schema changes occur.