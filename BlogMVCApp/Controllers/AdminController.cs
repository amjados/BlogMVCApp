using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogMVCApp.Models;
using BlogMVCApp.Data;
using System.Security.Claims;

namespace BlogMVCApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ApplicationDbContext context,
            ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
        }

        // User Management
        public async Task<IActionResult> UserManagement()
        {
            try
            {
                var users = await _userManager.Users
                    .Select(u => new
                    {
                        u.Id,
                        u.UserName,
                        u.Email,
                        u.FirstName,
                        u.LastName,
                        u.CreatedAt,
                        u.LockoutEnabled,
                        u.LockoutEnd,
                        u.EmailConfirmed
                    })
                    .ToListAsync();

                return View(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading users");
                TempData["Error"] = "An error occurred while loading users.";
                return View(new List<object>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();

            ViewBag.User = user;
            ViewBag.UserRoles = userRoles;
            ViewBag.UserClaims = userClaims;
            ViewBag.AllRoles = allRoles;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(string userId, string firstName, string lastName,
            string email, bool lockoutEnabled, List<string> selectedRoles)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                // Update user properties
                user.FirstName = firstName?.Trim();
                user.LastName = lastName?.Trim();
                user.Email = email?.Trim();
                user.LockoutEnabled = lockoutEnabled;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
                    return RedirectToAction("EditUser", new { id = userId });
                }

                // Update user roles
                var currentRoles = await _userManager.GetRolesAsync(user);
                selectedRoles = selectedRoles ?? new List<string>();

                var rolesToAdd = selectedRoles.Except(currentRoles);
                var rolesToRemove = currentRoles.Except(selectedRoles);

                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                await _userManager.AddToRolesAsync(user, rolesToAdd);

                TempData["Success"] = "User updated successfully.";
                return RedirectToAction("UserManagement");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", userId);
                TempData["Error"] = "An error occurred while updating the user.";
                return RedirectToAction("EditUser", new { id = userId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockoutUser(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
                TempData["Success"] = $"User {user.Email} has been locked out.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error locking out user {UserId}", userId);
                TempData["Error"] = "An error occurred while locking out the user.";
            }

            return RedirectToAction("UserManagement");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnlockUser(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                await _userManager.SetLockoutEndDateAsync(user, null);
                TempData["Success"] = $"User {user.Email} has been unlocked.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unlocking user {UserId}", userId);
                TempData["Error"] = "An error occurred while unlocking the user.";
            }

            return RedirectToAction("UserManagement");
        }

        // Role Management
        public async Task<IActionResult> RoleManagement()
        {
            try
            {
                var roles = await _roleManager.Roles
                    .Select(r => new
                    {
                        r.Id,
                        r.Name,
                        r.Description,
                        r.CreatedAt,
                        UserCount = _userManager.GetUsersInRoleAsync(r.Name!).Result.Count
                    })
                    .ToListAsync();

                return View(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading roles");
                TempData["Error"] = "An error occurred while loading roles.";
                return View(new List<object>());
            }
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string roleName, string description)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roleName))
                {
                    TempData["Error"] = "Role name is required.";
                    return View();
                }

                var role = new ApplicationRole
                {
                    Name = roleName.Trim(),
                    Description = description?.Trim(),
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    TempData["Success"] = $"Role '{roleName}' created successfully.";
                    return RedirectToAction("RoleManagement");
                }

                TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role {RoleName}", roleName);
                TempData["Error"] = "An error occurred while creating the role.";
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role == null)
                {
                    return NotFound();
                }

                // Check if role is in use
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
                if (usersInRole.Any())
                {
                    TempData["Error"] = $"Cannot delete role '{role.Name}' because it is assigned to {usersInRole.Count} user(s).";
                    return RedirectToAction("RoleManagement");
                }

                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    TempData["Success"] = $"Role '{role.Name}' deleted successfully.";
                }
                else
                {
                    TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting role {RoleId}", roleId);
                TempData["Error"] = "An error occurred while deleting the role.";
            }

            return RedirectToAction("RoleManagement");
        }

        // Claims Management
        public async Task<IActionResult> ClaimsManagement(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound();
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                var userClaims = await _userManager.GetClaimsAsync(user);

                ViewBag.User = user;
                ViewBag.UserClaims = userClaims;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading claims for user {UserId}", userId);
                TempData["Error"] = "An error occurred while loading user claims.";
                return RedirectToAction("UserManagement");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClaim(string userId, string claimType, string claimValue)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(claimType) || string.IsNullOrWhiteSpace(claimValue))
                {
                    TempData["Error"] = "Claim type and value are required.";
                    return RedirectToAction("ClaimsManagement", new { userId });
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                var claim = new Claim(claimType.Trim(), claimValue.Trim());
                var result = await _userManager.AddClaimAsync(user, claim);

                if (result.Succeeded)
                {
                    TempData["Success"] = $"Claim '{claimType}' added successfully.";
                }
                else
                {
                    TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding claim to user {UserId}", userId);
                TempData["Error"] = "An error occurred while adding the claim.";
            }

            return RedirectToAction("ClaimsManagement", new { userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveClaim(string userId, string claimType, string claimValue)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                var claim = new Claim(claimType, claimValue);
                var result = await _userManager.RemoveClaimAsync(user, claim);

                if (result.Succeeded)
                {
                    TempData["Success"] = $"Claim '{claimType}' removed successfully.";
                }
                else
                {
                    TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing claim from user {UserId}", userId);
                TempData["Error"] = "An error occurred while removing the claim.";
            }

            return RedirectToAction("ClaimsManagement", new { userId });
        }

        // Demo action with explicit attributes for educational purposes
        [Authorize(Roles = "Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult TestAdminFilters()
        {
            ViewBag.Message = "This is a demo action showing explicit authorization and cache attributes";
            ViewBag.UserName = User.Identity?.Name;
            ViewBag.Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return View("TestDemo");
        }
    }
}