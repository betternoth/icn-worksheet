namespace IcnWorksheet.Domain;

/// <summary>
/// Domain entity for Ward Infection Surveillance records
/// Thai: บันทึกการติดเชื้อในหอผู้ป่วย
/// </summary>
public class WardInfectionSurveillance : Entity
{
    public string WardName { get; set; } = string.Empty;
    public int Month { get; set; }
    public int Year { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string HospitalNumber { get; set; } = string.Empty;
    public string AdmissionNumber { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public int Age { get; set; }
    public DateTime? AdmissionDate { get; set; }
    public DateTime? TransferOutDate1 { get; set; }
    public DateTime? TransferInDate1 { get; set; }
    public DateTime? DischargeDate { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string InfectionSite { get; set; } = string.Empty;
    public string SpecimenType { get; set; } = string.Empty;
    public DateTime? SpecimenSubmissionDate { get; set; }
    public string CultureResult { get; set; } = string.Empty;
    public string RiskFactor { get; set; } = string.Empty;
    public DateTime? InfectionDate { get; set; }
}
