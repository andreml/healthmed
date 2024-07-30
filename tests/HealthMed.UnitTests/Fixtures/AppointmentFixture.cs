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
            DoctorId = _faker.Random.Guid(),
            PatientId = _faker.Random.Guid(),
            StartDate = _faker.Date.Recent(1000),
            EndDate = _faker.Date.Recent(1000)
        };
}

[CollectionDefinition("AppointmentFixtureCollection")]
public class AppointmentFixtureCollection : ICollectionFixture<AppointmentFixture>
{ }
