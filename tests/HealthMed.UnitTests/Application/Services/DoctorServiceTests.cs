using HealthMed.Application.Services;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Repository;
using HealthMed.UnitTests.Fixtures;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace HealthMed.UnitTests.Application.Services;

[Collection(nameof(DoctorFixtureCollection))]
public class DoctorServiceTests
{
    private readonly DoctorFixture _fixture;
    private readonly Mock<IDoctorRepository> _repository;
    private readonly Mock<IConfiguration> _config;

    private readonly DoctorService _service;

    public DoctorServiceTests(DoctorFixture fixture)
    {
        _fixture = fixture;
        _repository = new Mock<IDoctorRepository>();
        _config = new Mock<IConfiguration>();

        _service = new DoctorService(_repository.Object, _config.Object);
    }

    [Fact]
    [Trait("Category", "AddDoctorAsync")]
    public async Task AddDoctorAsync_ShouldReturnErrorWhenAlreadyExistingPatient()
    {
        // Arrange
        _repository.Setup(x => x.GetByEmailOrCpfAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new Doctor("", "", "", "", ""));

        // Act
        var result = await _service.AddDoctorAsync(_fixture.GenerateAddDoctorDto());

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Já existe um cadastro de médico com este email ou cpf", result.Errors);
        _repository.Verify(x => x.AddAsync(It.IsAny<Doctor>()), times: Times.Never);
    }

    [Fact]
    [Trait("Category", "AddDoctorAsync")]
    public async Task AddDoctorAsync_ShouldReturnSuccess()
    {
        // Arrange
        _repository.Setup(x => x.GetByEmailOrCpfAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(value: null);

        // Act
        var result = await _service.AddDoctorAsync(_fixture.GenerateAddDoctorDto());

        // Assert
        Assert.False(result.HasError);
        _repository.Verify(x => x.AddAsync(It.IsAny<Doctor>()), times: Times.Once);
    }
}
