using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Models;

namespace IcnWorksheet.Pages.WardInfection;

public class AddModel : PageModel
{
    private readonly ILogger<AddModel> _logger;

    public AddModel(ILogger<AddModel> logger)
    {
        _logger = logger;
    }

    [BindProperty]
    public WardInfectionSurveillanceDto Input { get; set; } = new();

    public IActionResult OnGet()
    {
        // Initialize default values
        Input.Year = DateTime.Now.Year;
        Input.Month = DateTime.Now.Month;
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
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
