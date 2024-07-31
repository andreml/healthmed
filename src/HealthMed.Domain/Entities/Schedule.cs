namespace HealthMed.Domain.Entities;

public class Schedule : Entity
{
    public DateTime StartAvailabilityDate { get; set; }
    public DateTime EndAvailabilityDate { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public Doctor Doctor { get; set; } = default!;
    public Guid DoctorId { get; set; } // Foreign key property
}
