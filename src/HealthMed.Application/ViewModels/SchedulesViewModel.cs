namespace HealthMed.Application.ViewModels;

public class ScheduleViewModel
{
    public Guid ScheduleId { get; set; }
    public DateTime StartDate { get; set; } = default!;
    public DateTime EndDate { get; set; } = default!;
    public ICollection<AppointmentViewModel> Appointments { get; set; } = default!;
}

public class AppointmentViewModel
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Patient { get; set; } = default!;
}
