# Screenshot Collection Script for BlogMVCApp (PowerShell)
# This script helps you systematically capture screenshots of your application

Write-Host "📸 BlogMVCApp Screenshot Collection Script" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

# Check if application is running
Write-Host "🔍 Checking if application is running on https://localhost:5001..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "https://localhost:5001" -UseBasicParsing -SkipCertificateCheck -TimeoutSec 5
    Write-Host "✅ Application is running!" -ForegroundColor Green
} catch {
    Write-Host "❌ Application is not running. Please start it with:" -ForegroundColor Red
    Write-Host "   cd BlogMVCApp" -ForegroundColor Yellow
    Write-Host "   dotnet run" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Then run this script again." -ForegroundColor Yellow
    exit 1
}

Write-Host ""
Write-Host "📋 Screenshot Collection Checklist" -ForegroundColor Cyan
Write-Host "==================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Please visit each URL and take a screenshot:" -ForegroundColor White
Write-Host ""

# Home & Landing Pages
Write-Host "🏠 HOME & LANDING PAGES" -ForegroundColor Green
Write-Host "----------------------"
Write-Host "1. Homepage: https://localhost:5001/" -ForegroundColor White
Write-Host "   → Save as: docs/screenshots/home/homepage.png" -ForegroundColor Gray
Write-Host ""
Write-Host "2. Privacy: https://localhost:5001/Home/Privacy" -ForegroundColor White
Write-Host "   → Save as: docs/screenshots/home/privacy.png" -ForegroundColor Gray
Write-Host ""
Write-Host "3. Landing Page: https://localhost:5001/Home/LandPage" -ForegroundColor White
Write-Host "   → Save as: docs/screenshots/home/landing-page.png" -ForegroundColor Gray
Write-Host ""

# Authentication
Write-Host "🔐 AUTHENTICATION" -ForegroundColor Blue
Write-Host "----------------"
Write-Host "4. Login: https://localhost:5001/Home/Login" -ForegroundColor White
Write-Host "   → Save as: docs/screenshots/auth/login.png" -ForegroundColor Gray
Write-Host ""
Write-Host "5. User Stats: https://localhost:5001/Home/ViewStats (requires login)" -ForegroundColor White
Write-Host "   → Save as: docs/screenshots/auth/user-stats.png" -ForegroundColor Gray
Write-Host ""

# Blog Features
Write-Host "📝 BLOG FEATURES" -ForegroundColor Magenta
Write-Host "--------------"
Write-Host "6. Write Post: https://localhost:5001/Home/WritePost (requires login)" -ForegroundColor White
Write-Host "   → Save as: docs/screenshots/blog/write-post.png" -ForegroundColor Gray
Write-Host ""
Write-Host "7. View All Posts: https://localhost:5001/Home/ViewAllPosts (requires login)" -ForegroundColor White
Write-Host "   → Save as: docs/screenshots/blog/all-posts.png" -ForegroundColor Gray
Write-Host ""
Write-Host "8. Preview Post: https://localhost:5001/Home/PreviewPost" -ForegroundColor White
Write-Host "   → Save as: docs/screenshots/blog/preview-post.png" -ForegroundColor Gray
Write-Host ""

# Admin Panel
Write-Host "👑 ADMIN PANEL (requires admin role)" -ForegroundColor Red
Write-Host "----------------------------------"
Write-Host "9. User Management: https://localhost:5001/Admin/UserManagement" -ForegroundColor White
Write-Host "   → Save as: docs/screenshots/admin/user-management.png" -ForegroundColor Gray
Write-Host ""
Write-Host "10. Role Management: https://localhost:5001/Admin/RoleManagement" -ForegroundColor White
Write-Host "    → Save as: docs/screenshots/admin/role-management.png" -ForegroundColor Gray
Write-Host ""

# API & Features
Write-Host "🔧 API & SPECIAL FEATURES" -ForegroundColor DarkYellow
Write-Host "------------------------"
Write-Host "11. API Info: https://localhost:5001/api/info" -ForegroundColor White
Write-Host "    → Save as: docs/screenshots/features/api-response.png" -ForegroundColor Gray
Write-Host ""
Write-Host "12. Caching Demo: https://localhost:5001/Home/TestCaching" -ForegroundColor White
Write-Host "    → Save as: docs/screenshots/features/caching-demo.png" -ForegroundColor Gray
Write-Host ""
Write-Host "13. Rate Limiting: https://localhost:5001/Home/TestRateLimit" -ForegroundColor White
Write-Host "    → Save as: docs/screenshots/features/rate-limiting.png" -ForegroundColor Gray
Write-Host ""

# Mobile Views
Write-Host "📱 RESPONSIVE DESIGN" -ForegroundColor DarkCyan
Write-Host "------------------"
Write-Host "14. Mobile Home (use browser dev tools, 375px width)" -ForegroundColor White
Write-Host "    → Save as: docs/screenshots/responsive/mobile-home.png" -ForegroundColor Gray
Write-Host ""
Write-Host "15. Tablet Blog (use browser dev tools, 768px width)" -ForegroundColor White
Write-Host "    → Save as: docs/screenshots/responsive/tablet-blog.png" -ForegroundColor Gray
Write-Host ""
Write-Host "16. Desktop Admin (use browser dev tools, 1200px+ width)" -ForegroundColor White
Write-Host "    → Save as: docs/screenshots/responsive/desktop-admin.png" -ForegroundColor Gray
Write-Host ""

Write-Host "💡 TIPS FOR TAKING SCREENSHOTS" -ForegroundColor Yellow
Write-Host "=============================="
Write-Host "• Use incognito/private mode for clean screenshots" -ForegroundColor White
Write-Host "• Set browser zoom to 100%" -ForegroundColor White
Write-Host "• Use browser dev tools (F12) for responsive screenshots" -ForegroundColor White
Write-Host "• Crop screenshots to remove unnecessary browser chrome" -ForegroundColor White
Write-Host "• Use meaningful sample data, not 'test' or 'lorem ipsum'" -ForegroundColor White
Write-Host "• Show different user states (logged in vs logged out)" -ForegroundColor White
Write-Host ""

Write-Host "🚀 AFTER TAKING SCREENSHOTS" -ForegroundColor Green
Write-Host "==========================="
Write-Host "1. Verify all images are in the correct folders" -ForegroundColor White
Write-Host "2. Run: git add docs/screenshots/" -ForegroundColor Yellow
Write-Host "3. Run: git commit -m 'Add UI screenshots'" -ForegroundColor Yellow
Write-Host "4. Run: git push" -ForegroundColor Yellow
Write-Host "5. Update README.md with screenshot links" -ForegroundColor White
Write-Host ""

Write-Host "✅ Happy screenshotting! 📸" -ForegroundColor Green

# Open the application in browser
Write-Host ""
Write-Host "🌐 Opening application in your default browser..." -ForegroundColor Cyan
Start-Process "https://localhost:5001"