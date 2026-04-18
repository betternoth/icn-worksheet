using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Models;
using IcnWorksheet.Data;
using PatientEntity = IcnWorksheet.Domain.Patient;

namespace IcnWorksheet.Pages.Patient;

public class EditModel : PageModel
{
    private readonly ILogger<EditModel> _logger;
    private readonly IPatientRepository _patientRepository;

    public EditModel(ILogger<EditModel> logger, IPatientRepository patientRepository)
    {
        _logger = logger;
        _patientRepository = patientRepository;
    }

    [BindProperty]
    public PatientDto Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            // Fetch patient from database
            var patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
            {
                _logger.LogWarning("Patient not found for edit with id: {Id}", id);
                return NotFound();
            }

            // Map to DTO
            Input = new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                HospitalNumber = patient.HospitalNumber
            };

            _logger.LogInformation("Loading patient with id: {Id} for edit", id);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading patient with id: {Id}", id);
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Model validation failed for Edit Patient");
            return Page();
        }

        try
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(Input.FirstName))
            {
                ModelState.AddModelError("Input.FirstName", "First Name is required.");
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Input.LastName))
            {
                ModelState.AddModelError("Input.LastName", "Last Name is required.");
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Input.HospitalNumber))
            {
                ModelState.AddModelError("Input.HospitalNumber", "Hospital Number is required.");
                return Page();
            }

            // Fetch existing patient
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                ModelState.AddModelError(string.Empty, "Patient not found.");
                return Page();
            }

            // Check if HN is being changed and if it already exists
            if (patient.HospitalNumber != Input.HospitalNumber)
            {
                var existingPatient = await _patientRepository.GetByHospitalNumberAsync(Input.HospitalNumber);
                if (existingPatient != null)
                {
                    ModelState.AddModelError("Input.HospitalNumber", $"Hospital Number '{Input.HospitalNumber}' already exists.");
                    return Page();
                }
            }

            // Update patient entity
            patient.FirstName = Input.FirstName.Trim();
            patient.LastName = Input.LastName.Trim();
            patient.HospitalNumber = Input.HospitalNumber.Trim();
            patient.UpdatedAt = DateTime.UtcNow;

            // Save to database
            await _patientRepository.UpdateAsync(patient);
            await _patientRepository.SaveChangesAsync();

            _logger.LogInformation(
                "Updated Patient: {PatientName} (HN: {HN})",
                patient.GetFullName(),
                patient.HospitalNumber
            );

            TempData["Success"] = $"Patient {patient.GetFullName()} updated successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient");
            ModelState.AddModelError(string.Empty, "An error occurred while updating the patient.");
            return Page();
        }
    }
}
