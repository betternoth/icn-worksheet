namespace IcnWorksheet.Models;

/// <summary>
/// Data Transfer Object for Patient management
/// Thai: จัดการข้อมูลผู้ป่วย
/// </summary>
public class PatientDto : BaseDto
{
    /// <summary>First Name</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Last Name</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>HN (Hospital Number) - Unique identifier for patient</summary>
    public string HospitalNumber { get; set; } = string.Empty;

    /// <summary>Gender (M/F/Other)</summary>
    public string? Gender { get; set; }

    /// <summary>Age</summary>
    public int? Age { get; set; }

    /// <summary>Full Name (Computed)</summary>
    public string FullName => $"{FirstName} {LastName}".Trim();
}
