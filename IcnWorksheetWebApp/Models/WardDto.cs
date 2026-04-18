namespace IcnWorksheet.Models;

/// <summary>
/// Data Transfer Object for Ward management
/// </summary>
public class WardDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    /// <summary>Full Ward Name (Computed)</summary>
    public string FullWardName => $"{Name} ({Group})".Trim();
}
