using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlogMVCApp.Data.Entities;

[PrimaryKey("UserId", "RoleId")]
public partial class AspNetUserRole
{
    [Key]
    public string UserId { get; set; } = null!;

    [Key]
    public string RoleId { get; set; } = null!;

    public DateTime AssignedAt { get; set; }

    [StringLength(450)]
    public string? AssignedBy { get; set; }

    [ForeignKey("AssignedBy")]
    [InverseProperty("AspNetUserRoleAssignedByNavigations")]
    public virtual AspNetUser? AssignedByNavigation { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("AspNetUserRoles")]
    public virtual AspNetRole Role { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("AspNetUserRoleUsers")]
    public virtual AspNetUser User { get; set; } = null!;
}
