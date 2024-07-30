using HealthMed.Domain.Utils;
using Xunit;

namespace HealthMed.UnitTests.Domain
{
    public class ValidateDateTests
    {
        [Theory]
        [MemberData(nameof(GetDates))]
        public void ValidateDate_IsValid(DateTime date, bool valid)
        {
            // Act
            var validDate = ValidateDate.IsValid(date);

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
        [MemberData(nameof(GetIntervals))]
        public void ValidateDate_ValidRange(DateTime startDate, DateTime endDate, bool valid)
        {
            // Act
            var validDate = ValidateDate.ValidRange(startDate, endDate);

            //Assert
            Assert.Equal(valid, validDate);
        }

        public static IEnumerable<object[]> GetIntervals()
        {
            yield return new object[] { new DateTime(2024, 07, 30, 12, 00, 0), new DateTime(2024, 07, 30, 12, 30, 0), true };
            yield return new object[] { new DateTime(2024, 07, 30, 18, 30, 0), new DateTime(2024, 07, 30, 19, 00, 0), true };
            yield return new object[] { new DateTime(2024, 07, 30, 12, 00, 0), new DateTime(2024, 07, 30, 13, 00, 0), false };
            yield return new object[] { new DateTime(2024, 07, 30, 17, 30, 0), new DateTime(2024, 07, 30, 17, 30, 0), false };
        }
    }
}
