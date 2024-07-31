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
        if (start > end)
            throw new ArgumentException("The start time must be before the end time.");

        if (start.Minute % 30 != 0 || end.Minute % 30 != 0)
            throw new ArgumentException("Start and end times must be on the hour or half-hour.");

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
