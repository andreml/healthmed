namespace HealthMed.Application.ViewModels;

public class PatientAppointmentsViewModel
{
    public ICollection<PatientAppointment> Appointments { get; set; } = default!;
}

public class PatientAppointment
{
    public string Doctor { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

