namespace HealthMed.Application.ViewModels;

public class AvailableSchedulesViewModel
{
    public string Doctor { get; set; } = default!;
    public string Crm { get; set; } = default!;
    public ICollection<AvailableSchedule> Schedules { get; set; } = default!;
}

public class AvailableSchedule
{
    public Guid AppointmentId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
