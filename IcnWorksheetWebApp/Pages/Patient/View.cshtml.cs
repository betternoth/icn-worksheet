using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Models;
using IcnWorksheet.Data;

namespace IcnWorksheet.Pages.Patient;

public class ViewModel : PageModel
{
    private readonly ILogger<ViewModel> _logger;
    private readonly IPatientRepository _patientRepository;

    public ViewModel(ILogger<ViewModel> logger, IPatientRepository patientRepository)
    {
        _logger = logger;
        _patientRepository = patientRepository;
    }

    public PatientDto? Patient { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            // Fetch patient from database
            var patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
            {
                _logger.LogWarning("Patient not found with id: {Id}", id);
                return NotFound();
            }

            // Convert to DTO
            Patient = new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                HospitalNumber = patient.HospitalNumber
            };

            _logger.LogInformation("Fetched patient with id: {Id}", id);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient with id: {Id}", id);
            return Page();
        }
    }
}
