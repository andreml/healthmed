using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.Infra.Mapping;

public class ScheduleMapping : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.DoctorId)
            .IsRequired();

        builder
            .Property(x => x.StartAvailabilityDate)
            .IsRequired();

        builder
            .Property(x => x.EndAvailabilityDate)
            .IsRequired();

        builder
            .HasOne<Doctor>()
            .WithMany()
            .HasForeignKey(a => a.DoctorId);
    }
}
