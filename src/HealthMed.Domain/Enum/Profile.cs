using System.Text.Json.Serialization;

namespace HealthMed.Domain.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Profile
{
    Doctor,
    Patient
}

public static class Perfis
{
    public const string Doctor = "Doctor";
    public const string Patient = "Patient";
}
