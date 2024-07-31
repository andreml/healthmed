using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Repository;

public interface IAppointmentRepository
{
    Task AddAsync(Appointment schedule);
    Task<ICollection<Appointment>> GetAppointmentsByDoctorIdAndIntervalAsync(Guid doctorId, DateTime startDateInterval, DateTime endDateInterval);
    Task<ICollection<Appointment>> GetAppointmentsOfDayByDoctorIdAsync(Guid doctorId, DateTime date);
    Task<ICollection<Appointment>> GetAppointmentsByIdAndDoctorIdAsync(Guid doctorId, Guid scheduleId);
    Task<Appointment?> GetAppointmentByIdAndDoctorId(Guid id, Guid doctorID);
    Task<ICollection<Appointment>> GetAppointmentsByPatientIdAndInterval(Guid patientId, DateTime startDate, DateTime endDate);
    Task UpdateAsync(Appointment schedule);
    Task RemoveAsync(Appointment schedule);
}
