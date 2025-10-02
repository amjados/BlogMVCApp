using Microsoft.AspNetCore.Identity;

namespace BlogMVCApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            // Set Identity default values that should be true by default
            LockoutEnabled = true;
        }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Biography { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
        public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
        public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

        // Computed property
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}