namespace HealthMed.Domain.Utils;

public static class ValidateDate
{
    private const int VALID_INTERVAL_MINUTES = 30;

    public static bool IsValid(DateTime date) =>
        (date.Minute == 0 || date.Minute == 30);

    public static bool ValidRange(DateTime start, DateTime end) =>
        (end - start).TotalMinutes == VALID_INTERVAL_MINUTES;
}
