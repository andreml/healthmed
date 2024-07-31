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

    public async Task<Appointment?> GetByDoctorIdAndStartAndEnd(Guid doctorId, DateTime startDate, DateTime endDate) =>
        await _dbSet
                .Include(x => x.Patient)
                .FirstOrDefaultAsync(x => x.Id == doctorId && x.StartDate == startDate && x.EndDate == endDate);

    public async Task<ICollection<Appointment>> GetByPatientIdAndInterval(Guid patientId, DateTime startDate, DateTime endDate) =>
        await _dbSet
                .Include(x => x.Doctor)
                .Where(x => x.Patient.Id == patientId && x.StartDate >= startDate && x.StartDate <= endDate)
                .ToListAsync();

    public async Task<ICollection<Appointment>> GetByDoctorIdAndInterval(Guid doctorId, DateTime startDate, DateTime endDate) =>
        await _dbSet
                .Include(x => x.Patient)
                .Where(x => x.DoctorId == doctorId && x.StartDate >= startDate && x.StartDate <= endDate)
                .ToListAsync();

    public async Task AddAsync(Appointment appointment)
    {
        await _context.Appointment.AddAsync(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Appointment appointment)
    {
        _context.Remove(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task<Appointment?> GetByIdAndPatientId(Guid id, Guid patientId) =>
        await _dbSet.FirstOrDefaultAsync(x => x.Id == id &&x.Patient.Id == patientId);
}
