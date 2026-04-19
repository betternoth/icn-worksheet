using IcnWorksheet.Domain;

namespace IcnWorksheet.Data;

/// <summary>
/// Repository interface for WardInfectionSurveillance entity with specific business operations.
/// </summary>
public interface IWardInfectionRepository : IRepository<WardInfectionSurveillance>
{
    /// <summary>
    /// Get ward infection records by patient ID
    /// </summary>
    Task<IEnumerable<WardInfectionSurveillance>> GetByPatientIdAsync(int patientId);

    /// <summary>
    /// Get ward infection records by ward name
    /// </summary>
    Task<IEnumerable<WardInfectionSurveillance>> GetByWardNameAsync(string wardName);

    /// <summary>
    /// Get ward infection records for a specific month and year
    /// </summary>
    Task<IEnumerable<WardInfectionSurveillance>> GetByMonthYearAsync(int month, int year);
}
