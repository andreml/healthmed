using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Repository;

public interface IScheduleRepository
{
    Task AddAsync(Schedule schedule);
    Task<Schedule?> GetScheduleByDoctorIdAndIntervalAsync(Guid doctorId, DateTime startDateInterval, DateTime endDateInterval);
    Task<ICollection<Schedule>> GetConcurrentSchedulesAsync(Guid doctorId, DateTime startDate, DateTime endDate);
    Task<Schedule?> GetScheduleByIdAndDoctorIdAsync(Guid doctorId, Guid id);
    Task UpdateAsync(Schedule schedule);
    Task RemoveAsync(Schedule schedule);
}
