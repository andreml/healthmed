using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Repository;

public interface IScheduleRepository
{
    Task<Schedule?> GetScheduleByDoctorIdAndIntervalAsync(Guid doctorId, DateTime startDateInterval, DateTime endDateInterval);
}
