using HealthMed.Application.Services;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Repository;
using HealthMed.UnitTests.Fixtures;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace HealthMed.UnitTests.Application.Services;

[Collection(nameof(PatientFixtureCollection))]
public class PatientServiceTests
{
    private readonly PatientFixture _fixture;
    private readonly Mock<IPatientRepository> _patientRepositoryMock;
    private readonly Mock<IConfiguration> _configMock;

    private readonly PatientService _service;

    public PatientServiceTests(PatientFixture fixture)
    {
        _fixture = fixture;
        _patientRepositoryMock = new Mock<IPatientRepository>();
        _configMock = new Mock<IConfiguration>();

        _service = new PatientService(_patientRepositoryMock.Object, _configMock.Object);
    }

    [Fact]
    [Trait("Category", "AddPatientAsync")]
    public async Task AddPatientAsync_ShouldReturnErrorWhenAlreadyExistingPatient()
    {
        // Arrange
        _patientRepositoryMock.Setup(x => x.GetByEmailOrCpfAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new Patient("","","",""));

        // Act
        var result = await _service.AddPatientAsync(_fixture.GenerateAddPatientDto());

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Já existe um cadastro de paciente com este email ou cpf", result.Errors);
        _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), times: Times.Never);
    }

    [Fact]
    [Trait("Category", "AddPatientAsync")]
    public async Task AddPatientAsync_ShouldReturnSuccess()
    {
        // Arrange
        _patientRepositoryMock.Setup(x => x.GetByEmailOrCpfAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(value: null);

        // Act
        var result = await _service.AddPatientAsync(_fixture.GenerateAddPatientDto());

        // Assert
        Assert.False(result.HasError);
        _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), times: Times.Once);
    }
}
