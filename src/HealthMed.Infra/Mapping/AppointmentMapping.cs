using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.Infra.Mapping;

public class AppointmentMapping : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasIndex(x => new { x.StartDate, x.EndDate, x.DoctorId })
            .IsUnique();

        builder
            .Property(x => x.DoctorId)
            .IsRequired();

        builder
            .Property(x => x.StartDate)
            .IsRequired();

        builder
            .Property(x => x.EndDate)
            .IsRequired();

        builder
            .HasOne<Doctor>()
            .WithMany()
            .HasForeignKey(a => a.DoctorId);
    }
}
