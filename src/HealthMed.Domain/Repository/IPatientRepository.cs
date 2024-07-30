using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Repository;

public interface IPatientrepository
{
    Task AddAsync(Patient patient);
    Task<Patient?> GetByEmailAndPasswordAsync(string email, string password);
    Task<Patient?> GetByEmailOrCpfAsync(string email, string cpf);
    Task<Patient?> GetByIdAsync(Guid id);
}
