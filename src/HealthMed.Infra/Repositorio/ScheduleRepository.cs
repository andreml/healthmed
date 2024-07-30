using HealthMed.Domain.Entities;
using HealthMed.Domain.Repository;
using HealthMed.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Infra.Repositorio;

public class ScheduleRepository : IScheduleRepository
{
    protected ApplicationDbContext _context;
    protected DbSet<Schedule> _dbSet;

    public ScheduleRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Schedule>();
    }

    public async Task<Schedule?> GetScheduleByDoctorIdAndIntervalAsync(Guid doctorId, DateTime startDateInterval, DateTime endDateInterval)
    {
        return await _dbSet.FirstOrDefaultAsync(x =>
                                            x.DoctorId == doctorId
                                            && (startDateInterval >= x.StartAvailabilityDate && startDateInterval < x.EndAvailabilityDate)
                                            && (endDateInterval <= x.EndAvailabilityDate && endDateInterval > startDateInterval));
    }
}
