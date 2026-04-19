namespace IcnWorksheet.Domain;

/// <summary>
/// Domain entity for Patient management
/// Thai: บันทึกข้อมูลผู้ป่วย
/// </summary>
public class Patient : Entity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string HospitalNumber { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public int? Age { get; set; }

    /// <summary>Full Name (Computed)</summary>
    public string GetFullName() => $"{FirstName} {LastName}".Trim();
}
