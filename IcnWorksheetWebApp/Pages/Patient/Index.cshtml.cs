using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Models;
using IcnWorksheet.Data;

namespace IcnWorksheet.Pages.Patient;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IPatientRepository _patientRepository;

    public IndexModel(ILogger<IndexModel> logger, IPatientRepository patientRepository)
    {
        _logger = logger;
        _patientRepository = patientRepository;
    }

    public List<PatientDto>? Patients { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            // Fetch all patients from database
            var patients = await _patientRepository.GetAllAsync();

            // Convert to DTOs
            Patients = patients.Select(p => new PatientDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                HospitalNumber = p.HospitalNumber
            }).ToList();

            _logger.LogInformation("Retrieved {Count} patients from database", Patients.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient list");
            Patients = new List<PatientDto>();
        }
    }
}
