using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlogMVCApp.Data.Entities;

public partial class AuditLog
{
    [Key]
    public int Id { get; set; }

    [StringLength(450)]
    public string? UserId { get; set; }

    [StringLength(100)]
    public string Action { get; set; } = null!;

    [StringLength(100)]
    public string? EntityType { get; set; }

    [StringLength(50)]
    public string? EntityId { get; set; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    [StringLength(45)]
    public string? IpAddress { get; set; }

    [StringLength(1000)]
    public string? UserAgent { get; set; }

    public DateTime Timestamp { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("AuditLogs")]
    public virtual AspNetUser? User { get; set; }
}
