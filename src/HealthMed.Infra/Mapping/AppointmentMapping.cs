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
            .Property(x => x.StartDate)
            .IsRequired();

        builder
            .Property(x => x.EndDate)
            .IsRequired();

        builder
            .HasOne(a => a.Doctor)
            .WithMany();

        builder
            .HasOne(e => e.Doctor)
            .WithMany()
            .HasForeignKey("DoctorId") 
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(e => e.Patient)
            .WithMany()
            .HasForeignKey("PatientId")
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(x => x.Version).IsRowVersion();
    }
}
