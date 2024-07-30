using HealthMed.Domain.Utils;

namespace HealthMed.Domain.Entities;

public class Doctor : Entity
{
    public Doctor()
    {
        
    }
    public Doctor(string name, string cpf, string crm, string email, string password)
    {
        Name = name;
        Cpf = cpf;
        Crm = crm;
        Email = email;
        Password = Encryptor.Encrypt(password);
    }
    public string Name { get; set; } = default!;
    public string Cpf { get; set; } = default!;
    public string Crm { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
