namespace IcnWorksheet.Domain;

/// <summary>
/// Base entity class for all domain models.
/// </summary>
public abstract class Entity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
