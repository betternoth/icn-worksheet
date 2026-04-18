using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Data;
using IcnWorksheet.Models;

namespace IcnWorksheet.Pages.WardInfection;

public class EditModel : PageModel
{
    private readonly ILogger<EditModel> _logger;
    private readonly IWardRepository _wardRepository;

    public EditModel(ILogger<EditModel> logger, IWardRepository wardRepository)
    {
        _logger = logger;
        _wardRepository = wardRepository;
    }

    [BindProperty]
    public WardInfectionSurveillanceDto Input { get; set; } = new();

    public List<string> WardNames { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            // Load ward names for autocomplete
            var wards = await _wardRepository.GetAllAsync();
            WardNames = wards.Select(w => w.Name).OrderBy(n => n).ToList();

            // TODO: Integrate with actual service layer to fetch record by id
            _logger.LogInformation("Loading ward infection record with id: {Id} for edit", id);
            return NotFound();
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
            // Reload ward names if validation fails
            var wards = await _wardRepository.GetAllAsync();
            WardNames = wards.Select(w => w.Name).OrderBy(n => n).ToList();

            _logger.LogWarning("Model validation failed for Edit Ward Infection Record");
            return Page();
        }

        try
        {
            // TODO: Integrate with actual service layer
            Input.Id = id;
            _logger.LogInformation(
                "Updated Ward Infection Record: {Patient} from {Ward} on {Date}",
                Input.PatientName,
                Input.WardName,
                DateTime.Now
            );

            TempData["Success"] = "Ward infection record updated successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ward infection record");
            ModelState.AddModelError(string.Empty, "An error occurred while updating the record.");
            return Page();
        }
    }
}
