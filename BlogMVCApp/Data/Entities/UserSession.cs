using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlogMVCApp.Data.Entities;

[Index("SessionToken", Name = "UQ__UserSess__46BDD1249B362614", IsUnique = true)]
public partial class UserSession
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(450)]
    public string UserId { get; set; } = null!;

    [StringLength(500)]
    public string SessionToken { get; set; } = null!;

    [StringLength(500)]
    public string? DeviceInfo { get; set; }

    [StringLength(45)]
    public string? IpAddress { get; set; }

    [StringLength(1000)]
    public string? UserAgent { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastAccessedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserSessions")]
    public virtual AspNetUser User { get; set; } = null!;
}
