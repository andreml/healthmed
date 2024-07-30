namespace HealthMed.Domain.Utils;

public static class RegexUtils
{
    public const string PasswordValidator = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[$*&@#])[0-9a-zA-Z$*&@#]{8,}$";
    public const string EmailValidator = @"^[^@]+@[^@]+\.[^@]+$";
}
