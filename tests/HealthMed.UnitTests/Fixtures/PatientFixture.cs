using Bogus;
using HealthMed.Application.Dtos;
using Xunit;

namespace HealthMed.UnitTests.Fixtures;

public class PatientFixture
{
    private readonly Faker _faker;

    public PatientFixture()
    {
        _faker = new Faker();
    }

    public AddPatientDto GenerateAddPatientDto() =>
        new()
        {
            Cpf = _faker.Random.Long(10000000000, 99999999999).ToString(),
            Email = _faker.Internet.Email(),
            Name = _faker.Name.FullName(),
            Password = _faker.Random.String(10)
        };
}

[CollectionDefinition("PatientFixtureCollection")]
public class PatientFixtureCollection : ICollectionFixture<PatientFixture>
{ }
