namespace IcnWorksheet.Models;

/// <summary>
/// Request model for creating a new patient from Ward Infection form
/// </summary>
public class CreatePatientRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string HospitalNumber { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public int? Age { get; set; }
}
