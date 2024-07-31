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
            .HasOne(e => e.Schedule)
            .WithMany(s => s.Appointments)
            .HasForeignKey(e => e.ScheduleId)
            .IsRequired();

        builder
            .HasOne(e => e.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(e => e.PatientId)
            .IsRequired();
    }
}
