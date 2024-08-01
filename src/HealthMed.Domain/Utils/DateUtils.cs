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

    public static List<Tuple<DateTime, DateTime>> SplitInto30MinuteIntervals(DateTime start, DateTime end)
    {
        var intervals = new List<Tuple<DateTime, DateTime>>();

        DateTime currentStart = start;

        while (currentStart < end)
        {
            DateTime currentEnd = currentStart.AddMinutes(30);
            intervals.Add(new Tuple<DateTime, DateTime>(currentStart, currentEnd));
            currentStart = currentEnd;
        }

        return intervals;
    }
}
