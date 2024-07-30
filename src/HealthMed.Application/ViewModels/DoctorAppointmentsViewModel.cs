namespace HealthMed.Application.ViewModels;

public class DoctorAppointmentsViewModel
{
    public ICollection<DoctorAppointment> Appointments { get; set; } = default!;
}

public class DoctorAppointment
{
    public string Patient { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

