using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Repository;

public interface IAppointmentRepository
{
    Task AddAsync(Appointment schedule);
    Task<Appointment?> GetAppointmentByIdAndPatientId(Guid id, Guid patientId);
    Task<ICollection<Appointment>> GetAppointmentsByPatientIdAndInterval(Guid patientId, DateTime startDate, DateTime endDate);
    Task RemoveAsync(Appointment schedule);
}
