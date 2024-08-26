using System.ComponentModel.DataAnnotations.Schema;

namespace DatingAppServer.Entities;

/// <summary>
/// "[Table("Photos")]" - As we are not specifically creating a DbSet property for Photo.
/// We use Data Annotation to specify what would be the entity called in DB.
/// </summary>
[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }

    //Navigation Property - Required One-to-many relationship
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
}