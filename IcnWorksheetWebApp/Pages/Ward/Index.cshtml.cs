using Microsoft.AspNetCore.Mvc.RazorPages;
using IcnWorksheet.Data;
using IcnWorksheet.Models;

namespace IcnWorksheet.Pages.Ward;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IWardRepository _wardRepository;

    public IndexModel(ILogger<IndexModel> logger, IWardRepository wardRepository)
    {
        _logger = logger;
        _wardRepository = wardRepository;
    }

    public List<WardDto>? Wards { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var wards = await _wardRepository.GetAllAsync();
            Wards = wards.Select(w => new WardDto
            {
                Id = w.Id,
                Name = w.Name,
                Group = w.Group,
                Type = w.Type
            }).OrderBy(w => w.Name).ToList();

            _logger.LogInformation("Loaded {WardCount} wards", Wards.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading wards");
            Wards = new List<WardDto>();
        }
    }
}
