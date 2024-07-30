namespace HealthMed.Domain.Entities;

public class Schedule : Entity
{
    public Guid DoctorId { get; set; } = default!;
    public DateTime StartAvailabilityDate { get; set; }
    public DateTime EndAvailabilityDate { get; set; }
}
