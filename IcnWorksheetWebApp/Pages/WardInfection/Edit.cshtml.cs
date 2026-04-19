using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Data;
using IcnWorksheet.Models;

namespace IcnWorksheet.Pages.WardInfection;

public class EditModel : PageModel
{
    private readonly ILogger<EditModel> _logger;
    private readonly IWardRepository _wardRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IWardInfectionRepository _wardInfectionRepository;

    public EditModel(
        ILogger<EditModel> logger,
        IWardRepository wardRepository,
        IPatientRepository patientRepository,
        IWardInfectionRepository wardInfectionRepository)
    {
        _logger = logger;
        _wardRepository = wardRepository;
        _patientRepository = patientRepository;
        _wardInfectionRepository = wardInfectionRepository;
    }

    [BindProperty]
    public WardInfectionSurveillanceDto Input { get; set; } = new();

    public List<string> WardNames { get; set; } = new();
    public List<PatientDto> Patients { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            // Load ward names for autocomplete
            var wards = await _wardRepository.GetAllAsync();
            WardNames = wards.Select(w => w.Name).OrderBy(n => n).ToList();

            // Load patients for dropdown
            var patients = await _patientRepository.GetAllAsync();
            Patients = patients.Select(p => new PatientDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                HospitalNumber = p.HospitalNumber
            }).OrderBy(p => p.FirstName).ThenBy(p => p.LastName).ToList();

            // Fetch the ward infection record
            var record = await _wardInfectionRepository.GetByIdAsync(id);
            if (record == null)
            {
                _logger.LogWarning("Ward infection record with id: {Id} not found", id);
                return NotFound();
            }

            // Get patient information
            var patient = await _patientRepository.GetByIdAsync(record.PatientId);
            if (patient == null)
            {
                _logger.LogWarning("Patient with id: {PatientId} not found for ward infection record: {Id}", record.PatientId, id);
                return NotFound();
            }

            // Map to DTO
            Input = new WardInfectionSurveillanceDto
            {
                Id = record.Id,
                WardName = record.WardName,
                Month = record.Month,
                Year = record.Year,
                PatientId = record.PatientId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
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
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading ward infection record with id: {Id}", id);
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            // Reload data if validation fails
            var wards = await _wardRepository.GetAllAsync();
            WardNames = wards.Select(w => w.Name).OrderBy(n => n).ToList();

            var patients = await _patientRepository.GetAllAsync();
            Patients = patients.Select(p => new PatientDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                HospitalNumber = p.HospitalNumber
            }).OrderBy(p => p.FirstName).ThenBy(p => p.LastName).ToList();

            _logger.LogWarning("Model validation failed for Edit Ward Infection Record with id: {Id}", id);
            return Page();
        }

        try
        {
            // Fetch the existing record
            var record = await _wardInfectionRepository.GetByIdAsync(id);
            if (record == null)
            {
                _logger.LogWarning("Ward infection record with id: {Id} not found for update", id);
                return NotFound();
            }

            // Get patient to verify existence and get hospital number
            var patient = await _patientRepository.GetByIdAsync(Input.PatientId);
            if (patient == null)
            {
                ModelState.AddModelError(string.Empty, "Selected patient not found.");
                return Page();
            }

            // Update the record
            record.WardName = Input.WardName;
            record.Month = Input.Month;
            record.Year = Input.Year;
            record.PatientId = Input.PatientId;
            record.AdmissionNumber = Input.AdmissionNumber;
            record.Gender = Input.Gender;
            record.Age = Input.Age;
            record.AdmissionDate = Input.AdmissionDate;
            record.TransferOutDate1 = Input.TransferOutDate1;
            record.TransferInDate1 = Input.TransferInDate1;
            record.TransferFromWard = Input.TransferFromWard;
            record.TransferToWard = Input.TransferToWard;
            record.DischargeDate = Input.DischargeDate;
            record.DischargeType = Input.DischargeType;
            record.Diagnosis = Input.Diagnosis;
            record.InfectionSite = Input.InfectionSite;
            record.SpecimenType = Input.SpecimenType;
            record.SpecimenSubmissionDate = Input.SpecimenSubmissionDate;
            record.CultureResult = Input.CultureResult;
            record.RiskFactor = Input.RiskFactor;
            record.InfectionDate = Input.InfectionDate;

            await _wardInfectionRepository.UpdateAsync(record);
            await _wardInfectionRepository.SaveChangesAsync();

            _logger.LogInformation(
                "Updated Ward Infection Record {Id} for patient {PatientId}: {PatientName}",
                id,
                Input.PatientId,
                patient.GetFullName()
            );

            TempData["Success"] = "Ward infection record updated successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ward infection record with id: {Id}", id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the record.");
            return Page();
        }
    }
}
