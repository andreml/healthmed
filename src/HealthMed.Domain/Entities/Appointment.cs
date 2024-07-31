using System.ComponentModel.DataAnnotations;

namespace HealthMed.Domain.Entities;

public class Appointment : Entity
{
    public Guid ScheduleId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Doctor Doctor { get; set; } = default!;
    public Patient? Patient { get; set; }

    [Timestamp]
    public byte[] Version { get; set; } = default!;
}
