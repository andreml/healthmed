namespace HealthMed.Domain.Entities;

public class Appointment : Entity
{
    public Patient Patient { get; set; } = default!;
    public Guid DoctorId { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
