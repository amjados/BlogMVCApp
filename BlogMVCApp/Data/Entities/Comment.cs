using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlogMVCApp.Data.Entities;

[Index("AuthorId", Name = "IX_Comments_AuthorId")]
[Index("PostId", Name = "IX_Comments_PostId")]
public partial class Comment
{
    [Key]
    public int Id { get; set; }

    public int PostId { get; set; }

    public string? AuthorId { get; set; }

    [StringLength(100)]
    public string AuthorName { get; set; } = null!;

    [StringLength(256)]
    public string? AuthorEmail { get; set; }

    [StringLength(1000)]
    public string Content { get; set; } = null!;

    public int? ParentCommentId { get; set; }

    public bool IsApproved { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [StringLength(45)]
    public string? IpAddress { get; set; }

    [ForeignKey("AuthorId")]
    [InverseProperty("Comments")]
    public virtual AspNetUser? Author { get; set; }

    [InverseProperty("ParentComment")]
    public virtual ICollection<Comment> InverseParentComment { get; set; } = new List<Comment>();

    [ForeignKey("ParentCommentId")]
    [InverseProperty("InverseParentComment")]
    public virtual Comment? ParentComment { get; set; }

    [ForeignKey("PostId")]
    [InverseProperty("Comments")]
    public virtual Post Post { get; set; } = null!;
}
