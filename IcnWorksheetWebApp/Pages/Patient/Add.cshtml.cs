using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Models;
using IcnWorksheet.Data;
using PatientEntity = IcnWorksheet.Domain.Patient;

namespace IcnWorksheet.Pages.Patient;

public class AddModel : PageModel
{
    private readonly ILogger<AddModel> _logger;
    private readonly IPatientRepository _patientRepository;

    public AddModel(ILogger<AddModel> logger, IPatientRepository patientRepository)
    {
        _logger = logger;
        _patientRepository = patientRepository;
    }

    [BindProperty]
    public PatientDto Input { get; set; } = new();

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Model validation failed for Add Patient");
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

            // Check for duplicate HN
            var existingPatient = await _patientRepository.GetByHospitalNumberAsync(Input.HospitalNumber);
            if (existingPatient != null)
            {
                ModelState.AddModelError("Input.HospitalNumber", $"Hospital Number '{Input.HospitalNumber}' already exists.");
                return Page();
            }

            // Create new patient entity
            var patient = new PatientEntity
            {
                FirstName = Input.FirstName.Trim(),
                LastName = Input.LastName.Trim(),
                HospitalNumber = Input.HospitalNumber.Trim()
            };

            // Save to database
            await _patientRepository.AddAsync(patient);
            await _patientRepository.SaveChangesAsync();

            _logger.LogInformation(
                "New Patient created: {PatientName} (HN: {HN})",
                patient.GetFullName(),
                patient.HospitalNumber
            );

            TempData["Success"] = $"Patient {patient.GetFullName()} added successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding patient");
            ModelState.AddModelError(string.Empty, "An error occurred while saving the patient.");
            return Page();
        }
    }
}
