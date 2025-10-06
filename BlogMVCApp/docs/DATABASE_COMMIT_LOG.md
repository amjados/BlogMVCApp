# Database Docker Commit Log

## Latest Commit: v2-admin-fixed

**Date**: October 6, 2025  
**Image**: `blogmvcapp-db:v2-admin-fixed` (also tagged as `latest`)  
**Container Source**: `distracted_franklin`  
**Image ID**: `24cd4414bc12`  

### Changes Included

1. **Fixed Admin Login Issue**
   - Admin password reset mechanism added to DataSeeder
   - Password automatically resets to "Admin123!" on application startup
   - Ensures admin can always log in with correct credentials

2. **Database State**
   - All seed data intact (users, posts, categories, tags)
   - Admin user: `admin@blogmvc.com` / `Admin123!`
   - Other test users: `john.doe@blogmvc.com`, `jane.smith@blogmvc.com`
   - Complete blog data with posts and relationships

3. **Technical Fixes**
   - PerformanceMonitoringMiddleware headers issue resolved
   - DataSeeder enhanced with password reset logic
   - All middleware working without exceptions

### Usage

To use this database image:

```bash
# Stop current container (if running)
docker stop distracted_franklin

# Remove old container
docker rm distracted_franklin

# Run with the committed image
docker run -d --name blogmvc-db \
  -e ACCEPT_EULA=Y \
  -e SA_PASSWORD="amjadOmar1!A" \
  -p 50506:1433 \
  blogmvcapp-db:latest
```

### Verification

The commit was verified with successful admin login:
```
info: BlogMVCApp.Controllers.HomeController[0]
      User admin@blogmvc.com logged in successfully.
```

### Admin Credentials (Post-Fix)
- **Email**: admin@blogmvc.com
- **Password**: Admin123!
- **Role**: Admin
- **Status**: Active and verified working

---

**Previous Commits**: 
- Initial database setup with seed data
- Various schema and data corrections