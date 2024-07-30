using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Repository;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByDoctorIdAndStartAndEnd(Guid doctorId, DateTime startDate, DateTime endDate);
    Task<ICollection<Appointment>> GetByPatientIdAndInterval(Guid patientId, DateTime startDate, DateTime endDate);
    Task<ICollection<Appointment>> GetByDoctorIdAndInterval(Guid doctorId, DateTime startDate, DateTime endDate);
    Task AddAsync(Appointment appointment);
    Task RemoveAsync(Appointment appointment);
    Task<Appointment?> GetByIdAndPatientId(Guid id, Guid patientId);
}
