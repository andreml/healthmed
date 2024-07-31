using HealthMed.Domain.Entities;
using HealthMed.Domain.Repository;
using HealthMed.Domain.Utils;
using HealthMed.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Infra.Repositorio;

public class DoctorRepository : IDoctorRepository
{
    protected ApplicationDbContext _context;
    protected DbSet<Doctor> _dbSet;

    public DoctorRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Doctor>();

    }

    public async Task AddAsync(Doctor doctor)
    {
        await _context.Doctor.AddAsync(doctor);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Doctor>> GetAll() =>
        await _dbSet.ToListAsync();

    public async Task<Doctor?> GetByEmailAndPasswordAsync(string email, string password) =>
        await _dbSet.FirstOrDefaultAsync(x => x.Email == email && x.Password == Encryptor.Encrypt(password));

    public async Task<Doctor?> GetByEmailOrCpfAsync(string email, string cpf) =>
        await _dbSet.FirstOrDefaultAsync(x => x.Email == email || x.Cpf == cpf);

    public async Task<Doctor?> GetByIdAsync(Guid id) =>
        await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
}
