using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IcnWorksheet.Pages.WardInfection;

public class DeleteModel : PageModel
{
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(ILogger<DeleteModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnPost(int id)
    {
        try
        {
            // TODO: Integrate with actual service layer to delete record by id
            _logger.LogInformation("Deleting ward infection record with id: {Id}", id);

            TempData["Success"] = "Ward infection record deleted successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ward infection record with id: {Id}", id);
            TempData["Error"] = "An error occurred while deleting the record.";
            return RedirectToPage("Index");
        }
    }
}
