using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Data;
using IcnWorksheet.Models;

namespace IcnWorksheet.Pages.WardInfection;

public class AddModel : PageModel
{
    private readonly ILogger<AddModel> _logger;
    private readonly IWardRepository _wardRepository;

    public AddModel(ILogger<AddModel> logger, IWardRepository wardRepository)
    {
        _logger = logger;
        _wardRepository = wardRepository;
    }

    [BindProperty]
    public WardInfectionSurveillanceDto Input { get; set; } = new();

    public List<string> WardNames { get; set; } = new();

    public async Task OnGetAsync()
    {
        try
        {
            // Load ward names for autocomplete
            var wards = await _wardRepository.GetAllAsync();
            WardNames = wards.Select(w => w.Name).OrderBy(n => n).ToList();

            // Initialize default values
            Input.Year = DateTime.Now.Year;
            Input.Month = DateTime.Now.Month;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading wards for autocomplete");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Reload ward names if validation fails
            var wards = await _wardRepository.GetAllAsync();
            WardNames = wards.Select(w => w.Name).OrderBy(n => n).ToList();

            _logger.LogWarning("Model validation failed for Add Ward Infection Record");
            return Page();
        }

        try
        {
            // TODO: Integrate with actual service layer
            // For now, just log the data
            _logger.LogInformation(
                "New Ward Infection Record: {Patient} from {Ward} on {Date}",
                Input.PatientName,
                Input.WardName,
                DateTime.Now
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
