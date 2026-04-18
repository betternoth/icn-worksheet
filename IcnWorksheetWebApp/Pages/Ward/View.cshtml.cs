using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Data;
using IcnWorksheet.Models;

namespace IcnWorksheet.Pages.Ward;

public class ViewModel : PageModel
{
    private readonly ILogger<ViewModel> _logger;
    private readonly IWardRepository _wardRepository;

    public ViewModel(ILogger<ViewModel> logger, IWardRepository wardRepository)
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
                _logger.LogWarning("Ward not found with id: {Id}", id);
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
            _logger.LogError(ex, "Error loading ward with id: {Id}", id);
            TempData["Error"] = "An error occurred while loading the ward.";
            return RedirectToPage("Index");
        }
    }
}
