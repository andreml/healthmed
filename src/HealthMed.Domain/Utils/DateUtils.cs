using System;

namespace HealthMed.Domain.Utils;

public static class DateUtils
{
    private const int VALID_INTERVAL_MINUTES = 30;

    public static bool IsValid(DateTime date) =>
        (date.Minute == 0 || date.Minute == 30);

    public static bool ValidAppointmentRange(DateTime start, DateTime end) =>
        (end - start).TotalMinutes == VALID_INTERVAL_MINUTES;

    public static bool ValidScheduleRange(DateTime start, DateTime end) =>
        (end - start).TotalMinutes >= VALID_INTERVAL_MINUTES;

    public static DateTime RemoveSeconds(this DateTime date) =>
        new (date.Year, date.Month, date.Day, date.Hour, date.Minute, 0, 0, date.Kind);
}
