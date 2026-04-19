using Microsoft.EntityFrameworkCore;
using IcnWorksheet.Domain;

namespace IcnWorksheet.Data;

/// <summary>
/// Repository implementation for WardInfectionSurveillance entity.
/// </summary>
public class WardInfectionRepository : Repository<WardInfectionSurveillance>, IWardInfectionRepository
{
    private readonly ApplicationDbContext _context;

    public WardInfectionRepository(ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WardInfectionSurveillance>> GetByPatientIdAsync(int patientId)
    {
        return await _context.WardInfectionSurveillance
            .Where(w => w.PatientId == patientId)
            .Include(w => w.Patient)
            .OrderByDescending(w => w.Year)
            .ThenByDescending(w => w.Month)
            .ToListAsync();
    }

    public async Task<IEnumerable<WardInfectionSurveillance>> GetByWardNameAsync(string wardName)
    {
        return await _context.WardInfectionSurveillance
            .Where(w => w.WardName == wardName)
            .Include(w => w.Patient)
            .OrderByDescending(w => w.Year)
            .ThenByDescending(w => w.Month)
            .ToListAsync();
    }

    public async Task<IEnumerable<WardInfectionSurveillance>> GetByMonthYearAsync(int month, int year)
    {
        return await _context.WardInfectionSurveillance
            .Where(w => w.Month == month && w.Year == year)
            .Include(w => w.Patient)
            .OrderBy(w => w.WardName)
            .ToListAsync();
    }
}
