using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Models;

namespace IcnWorksheet.Pages.WardInfection;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public List<WardInfectionSurveillanceDto>? Records { get; set; }

    public void OnGet()
    {
        try
        {
            // TODO: Integrate with actual service layer to fetch records
            // For now, return empty list
            Records = new List<WardInfectionSurveillanceDto>();
            _logger.LogInformation("Retrieved ward infection records");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving ward infection records");
            Records = new List<WardInfectionSurveillanceDto>();
        }
    }
}
