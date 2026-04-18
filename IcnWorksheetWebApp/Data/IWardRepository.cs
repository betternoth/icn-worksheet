using IcnWorksheet.Domain;

namespace IcnWorksheet.Data;

/// <summary>
/// Repository interface for Ward entity with specific business operations.
/// </summary>
public interface IWardRepository : IRepository<Ward>
{
    /// <summary>
    /// Get ward by name
    /// </summary>
    Task<Ward?> GetByNameAsync(string name);

    /// <summary>
    /// Check if Ward name already exists
    /// </summary>
    Task<bool> NameExistsAsync(string name);

    /// <summary>
    /// Search wards by name or group
    /// </summary>
    Task<IEnumerable<Ward>> SearchAsync(string searchTerm);

    /// <summary>
    /// Get all wards by group
    /// </summary>
    Task<IEnumerable<Ward>> GetByGroupAsync(string group);

    /// <summary>
    /// Get all wards by type
    /// </summary>
    Task<IEnumerable<Ward>> GetByTypeAsync(string type);
}
