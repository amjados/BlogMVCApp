using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlogMVCApp.Data.Entities;

[Index("Slug", Name = "UQ__Categori__BC7B5FB68F5C876C", IsUnique = true)]
public partial class Category
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string Slug { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    [StringLength(450)]
    public string CreatedBy { get; set; } = null!;

    public bool IsActive { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("Categories")]
    public virtual AspNetUser CreatedByNavigation { get; set; } = null!;

    [InverseProperty("Category")]
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
