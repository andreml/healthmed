namespace HealthMed.Domain.Entities;

public class Appointment : Entity
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Schedule Schedule { get; set; } = default!;
    public Guid ScheduleId { get; set; } // Foreign key property
    public Patient Patient { get; set; } = default!;
    public Guid PatientId { get; set; } // Foreign key property
}
