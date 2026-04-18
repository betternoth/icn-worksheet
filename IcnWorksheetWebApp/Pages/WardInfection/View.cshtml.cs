using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Models;

namespace IcnWorksheet.Pages.WardInfection;

public class ViewModel : PageModel
{
    private readonly ILogger<ViewModel> _logger;

    public ViewModel(ILogger<ViewModel> logger)
    {
        _logger = logger;
    }

    public WardInfectionSurveillanceDto? Record { get; set; }

    public IActionResult OnGet(int id)
    {
        try
        {
            // TODO: Integrate with actual service layer to fetch record by id
            // For now, return Not Found
            _logger.LogInformation("Fetching ward infection record with id: {Id}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving ward infection record with id: {Id}", id);
            return Page();
        }
    }
}
