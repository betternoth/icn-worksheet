using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Data;
using IcnWorksheet.Domain;
using IcnWorksheet.Models;

namespace IcnWorksheet.Pages.WardInfection;

public class AddModel : PageModel
{
    private readonly ILogger<AddModel> _logger;
    private readonly IWardRepository _wardRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IWardInfectionRepository _wardInfectionRepository;

    public AddModel(
        ILogger<AddModel> logger,
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

    public async Task OnGetAsync()
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

            // Initialize default values
            Input.Year = DateTime.Now.Year;
            Input.Month = DateTime.Now.Month;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading data for add page");
        }
    }

    public async Task<IActionResult> OnPostAsync()
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

            _logger.LogWarning("Model validation failed for Add Ward Infection Record");
            return Page();
        }

        try
        {
            // Get patient to verify existence and get hospital number
            var patient = await _patientRepository.GetByIdAsync(Input.PatientId);
            if (patient == null)
            {
                ModelState.AddModelError(string.Empty, "Selected patient not found.");
                return Page();
            }

            // Create new ward infection surveillance record
            var wardInfectionRecord = new WardInfectionSurveillance
            {
                WardName = Input.WardName,
                Month = Input.Month,
                Year = Input.Year,
                PatientId = Input.PatientId,
                AdmissionNumber = Input.AdmissionNumber,
                Gender = Input.Gender,
                Age = Input.Age,
                AdmissionDate = Input.AdmissionDate,
                TransferOutDate1 = Input.TransferOutDate1,
                TransferInDate1 = Input.TransferInDate1,
                DischargeDate = Input.DischargeDate,
                DischargeType = Input.DischargeType,
                Diagnosis = Input.Diagnosis,
                InfectionSite = Input.InfectionSite,
                SpecimenType = Input.SpecimenType,
                SpecimenSubmissionDate = Input.SpecimenSubmissionDate,
                CultureResult = Input.CultureResult,
                RiskFactor = Input.RiskFactor,
                InfectionDate = Input.InfectionDate
            };

            await _wardInfectionRepository.AddAsync(wardInfectionRecord);
            await _wardInfectionRepository.SaveChangesAsync();

            _logger.LogInformation(
                "New Ward Infection Record created for patient {PatientId}: {PatientName} from {Ward}",
                Input.PatientId,
                patient.GetFullName(),
                Input.WardName
            );

            TempData["Success"] = "Ward infection record added successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding ward infection record");
            ModelState.AddModelError(string.Empty, "An error occurred while saving the record.");
            return Page();
        }
    }
}

