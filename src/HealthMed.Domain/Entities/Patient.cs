using HealthMed.Domain.Utils;

namespace HealthMed.Domain.Entities;

public class Patient : Entity
{
    public string Name { get; set; } = default!;
    public string Cpf { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public ICollection<Appointment> Appointments { get; set; }

    public Patient(string name, string cpf, string email, string password)
    {
        Name = name;
        Cpf = cpf;
        Email = email;
        Password = Encryptor.Encrypt(password);
        Appointments = new List<Appointment>();
    }
}
