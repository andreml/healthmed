using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Repository;

public interface IDoctorRepository
{
    Task<Doctor?> GetByIdAsync(Guid id);
}
