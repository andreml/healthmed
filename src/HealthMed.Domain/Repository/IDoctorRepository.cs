using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Repository;

public interface IDoctorRepository
{
    Task<Doctor?> GetByIdAsync(Guid id);
    Task AddAsync(Doctor doctor);
    Task<Doctor?> GetByEmailAndPasswordAsync(string email, string password);
    Task<Doctor?> GetByEmailOrCpfAsync(string email, string cpf);
    Task<ICollection<Doctor>> GetAll();
}
