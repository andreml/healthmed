namespace HealthMed.Domain.Entities;

public class Doctor : Entity
{
    public string Name { get; set; } = default!;
    public string Cpf { get; set; } = default!;
    public string Crm { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
