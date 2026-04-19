using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Data;
using IcnWorksheet.Models;

namespace IcnWorksheet.Pages.WardInfection;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IWardInfectionRepository _wardInfectionRepository;
    private readonly IPatientRepository _patientRepository;

    public IndexModel(
        ILogger<IndexModel> logger,
        IWardInfectionRepository wardInfectionRepository,
        IPatientRepository patientRepository)
    {
        _logger = logger;
        _wardInfectionRepository = wardInfectionRepository;
        _patientRepository = patientRepository;
    }

    public List<WardInfectionSurveillanceDto>? Records { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            // Fetch all ward infection records
            var records = await _wardInfectionRepository.GetAllAsync();
            Records = new List<WardInfectionSurveillanceDto>();

            foreach (var record in records)
            {
                var patient = await _patientRepository.GetByIdAsync(record.PatientId);
                Records.Add(new WardInfectionSurveillanceDto
                {
                    Id = record.Id,
                    WardName = record.WardName,
                    Month = record.Month,
                    Year = record.Year,
                    FirstName = patient?.FirstName ?? string.Empty,
                    LastName = patient?.LastName ?? string.Empty,
                    AdmissionNumber = record.AdmissionNumber,
                    Gender = record.Gender,
                    Age = record.Age,
                    AdmissionDate = record.AdmissionDate,
                    TransferOutDate1 = record.TransferOutDate1,
                    TransferInDate1 = record.TransferInDate1,
                    TransferFromWard = record.TransferFromWard,
                    TransferToWard = record.TransferToWard,
                    DischargeDate = record.DischargeDate,
                    DischargeType = record.DischargeType,
                    Diagnosis = record.Diagnosis,
                    InfectionSite = record.InfectionSite,
                    SpecimenType = record.SpecimenType,
                    SpecimenSubmissionDate = record.SpecimenSubmissionDate,
                    CultureResult = record.CultureResult,
                    RiskFactor = record.RiskFactor,
                    InfectionDate = record.InfectionDate
                });
            }

            _logger.LogInformation("Retrieved {Count} ward infection records", Records.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving ward infection records");
            Records = new List<WardInfectionSurveillanceDto>();
        }
    }
}
