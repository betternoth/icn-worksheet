using IcnWorksheet.Domain;
using Microsoft.EntityFrameworkCore;

namespace IcnWorksheet.Data;

/// <summary>
/// Repository implementation for Ward entity.
/// Provides database operations for Ward management.
/// </summary>
public class WardRepository : Repository<Ward>, IWardRepository
{
    private readonly ApplicationDbContext _context;

    public WardRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// Get ward by name
    /// </summary>
    public async Task<Ward?> GetByNameAsync(string name)
    {
        return await _context.Wards
            .FirstOrDefaultAsync(w => w.Name.ToLower() == name.ToLower());
    }

    /// <summary>
    /// Check if Ward name already exists
    /// </summary>
    public async Task<bool> NameExistsAsync(string name)
    {
        return await _context.Wards
            .AnyAsync(w => w.Name.ToLower() == name.ToLower());
    }

    /// <summary>
    /// Search wards by name or group
    /// </summary>
    public async Task<IEnumerable<Ward>> SearchAsync(string searchTerm)
    {
        var term = searchTerm.ToLower();
        return await _context.Wards
            .Where(w => w.Name.ToLower().Contains(term) || 
                        w.Group.ToLower().Contains(term))
            .OrderBy(w => w.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Get all wards by group
    /// </summary>
    public async Task<IEnumerable<Ward>> GetByGroupAsync(string group)
    {
        return await _context.Wards
            .Where(w => w.Group == group)
            .OrderBy(w => w.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Get all wards by type
    /// </summary>
    public async Task<IEnumerable<Ward>> GetByTypeAsync(string type)
    {
        return await _context.Wards
            .Where(w => w.Type == type)
            .OrderBy(w => w.Name)
            .ToListAsync();
    }
}
