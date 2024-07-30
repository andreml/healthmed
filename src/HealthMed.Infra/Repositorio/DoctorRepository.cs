using HealthMed.Domain.Entities;
using HealthMed.Domain.Repository;
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

    public async Task<Doctor?> GetByIdAsync(Guid id) =>
        await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
}
