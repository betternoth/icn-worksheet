using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Data;

namespace IcnWorksheet.Pages.Ward;

using WardEntity = IcnWorksheet.Domain.Ward;

public class AddModel : PageModel
{
    private readonly ILogger<AddModel> _logger;
    private readonly IWardRepository _wardRepository;

    public AddModel(ILogger<AddModel> logger, IWardRepository wardRepository)
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

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Check if ward name already exists
            var existingWard = await _wardRepository.GetByNameAsync(Input.Name.Trim());
            if (existingWard != null)
            {
                ModelState.AddModelError("Input.Name", "A ward with this name already exists.");
                _logger.LogWarning("Attempted to create ward with duplicate name: {Name}", Input.Name);
                return Page();
            }

            var ward = new WardEntity
            {
                Name = Input.Name.Trim(),
                Group = Input.Group,
                Type = Input.Type
            };

            await _wardRepository.AddAsync(ward);
            await _wardRepository.SaveChangesAsync();

            _logger.LogInformation("Created new ward: {WardName} ({Group})", ward.Name, ward.Group);
            TempData["Success"] = $"Ward '{ward.Name}' added successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding new ward");
            TempData["Error"] = "An error occurred while adding the ward.";
            return Page();
        }
    }

    public class InputModel
    {
        [BindProperty]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        public string Group { get; set; } = string.Empty;

        [BindProperty]
        public string Type { get; set; } = string.Empty;
    }
}
