using Bogus;
using HealthMed.Application.Dtos;
using Xunit;

namespace HealthMed.UnitTests.Fixtures;

public class ScheduleFixture
{
    private readonly Faker _faker;

    public ScheduleFixture()
    {
        _faker = new Faker();
    }

    public AddScheduleDto GenerateAddScheduleDto() =>
        new()
        {
            DoctorId = Guid.NewGuid(),
            StartAvailabilityDate = _faker.Date.Future(),
            EndAvailabilityDate = _faker.Date.Future(),
        };
    public UpdateScheduleDto GenerateUpdateScheduleDto() =>
        new()
        {
            DoctorId = Guid.NewGuid(),
            ScheduleId = Guid.NewGuid(),
            StartAvailabilityDate = _faker.Date.Future(),
            EndAvailabilityDate = _faker.Date.Future(),
        };
}

[CollectionDefinition("ScheduleFixtureCollection")]
public class ScheduleFixtureCollection : ICollectionFixture<ScheduleFixture>
{ }
