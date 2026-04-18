using IcnWorksheet.Domain;
using Microsoft.EntityFrameworkCore;

namespace IcnWorksheet.Data;

/// <summary>
/// Repository implementation for Patient entity.
/// Handles all database operations for Patient management.
/// </summary>
public class PatientRepository : Repository<Patient>, IPatientRepository
{
    private readonly ApplicationDbContext _context;

    public PatientRepository(ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// Get patient by Hospital Number (HN)
    /// </summary>
    public async Task<Patient?> GetByHospitalNumberAsync(string hospitalNumber)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(p => p.HospitalNumber == hospitalNumber);
    }

    /// <summary>
    /// Check if Hospital Number already exists
    /// </summary>
    public async Task<bool> HospitalNumberExistsAsync(string hospitalNumber)
    {
        return await _context.Patients
            .AnyAsync(p => p.HospitalNumber == hospitalNumber);
    }

    /// <summary>
    /// Search patients by name (FirstName or LastName)
    /// </summary>
    public async Task<IEnumerable<Patient>> SearchByNameAsync(string searchTerm)
    {
        var term = searchTerm.ToLower();
        return await _context.Patients
            .Where(p => p.FirstName.ToLower().Contains(term) || 
                        p.LastName.ToLower().Contains(term))
            .ToListAsync();
    }
}
