namespace IcnWorksheet.Domain;

/// <summary>
/// Domain entity for Ward management
/// Thai: บันทึกข้อมูล病แนว
/// </summary>
public class Ward : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    /// <summary>Full Ward Name (Computed)</summary>
    public string GetFullWardName() => $"{Name} ({Group})".Trim();
}
