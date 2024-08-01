using Bogus;
using HealthMed.Application.Dtos;
using Xunit;

namespace HealthMed.UnitTests.Fixtures;

public class AppointmentFixture
{
    private readonly Faker _faker;

    public AppointmentFixture()
    {
        _faker = new Faker();
    }

    public AddAppointmentDto GenerateAddAppointmentDto() =>
        new()
        {
            PatientId = _faker.Random.Guid(),
            ScheduleId = _faker.Random.Guid(),
            EndDate = _faker.Date.Future(),
            StartDate = _faker.Date.Future(),
        };
}

[CollectionDefinition("AppointmentFixtureCollection")]
public class AppointmentFixtureCollection : ICollectionFixture<AppointmentFixture>
{ }
