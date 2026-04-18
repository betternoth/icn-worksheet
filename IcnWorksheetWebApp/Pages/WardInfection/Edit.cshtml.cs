using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Models;

namespace IcnWorksheet.Pages.WardInfection;

public class EditModel : PageModel
{
    private readonly ILogger<EditModel> _logger;

    public EditModel(ILogger<EditModel> logger)
    {
        _logger = logger;
    }

    [BindProperty]
    public WardInfectionSurveillanceDto Input { get; set; } = new();

    public IActionResult OnGet(int id)
    {
        try
        {
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

    public IActionResult OnPost(int id)
    {
        if (!ModelState.IsValid)
        {
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
