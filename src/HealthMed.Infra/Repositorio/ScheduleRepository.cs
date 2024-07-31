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

    public async Task AddAsync(Schedule schedule)
    {
        await _context.Schedule.AddAsync(schedule);
        await _context.SaveChangesAsync();
    }

    public async Task<Schedule?> GetScheduleByDoctorIdAndIntervalAsync(Guid doctorId, DateTime startDateInterval, DateTime endDateInterval)
    {
        return await _dbSet.FirstOrDefaultAsync(x =>
                                            x.DoctorId == doctorId
                                            && (startDateInterval >= x.StartAvailabilityDate && startDateInterval < x.EndAvailabilityDate)
                                            && (endDateInterval <= x.EndAvailabilityDate && endDateInterval > startDateInterval));
    }

    public async Task<ICollection<Schedule>> GetConcurrentSchedulesAsync(Guid doctorId, DateTime startDate, DateTime endDate) =>
        await _dbSet
                .Where(x => x.DoctorId == doctorId && 
                    (
                        (x.StartAvailabilityDate <= startDate && x.EndAvailabilityDate >= endDate) ||
                        (x.StartAvailabilityDate >= startDate && x.EndAvailabilityDate <= endDate) ||
                        (x.StartAvailabilityDate >= startDate && x.StartAvailabilityDate < endDate) ||
                        (x.EndAvailabilityDate > startDate && x.EndAvailabilityDate <= endDate)
                    )
                ).ToListAsync();

    public async Task<Schedule?> GetScheduleByIdAndDoctorIdAsync(Guid doctorId, Guid id) =>
        await _dbSet.FirstOrDefaultAsync(x => x.Id == id && x.DoctorId == doctorId);

    public async Task UpdateAsync(Schedule schedule)
    {
        _dbSet.Update(schedule);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Schedule schedule)
    {
        _context.Remove(schedule);
        await _context.SaveChangesAsync();
    }
}
