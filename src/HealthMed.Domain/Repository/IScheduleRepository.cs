using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Repository;

public interface IScheduleRepository
{
    Task AddAsync(Schedule schedule);
    Task RemoveAsync(Schedule schedule);
    Task UpdateAsync(Schedule schedule);
    Task<ICollection<Schedule>> GetByDoctorIdAndIntervalAsync(Guid doctorId, DateTime startDate, DateTime endDate);
    Task<Schedule?> GetByIdAndDoctorIdAsync(Guid doctorId, Guid id);
    Task<Schedule?> GetByIdAsync(Guid id);
}
