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
            ScheduleId = _faker.Random.Guid()
        };
}

[CollectionDefinition("AppointmentFixtureCollection")]
public class AppointmentFixtureCollection : ICollectionFixture<AppointmentFixture>
{ }
