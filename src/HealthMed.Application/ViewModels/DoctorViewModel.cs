namespace HealthMed.Application.ViewModels;

public class DoctorsViewModel
{
    public ICollection<DoctorViewModel> Doctors { get; set; } = default!;
}

public class DoctorViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Crm { get; set; } = default!;
    public string Email { get; set; } = default!;
}
