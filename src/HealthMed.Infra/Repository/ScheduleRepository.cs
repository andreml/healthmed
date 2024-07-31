using HealthMed.Domain.Entities;
using HealthMed.Domain.Repository;
using HealthMed.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Infra.Repository;

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

    public async Task RemoveAsync(Schedule schedule)
    {
        _context.Remove(schedule);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Schedule schedule)
    {
        _dbSet.Update(schedule);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Schedule>> GetByDoctorIdAndIntervalAsync(Guid doctorId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
                        .Include(x => x.Appointments)
                            .ThenInclude(x => x.Patient)
                        .Where(x => x.Doctor.Id == doctorId
                                       && (
                                            (x.StartAvailabilityDate <= startDate && x.EndAvailabilityDate >= endDate) ||
                                            (x.StartAvailabilityDate >= startDate && x.EndAvailabilityDate <= endDate) ||
                                            (x.StartAvailabilityDate >= startDate && x.StartAvailabilityDate < endDate) ||
                                            (x.EndAvailabilityDate > startDate && x.EndAvailabilityDate <= endDate)
                                          )).ToListAsync();
    }

    public async Task<Schedule?> GetByIdAndDoctorIdAsync(Guid doctorId, Guid id) =>
        await _dbSet
                .Include(x => x.Appointments)
                    .ThenInclude(x => x.Patient)
                .Include(x => x.Doctor)
                .FirstOrDefaultAsync(x => x.Id == id && x.Doctor.Id == doctorId);

    public async Task<Schedule?> GetByIdAsync(Guid id) =>
        await _dbSet
                .Include(x => x.Appointments)
                    .ThenInclude(x => x.Patient)
                .Include(x => x.Doctor)
                .FirstOrDefaultAsync(x => x.Id == id);
}
