using HealthMed.Domain.Entities;
using HealthMed.Domain.Repository;
using HealthMed.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Infra.Repository;

public class AppointmentRepository : IAppointmentRepository
{
    protected ApplicationDbContext _context;
    protected DbSet<Appointment> _dbSet;

    public AppointmentRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Appointment>();
    }

    public async Task AddAsync(Appointment schedule)
    {
        await _context.Appointment.AddAsync(schedule);
        await _context.SaveChangesAsync();
    }

    public async Task<Appointment?> GetAppointmentByIdAndPatientId(Guid id, Guid patientId) =>
        await _dbSet
                .Include(x => x.Patient)
                .Include(x => x.Schedule)
                    .ThenInclude(x => x.Doctor)
                .FirstOrDefaultAsync(x => x.Id == id && x.Patient.Id == patientId);

    public async Task<ICollection<Appointment>> GetAppointmentsByPatientIdAndInterval(Guid patientId, DateTime startDate, DateTime endDate) =>
        await _dbSet
                .Include(x => x.Schedule)
                    .ThenInclude(x => x.Doctor)
                .Where(x => x.Patient.Id == patientId && x.StartDate >= startDate && x.EndDate <= endDate)
                .ToListAsync();

    public async Task RemoveAsync(Appointment schedule)
    {
        _context.Remove(schedule);
        await _context.SaveChangesAsync();
    }
}
