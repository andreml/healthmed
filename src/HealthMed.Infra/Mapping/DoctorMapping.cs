using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.Infra.Mapping;

public class DoctorMapping : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .HasColumnType("VARCHAR(100)")
            .IsRequired();

        builder
            .Property(x => x.Cpf)
            .HasColumnType("VARCHAR(11)")
            .IsRequired();

        builder
            .Property(x => x.Crm)
            .HasColumnType("VARCHAR(20)")
            .IsRequired();

        builder
            .Property(x => x.Email)
            .HasColumnType("VARCHAR(250)")
            .IsRequired();

        builder
            .Property(x => x.Password)
            .HasColumnType("VARCHAR(500)")
            .IsRequired();

        builder
            .HasMany(e => e.Schedules)
            .WithOne(e => e.Doctor)
            .HasForeignKey(e => e.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
