namespace HealthMed.Application.ViewModels;

public class AvailableSchedulesViewModel
{
    public string Doctor { get; set; } = default!;
    public string Crm { get; set; } = default!;
    public List<AvailableSchedule> AvailableSchedules { get; set; } = default!;
}

public class AvailableSchedule
{
    public Guid ScheduleId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
