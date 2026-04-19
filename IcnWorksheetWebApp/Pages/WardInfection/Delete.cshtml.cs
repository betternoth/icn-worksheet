using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Data;

namespace IcnWorksheet.Pages.WardInfection;

public class DeleteModel : PageModel
{
    private readonly ILogger<DeleteModel> _logger;
    private readonly IWardInfectionRepository _wardInfectionRepository;

    public DeleteModel(
        ILogger<DeleteModel> logger,
        IWardInfectionRepository wardInfectionRepository)
    {
        _logger = logger;
        _wardInfectionRepository = wardInfectionRepository;
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        try
        {
            // Fetch the record to verify it exists
            var record = await _wardInfectionRepository.GetByIdAsync(id);
            if (record == null)
            {
                _logger.LogWarning("Ward infection record with id: {Id} not found for deletion", id);
                TempData["Error"] = "Ward infection record not found.";
                return RedirectToPage("Index");
            }

            // Delete the record
            await _wardInfectionRepository.DeleteAsync(id);
            await _wardInfectionRepository.SaveChangesAsync();

            _logger.LogInformation("Ward infection record with id: {Id} deleted successfully", id);
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
