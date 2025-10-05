#!/bin/bash

# Screenshot Collection Script for BlogMVCApp
# This script helps you systematically capture screenshots of your application

echo "📸 BlogMVCApp Screenshot Collection Script"
echo "=========================================="
echo ""

# Check if application is running
echo "🔍 Checking if application is running on https://localhost:5001..."
if curl -s -k https://localhost:5001 > /dev/null; then
    echo "✅ Application is running!"
else
    echo "❌ Application is not running. Please start it with:"
    echo "   cd BlogMVCApp && dotnet run"
    echo ""
    echo "Then run this script again."
    exit 1
fi

echo ""
echo "📋 Screenshot Collection Checklist"
echo "=================================="
echo ""
echo "Please visit each URL and take a screenshot:"
echo ""

# Home & Landing Pages
echo "🏠 HOME & LANDING PAGES"
echo "----------------------"
echo "1. Homepage: https://localhost:5001/"
echo "   → Save as: docs/screenshots/home/homepage.png"
echo ""
echo "2. Privacy: https://localhost:5001/Home/Privacy"
echo "   → Save as: docs/screenshots/home/privacy.png"
echo ""
echo "3. Landing Page: https://localhost:5001/Home/LandPage"
echo "   → Save as: docs/screenshots/home/landing-page.png"
echo ""

# Authentication
echo "🔐 AUTHENTICATION"
echo "----------------"
echo "4. Login: https://localhost:5001/Home/Login"
echo "   → Save as: docs/screenshots/auth/login.png"
echo ""
echo "5. User Stats: https://localhost:5001/Home/ViewStats (requires login)"
echo "   → Save as: docs/screenshots/auth/user-stats.png"
echo ""

# Blog Features
echo "📝 BLOG FEATURES"
echo "--------------"
echo "6. Write Post: https://localhost:5001/Home/WritePost (requires login)"
echo "   → Save as: docs/screenshots/blog/write-post.png"
echo ""
echo "7. View All Posts: https://localhost:5001/Home/ViewAllPosts (requires login)"
echo "   → Save as: docs/screenshots/blog/all-posts.png"
echo ""
echo "8. Preview Post: https://localhost:5001/Home/PreviewPost"
echo "   → Save as: docs/screenshots/blog/preview-post.png"
echo ""

# Admin Panel
echo "👑 ADMIN PANEL (requires admin role)"
echo "----------------------------------"
echo "9. User Management: https://localhost:5001/Admin/UserManagement"
echo "   → Save as: docs/screenshots/admin/user-management.png"
echo ""
echo "10. Role Management: https://localhost:5001/Admin/RoleManagement"
echo "    → Save as: docs/screenshots/admin/role-management.png"
echo ""

# API & Features
echo "🔧 API & SPECIAL FEATURES"
echo "------------------------"
echo "11. API Info: https://localhost:5001/api/info"
echo "    → Save as: docs/screenshots/features/api-response.png"
echo ""
echo "12. Caching Demo: https://localhost:5001/Home/TestCaching"
echo "    → Save as: docs/screenshots/features/caching-demo.png"
echo ""
echo "13. Rate Limiting: https://localhost:5001/Home/TestRateLimit"
echo "    → Save as: docs/screenshots/features/rate-limiting.png"
echo ""

# Mobile Views
echo "📱 RESPONSIVE DESIGN"
echo "------------------"
echo "14. Mobile Home (use browser dev tools, 375px width)"
echo "    → Save as: docs/screenshots/responsive/mobile-home.png"
echo ""
echo "15. Tablet Blog (use browser dev tools, 768px width)"
echo "    → Save as: docs/screenshots/responsive/tablet-blog.png"
echo ""
echo "16. Desktop Admin (use browser dev tools, 1200px+ width)"
echo "    → Save as: docs/screenshots/responsive/desktop-admin.png"
echo ""

echo "💡 TIPS FOR TAKING SCREENSHOTS"
echo "=============================="
echo "• Use incognito/private mode for clean screenshots"
echo "• Set browser zoom to 100%"
echo "• Use browser dev tools (F12) for responsive screenshots"
echo "• Crop screenshots to remove unnecessary browser chrome"
echo "• Use meaningful sample data, not 'test' or 'lorem ipsum'"
echo "• Show different user states (logged in vs logged out)"
echo ""

echo "🚀 AFTER TAKING SCREENSHOTS"
echo "==========================="
echo "1. Verify all images are in the correct folders"
echo "2. Run: git add docs/screenshots/"
echo "3. Run: git commit -m 'Add UI screenshots'"
echo "4. Run: git push"
echo "5. Update README.md with screenshot links"
echo ""

echo "✅ Happy screenshotting! 📸"