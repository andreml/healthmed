using HealthMed.Application.Services;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Repository;
using HealthMed.Infra.Repositorio;
using HealthMed.UnitTests.Fixtures;
using Moq;
using Xunit;

namespace HealthMed.UnitTests.Application.Services;

[Collection(nameof(AppointmentFixtureCollection))]
public class AppointmentServiceTests
{
    private readonly AppointmentFixture _fixture;

    private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
    private readonly Mock<IScheduleRepository> _scheduleRepositoryMock;
    private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
    private readonly Mock<IPatientRepository> _patientRepositoryMock;

    private readonly AppointmentService _service;

    public AppointmentServiceTests(AppointmentFixture appointmentFixture)
    {
        _fixture = appointmentFixture;

        _doctorRepositoryMock = new Mock<IDoctorRepository>();
        _scheduleRepositoryMock = new Mock<IScheduleRepository>();
        _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        _patientRepositoryMock = new Mock<IPatientRepository>();

        _service = new AppointmentService(
                                _doctorRepositoryMock.Object,
                                _scheduleRepositoryMock.Object,
                                _appointmentRepositoryMock.Object,
                                _patientRepositoryMock.Object);
    }

    [Fact]
    [Trait("Categoria", "AddAppointmentAsync")]
    public async Task AddAppointmentAsync_ShouldReturnErrorWhenPatientNotFound()
    {
        // Arrange
        _patientRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(value: null);

        // Act
        var result = await _service.AddAppointmentAsync(_fixture.GenerateAddAppointmentDto());

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Paciente não encontrado", result.Errors);
    }

    [Fact]
    [Trait("Categoria", "AddAppointmentAsync")]
    public async Task AddAppointmentAsync_ShouldReturnErrorWhenDoctorNotFound()
    {
        // Arrange
        _patientRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Patient());

        _doctorRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(value: null);

        // Act
        var result = await _service.AddAppointmentAsync(_fixture.GenerateAddAppointmentDto());

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Médico não encontrado", result.Errors);
    }

    [Fact]
    [Trait("Categoria", "AddAppointmentAsync")]
    public async Task AddAppointmentAsync_ShouldReturnErrorWhenScheduleNotAvailable()
    {
        // Arrange
        _patientRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Patient());

        _doctorRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Doctor());

        _scheduleRepositoryMock.Setup(x => x.GetScheduleByDoctorIdAndIntervalAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(value: null);

        // Act
        var result = await _service.AddAppointmentAsync(_fixture.GenerateAddAppointmentDto());

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Horário indisponível", result.Errors);
    }

    [Fact]
    [Trait("Categoria", "AddAppointmentAsync")]
    public async Task AddAppointmentAsync_ShouldReturnErrorAppointmentAlreadyMadeByPatient()
    {
        // Arrange
        var dto = _fixture.GenerateAddAppointmentDto();

        _patientRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Patient());

        _doctorRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Doctor());

        _scheduleRepositoryMock.Setup(x => x.GetScheduleByDoctorIdAndIntervalAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new Schedule());

        _appointmentRepositoryMock.Setup(x => x.GetByDoctorIdAndStartAndEnd(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new Appointment
            {
                Patient = new Patient
                {
                    Id = dto.PatientId
                }
            });

        // Act
        var result = await _service.AddAppointmentAsync(dto);

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Você já agendou esta consulta neste horário", result.Errors);
    }

    [Fact]
    [Trait("Categoria", "AddAppointmentAsync")]
    public async Task AddAppointmentAsync_ShouldReturnErrorAppointmentAlreadyMadeByOtherPatient()
    {
        // Arrange
        _patientRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Patient());

        _doctorRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Doctor());

        _scheduleRepositoryMock.Setup(x => x.GetScheduleByDoctorIdAndIntervalAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new Schedule());

        _appointmentRepositoryMock.Setup(x => x.GetByDoctorIdAndStartAndEnd(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new Appointment
            {
                Patient = new Patient
                {
                    Id = Guid.NewGuid()
                }
            });

        // Act
        var result = await _service.AddAppointmentAsync(_fixture.GenerateAddAppointmentDto());

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Horário indisponível", result.Errors);
    }

    [Fact]
    [Trait("Categoria", "AddAppointmentAsync")]
    public async Task AddAppointmentAsync_ShouldReturnSuccess()
    {
        // Arrange
        _patientRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Patient());

        _doctorRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Doctor());

        _scheduleRepositoryMock.Setup(x => x.GetScheduleByDoctorIdAndIntervalAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new Schedule());

        _appointmentRepositoryMock.Setup(x => x.GetByDoctorIdAndStartAndEnd(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(value: null);

        // Act
        var result = await _service.AddAppointmentAsync(_fixture.GenerateAddAppointmentDto());

        // Assert
        Assert.False(result.HasError);
    }

    [Fact]
    [Trait("Categoria", "DeleteAppointmentAsync")]
    public async Task DeleteAppointmentAsync_ShouldReturnErrorWhenAppointmentNotFound()
    {
        // Arrange
        _appointmentRepositoryMock.Setup(x => x.GetByIdAndPatientId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(value: null);

        // Act
        var result = await _service.DeleteAppointmentAsync(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Consulta não encontrada", result.Errors);
    }

    [Fact]
    [Trait("Categoria", "DeleteAppointmentAsync")]
    public async Task DeleteAppointmentAsync_ShouldReturnSuccess()
    {
        // Arrange
        _appointmentRepositoryMock.Setup(x => x.GetByIdAndPatientId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(new Appointment());

        // Act
        var result = await _service.DeleteAppointmentAsync(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.False(result.HasError);
    }
}
