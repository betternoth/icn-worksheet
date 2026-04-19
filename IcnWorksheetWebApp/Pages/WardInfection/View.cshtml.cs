using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Data;
using IcnWorksheet.Models;

namespace IcnWorksheet.Pages.WardInfection;

public class ViewModel : PageModel
{
    private readonly ILogger<ViewModel> _logger;
    private readonly IWardInfectionRepository _wardInfectionRepository;
    private readonly IPatientRepository _patientRepository;

    public ViewModel(
        ILogger<ViewModel> logger,
        IWardInfectionRepository wardInfectionRepository,
        IPatientRepository patientRepository)
    {
        _logger = logger;
        _wardInfectionRepository = wardInfectionRepository;
        _patientRepository = patientRepository;
    }

    public WardInfectionSurveillanceDto? Record { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
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
            Record = new WardInfectionSurveillanceDto
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
            _logger.LogError(ex, "Error retrieving ward infection record with id: {Id}", id);
            return Page();
        }
    }
}
