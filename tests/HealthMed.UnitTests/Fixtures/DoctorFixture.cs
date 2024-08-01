using Bogus;
using HealthMed.Application.Dtos;
using Xunit;

namespace HealthMed.UnitTests.Fixtures;

public class DoctorFixture
{
    private readonly Faker _faker;

    public DoctorFixture()
    {
        _faker = new Faker();
    }

    public AddDoctorDto GenerateAddDoctorDto() =>
        new()
        {
            Cpf = _faker.Random.Long(10000000000, 99999999999).ToString(),
            Email = _faker.Internet.Email(),
            Name = _faker.Name.FullName(),
            Password = _faker.Random.String(10),
            CRM = _faker.Random.String(10)
        };
}

[CollectionDefinition("DoctorFixtureCollection")]
public class DoctorFixtureCollection : ICollectionFixture<DoctorFixture>
{ }
