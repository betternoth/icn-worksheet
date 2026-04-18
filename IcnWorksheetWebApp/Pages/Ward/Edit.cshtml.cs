using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Data;

namespace IcnWorksheet.Pages.Ward;

using WardEntity = IcnWorksheet.Domain.Ward;

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
    public InputModel Input { get; set; } = new();

    public List<string> Groups { get; set; } = new()
    {
        "PCT Med",
        "PCT Surgery",
        "PCT Preditic",
        "PCT ENT",
        "PCT Phychi",
        "PCT Radiation",
        "PCT Eye",
        "PCT OB-Gyne",
        "PCT Ortho",
        "PCT VIP"
    };

    public List<string> Types { get; set; } = new()
    {
        "ICU",
        "General"
    };

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var ward = await _wardRepository.GetByIdAsync(id);
            if (ward == null)
            {
                _logger.LogWarning("Ward not found for editing with id: {Id}", id);
                TempData["Error"] = "Ward not found.";
                return RedirectToPage("Index");
            }

            Input = new InputModel
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
            _logger.LogError(ex, "Error loading ward for editing with id: {Id}", id);
            TempData["Error"] = "An error occurred while loading the ward.";
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var ward = await _wardRepository.GetByIdAsync(Input.Id);
            if (ward == null)
            {
                _logger.LogWarning("Ward not found for update with id: {Id}", Input.Id);
                TempData["Error"] = "Ward not found.";
                return RedirectToPage("Index");
            }

            // Check for duplicate name (if changed)
            if (ward.Name != Input.Name.Trim())
            {
                var existingWard = await _wardRepository.GetByNameAsync(Input.Name.Trim());
                if (existingWard != null)
                {
                    ModelState.AddModelError("Input.Name", "A ward with this name already exists.");
                    _logger.LogWarning("Attempted to update ward with duplicate name: {Name}", Input.Name);
                    return Page();
                }
            }

            ward.Name = Input.Name.Trim();
            ward.Group = Input.Group;
            ward.Type = Input.Type;
            ward.UpdatedAt = DateTime.UtcNow;

            await _wardRepository.UpdateAsync(ward);
            await _wardRepository.SaveChangesAsync();

            _logger.LogInformation("Updated ward: {WardName}", ward.Name);
            TempData["Success"] = $"Ward '{ward.Name}' updated successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ward with id: {Id}", Input.Id);
            TempData["Error"] = "An error occurred while updating the ward.";
            return Page();
        }
    }

    public class InputModel
    {
        public int Id { get; set; }

        [BindProperty]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        public string Group { get; set; } = string.Empty;

        [BindProperty]
        public string Type { get; set; } = string.Empty;
    }
}
