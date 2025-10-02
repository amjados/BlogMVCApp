using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlogMVCApp.Data.Entities;

[Index("AuthorId", Name = "IX_Posts_AuthorId")]
[Index("CategoryId", Name = "IX_Posts_CategoryId")]
[Index("PublishedAt", Name = "IX_Posts_PublishedAt")]
[Index("Status", Name = "IX_Posts_Status")]
[Index("Slug", Name = "UQ__Posts__BC7B5FB6857EE75D", IsUnique = true)]
public partial class Post
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    [StringLength(500)]
    public string? Excerpt { get; set; }

    [StringLength(200)]
    public string Slug { get; set; } = null!;

    [StringLength(500)]
    public string? FeaturedImageUrl { get; set; }

    public int? CategoryId { get; set; }

    public string AuthorId { get; set; } = null!;

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public DateTime? PublishedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int ViewCount { get; set; }

    [StringLength(200)]
    public string? MetaTitle { get; set; }

    [StringLength(300)]
    public string? MetaDescription { get; set; }

    [ForeignKey("AuthorId")]
    [InverseProperty("Posts")]
    public virtual AspNetUser Author { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [InverseProperty("Posts")]
    public virtual Category? Category { get; set; }

    [InverseProperty("Post")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [ForeignKey("PostId")]
    [InverseProperty("Posts")]
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
