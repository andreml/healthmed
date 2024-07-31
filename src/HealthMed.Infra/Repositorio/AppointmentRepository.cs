using HealthMed.Domain.Entities;
using HealthMed.Domain.Repository;
using HealthMed.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Infra.Repositorio;

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

    public async Task<ICollection<Appointment>> GetAppointmentsByDoctorIdAndIntervalAsync(Guid doctorId, DateTime startDateInterval, DateTime endDateInterval)
    {
        return await _dbSet
                        .Include(x => x.Patient)
                        .Include(x => x.Doctor)
                        .Where(x => x.Doctor.Id == doctorId
                                    && x.StartDate >= startDateInterval && x.EndDate <= endDateInterval).ToListAsync();
    }

    public async Task<ICollection<Appointment>> GetAppointmentsOfDayByDoctorIdAsync(Guid doctorId, DateTime date) =>
        await _dbSet.Where(x => x.Doctor.Id == doctorId && x.StartDate >= date && x.EndDate <= date.AddDays(1)).ToListAsync();

    public async Task<ICollection<Appointment>> GetAppointmentsByIdAndDoctorIdAsync(Guid doctorId, Guid scheduleId) =>
        await _dbSet
                .Include(x => x.Patient)
                .Where(x => x.ScheduleId == scheduleId && x.Doctor.Id == doctorId).ToListAsync();

    public async Task<Appointment?> GetAppointmentByIdAndDoctorId(Guid id, Guid doctorID) =>
        await _dbSet
                .Include(x => x.Patient)
                .FirstOrDefaultAsync(x => x.Id == id && x.Doctor.Id == doctorID);

    public async Task<ICollection<Appointment>> GetAppointmentsByPatientIdAndInterval(Guid patientId, DateTime startDate, DateTime endDate) =>
        await _dbSet
                .Include(x => x.Doctor)
                .Where(x => x.Patient != null && x.Patient.Id == patientId && x.StartDate >= startDate && x.EndDate <= endDate)
                .ToListAsync();

    public async Task UpdateAsync(Appointment schedule)
    {
        _dbSet.Update(schedule);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Appointment schedule)
    {
        _context.Remove(schedule);
        await _context.SaveChangesAsync();
    }
}
