namespace IcnWorksheet.Models;

/// <summary>
/// Data Transfer Object for Ward Infection Surveillance reporting
/// Thai: รายงานการติดเชื้อในหอผู้ป่วย
/// </summary>
public class WardInfectionSurveillanceDto : BaseDto
{
    /// <summary>ชื่อ Ward (Ward Name)</summary>
    public string WardName { get; set; } = string.Empty;

    /// <summary>เดือน (Month)</summary>
    public int Month { get; set; }

    /// <summary>ปี (Year)</summary>
    public int Year { get; set; }

    /// <summary>Patient ID (Foreign Key)</summary>
    public int PatientId { get; set; }

    /// <summary>ชื่อผู้ป่วย (First Name)</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>นามสกุล (Last Name)</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>Full Name (Computed)</summary>
    public string PatientName => $"{FirstName} {LastName}".Trim();

    /// <summary>AN (Admission Number)</summary>
    public string AdmissionNumber { get; set; } = string.Empty;

    /// <summary>เพศ (Gender)</summary>
    public string Gender { get; set; } = string.Empty;

    /// <summary>อายุ (Age)</summary>
    public int Age { get; set; }

    /// <summary>วันที่ Admit (Admission Date)</summary>
    public DateTime? AdmissionDate { get; set; }

    /// <summary>วันที่รับย้ายออก Ward ลำดับ 1 (Transfer Out Date - First)</summary>
    public DateTime? TransferOutDate1 { get; set; }

    /// <summary>วันที่รับย้ายเข้า Ward ลำดับ 1 (Transfer In Date - First)</summary>
    public DateTime? TransferInDate1 { get; set; }

    /// <summary>วันที่ D/C (Discharge Date)</summary>
    public DateTime? DischargeDate { get; set; }

    /// <summary>ประเภทการ D/C (Discharge Type) - Alive or Dead</summary>
    public string DischargeType { get; set; } = string.Empty;

    /// <summary>Dx (Diagnosis)</summary>
    public string Diagnosis { get; set; } = string.Empty;

    /// <summary>Site (Infection Site)</summary>
    public string InfectionSite { get; set; } = string.Empty;

    /// <summary>ประเภทสิ่งส่งตรวจ (Type of Specimen)</summary>
    public string SpecimenType { get; set; } = string.Empty;

    /// <summary>วันที่ส่งตรวจ (Specimen Submission Date)</summary>
    public DateTime? SpecimenSubmissionDate { get; set; }

    /// <summary>ผลเพาะเชื้อ (Culture Result)</summary>
    public string CultureResult { get; set; } = string.Empty;

    /// <summary>ปัจจัย (Risk Factor)</summary>
    public string RiskFactor { get; set; } = string.Empty;

    /// <summary>วันที่ติดเชื้อ (Infection Date)</summary>
    public DateTime? InfectionDate { get; set; }
}
