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

    /// <summary>Full Name (Computed)</summary>
    public string GetFullName() => $"{FirstName} {LastName}".Trim();
}
