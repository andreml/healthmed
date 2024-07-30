using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.Infra.Mapping;

public class PatientMapping : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
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
            .Property(x => x.Email)
            .HasColumnType("VARCHAR(250)")
            .IsRequired();

        builder
            .Property(x => x.Password)
            .HasColumnType("VARCHAR(500)")
            .IsRequired();
    }
}
