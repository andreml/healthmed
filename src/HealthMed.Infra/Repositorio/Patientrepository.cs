using HealthMed.Domain.Entities;
using HealthMed.Domain.Repository;
using HealthMed.Domain.Utils;
using HealthMed.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Infra.Repositorio;

public class PatientRepository : IPatientRepository
{
    protected ApplicationDbContext _context;
    protected DbSet<Patient> _dbSet;

    public PatientRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Patient>();
    }

    public async Task AddAsync(Patient paciente)
    {
        await _context.Patient.AddAsync(paciente);
        await _context.SaveChangesAsync();
    }

    public async Task<Patient?> GetByEmailAndPasswordAsync(string email, string password) =>
        await _dbSet.FirstOrDefaultAsync(x => x.Email == email && x.Password == Encryptor.Encrypt(password));

    public async Task<Patient?> GetByEmailOrCpfAsync(string email, string cpf) =>
        await _dbSet.FirstOrDefaultAsync(x => x.Email == email || x.Cpf == cpf);

    public async Task<Patient?> GetByIdAsync(Guid id) =>
        await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
}
