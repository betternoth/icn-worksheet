using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Data;

namespace IcnWorksheet.Pages.Patient;

using PatientEntity = IcnWorksheet.Domain.Patient;

public class DeleteModel : PageModel
{
    private readonly ILogger<DeleteModel> _logger;
    private readonly IPatientRepository _patientRepository;

    public DeleteModel(ILogger<DeleteModel> logger, IPatientRepository patientRepository)
    {
        _logger = logger;
        _patientRepository = patientRepository;
    }

    public PatientEntity? Patient { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            Patient = await _patientRepository.GetByIdAsync(id);
            if (Patient == null)
            {
                _logger.LogWarning("Patient not found for deletion with id: {Id}", id);
                TempData["Error"] = "Patient not found.";
                return RedirectToPage("Index");
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading patient for deletion with id: {Id}", id);
            TempData["Error"] = "An error occurred while loading the patient.";
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        try
        {
            // Fetch patient to verify it exists
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                _logger.LogWarning("Patient not found for deletion with id: {Id}", id);
                TempData["Error"] = "Patient not found.";
                return RedirectToPage("Index");
            }

            var patientName = patient.GetFullName();
            var hospitalNumber = patient.HospitalNumber;

            // Delete patient from database
            await _patientRepository.DeleteAsync(id);
            await _patientRepository.SaveChangesAsync();

            _logger.LogInformation("Deleted patient: {PatientName} (HN: {HN})", 
                patientName, 
                hospitalNumber);

            TempData["Success"] = $"Patient {patientName} deleted successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting patient with id: {Id}", id);
            TempData["Error"] = "An error occurred while deleting the patient.";
            return RedirectToPage("Index");
        }
    }
}
