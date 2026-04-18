using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Data;
using IcnWorksheet.Models;

namespace IcnWorksheet.Pages.Ward;

public class DeleteModel : PageModel
{
    private readonly ILogger<DeleteModel> _logger;
    private readonly IWardRepository _wardRepository;

    public DeleteModel(ILogger<DeleteModel> logger, IWardRepository wardRepository)
    {
        _logger = logger;
        _wardRepository = wardRepository;
    }

    public WardDto? Ward { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var ward = await _wardRepository.GetByIdAsync(id);
            if (ward == null)
            {
                _logger.LogWarning("Ward not found for deletion with id: {Id}", id);
                TempData["Error"] = "Ward not found.";
                return RedirectToPage("Index");
            }

            Ward = new WardDto
            {
                Id = ward.Id,
                Name = ward.Name,
                Group = ward.Group,
                Type = ward.Type
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading ward for deletion with id: {Id}", id);
            TempData["Error"] = "An error occurred while loading the ward.";
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        try
        {
            var ward = await _wardRepository.GetByIdAsync(id);
            if (ward == null)
            {
                _logger.LogWarning("Ward not found for deletion with id: {Id}", id);
                TempData["Error"] = "Ward not found.";
                return RedirectToPage("Index");
            }

            var wardName = ward.Name;

            await _wardRepository.DeleteAsync(id);
            await _wardRepository.SaveChangesAsync();

            _logger.LogInformation("Deleted ward: {WardName}", wardName);
            TempData["Success"] = $"Ward '{wardName}' deleted successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ward with id: {Id}", id);
            TempData["Error"] = "An error occurred while deleting the ward.";
            return RedirectToPage("Index");
        }
    }
}
