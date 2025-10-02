using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlogMVCApp.Data.Entities;

[Index("Name", Name = "UQ__Tags__737584F680FC65C4", IsUnique = true)]
[Index("Slug", Name = "UQ__Tags__BC7B5FB617B23E3C", IsUnique = true)]
public partial class Tag
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string Slug { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    [ForeignKey("TagId")]
    [InverseProperty("Tags")]
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
