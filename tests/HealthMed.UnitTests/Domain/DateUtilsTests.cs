using HealthMed.Domain.Utils;
using Xunit;

namespace HealthMed.UnitTests.Domain;

public class DateUtilsTests
{
    [Theory]
    [MemberData(nameof(GetDates))]
    public void DateUtils_IsValid(DateTime date, bool valid)
    {
        // Act
        var validDate = DateUtils.IsValid(date);

        //Assert
        Assert.Equal(valid, validDate);
    }

    public static IEnumerable<object[]> GetDates()
    {
        yield return new object[] { new DateTime(2024, 07, 30, 12, 00, 0), true };
        yield return new object[] { new DateTime(2024, 07, 30, 18, 30, 0), true };
        yield return new object[] { new DateTime(2024, 07, 30, 16, 00, 0), true };
        yield return new object[] { new DateTime(2024, 07, 30, 12, 15, 0), false };
        yield return new object[] { new DateTime(2024, 07, 30, 17, 36, 0), false };
        yield return new object[] { new DateTime(2024, 07, 30, 18, 59, 0), false };
    }

    [Theory]
    [MemberData(nameof(GetIntervalsValidAppointmentRange))]
    public void DateUtils_ValidAppointmentRange(DateTime startDate, DateTime endDate, bool valid)
    {
        // Act
        var validDate = DateUtils.ValidAppointmentRange(startDate, endDate);

        //Assert
        Assert.Equal(valid, validDate);
    }

    public static IEnumerable<object[]> GetIntervalsValidAppointmentRange()
    {
        yield return new object[] { new DateTime(2024, 07, 30, 12, 00, 0), new DateTime(2024, 07, 30, 12, 30, 0), true };
        yield return new object[] { new DateTime(2024, 07, 30, 18, 30, 0), new DateTime(2024, 07, 30, 19, 00, 0), true };
        yield return new object[] { new DateTime(2024, 07, 30, 12, 00, 0), new DateTime(2024, 07, 30, 13, 00, 0), false };
        yield return new object[] { new DateTime(2024, 07, 30, 17, 30, 0), new DateTime(2024, 07, 30, 17, 30, 0), false };
    }

    [Theory]
    [MemberData(nameof(GetIntervalsValidScheduleRange))]
    public void DateUtils_ValidScheduleRange(DateTime startDate, DateTime endDate, bool valid)
    {
        // Act
        var validDate = DateUtils.ValidScheduleRange(startDate, endDate);

        //Assert
        Assert.Equal(valid, validDate);
    }

    public static IEnumerable<object[]> GetIntervalsValidScheduleRange()
    {
        yield return new object[] { new DateTime(2024, 07, 30, 12, 00, 0), new DateTime(2024, 07, 30, 12, 30, 0), true };
        yield return new object[] { new DateTime(2024, 07, 30, 18, 30, 0), new DateTime(2024, 07, 30, 19, 00, 0), true };
        yield return new object[] { new DateTime(2024, 07, 30, 12, 00, 0), new DateTime(2024, 07, 30, 12, 14, 0), false };
        yield return new object[] { new DateTime(2024, 07, 30, 17, 30, 0), new DateTime(2024, 07, 30, 17, 56, 0), false };
    }

    [Theory]
    [MemberData(nameof(GetDateTimesRemoveSeconds))]
    public void DateUtils_RemoveSeconds(DateTime input, DateTime expected)
    {
        // Act
        var output = DateUtils.RemoveSeconds(input);

        //Assert
        Assert.Equal(expected, output);
    }

    public static IEnumerable<object[]> GetDateTimesRemoveSeconds()
    {
        yield return new object[] { new DateTime(2024, 07, 30, 12, 00, 23), new DateTime(2024, 07, 30, 12, 00, 0) };
        yield return new object[] { new DateTime(2024, 07, 30, 19, 00, 0), new DateTime(2024, 07, 30, 19, 00, 0) };
    }

    [Fact]
    public void DateUtils_SplitInto30MinuteIntervals()
    {
        //Arrange
        var startDate = new DateTime(2024, 07, 30, 12, 00, 00);
        var endDate = new DateTime(2024, 07, 30, 13, 00, 00);

        // Act
        var output = DateUtils.SplitInto30MinuteIntervals(startDate, endDate);

        //Assert
        Assert.Equal(2, output.Count);
        Assert.Contains(new Tuple<DateTime, DateTime>(new DateTime(2024, 07, 30, 12, 00, 00), new DateTime(2024, 07, 30, 12, 30, 00)), output);
        Assert.Contains(new Tuple<DateTime, DateTime>(new DateTime(2024, 07, 30, 12, 30, 00), new DateTime(2024, 07, 30, 13, 00, 00)), output);
    }
}
