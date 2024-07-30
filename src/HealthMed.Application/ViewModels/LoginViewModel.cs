using HealthMed.Domain.Enum;

namespace HealthMed.Application.ViewModels;

public class LoginViewModel
{
    public LoginViewModel(string name, string email, Profile profile, string token)
    {
        Name = name;
        Email = email;
        Profile = profile;
        Token = token;
    }

    public string Name { get; set; }
    public string Email { get; set; }
    public Profile Profile { get; set; }
    public string Token { get; set; }
}
