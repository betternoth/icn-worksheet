using IcnWorksheet.Domain;

namespace IcnWorksheet.Data;

/// <summary>
/// Repository interface for Patient entity with specific business operations.
/// </summary>
public interface IPatientRepository : IRepository<Patient>
{
    /// <summary>
    /// Get patient by Hospital Number (HN)
    /// </summary>
    Task<Patient?> GetByHospitalNumberAsync(string hospitalNumber);

    /// <summary>
    /// Check if Hospital Number already exists
    /// </summary>
    Task<bool> HospitalNumberExistsAsync(string hospitalNumber);

    /// <summary>
    /// Search patients by name
    /// </summary>
    Task<IEnumerable<Patient>> SearchByNameAsync(string searchTerm);
}
