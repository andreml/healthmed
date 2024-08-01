using HealthMed.Application.Services;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Repository;
using HealthMed.Infra.Repository;
using HealthMed.UnitTests.Fixtures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HealthMed.UnitTests.Application.Services;

[Collection(nameof(AppointmentFixtureCollection))]
public class AppointmentServiceTests
{
    private readonly AppointmentFixture _fixture;
    private readonly Mock<IAppointmentRepository> _appointmentRepoMock;
    private readonly Mock<IPatientRepository> _patientRepoMock;
    private readonly Mock<IScheduleRepository> _scheduleRepoMock;

    private readonly AppointmentService _service;

    public AppointmentServiceTests(AppointmentFixture fixture)
    {
        _fixture = fixture;
        _appointmentRepoMock = new Mock<IAppointmentRepository>();
        _patientRepoMock = new Mock<IPatientRepository>();
        _scheduleRepoMock = new Mock<IScheduleRepository>();

        _service = new AppointmentService(
                        _appointmentRepoMock.Object,
                        _patientRepoMock.Object,
                        _scheduleRepoMock.Object);
    }

    [Fact]
    [Trait("Category", "AddAppointmentAsync")]
    public async Task AddAppointmentAsync_ShouldReturnErrorWhenPatientNotFound()
    {
        // Arrange
        _patientRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(value: null);

        // Act
        var result = await _service.AddAppointmentAsync(_fixture.GenerateAddAppointmentDto());

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Paciente não encontrado", result.Errors);
        _appointmentRepoMock.Verify(x => x.AddAsync(It.IsAny<Appointment>()), times: Times.Never);
    }

    [Fact]
    [Trait("Category", "AddAppointmentAsync")]
    public async Task AddAppointmentAsync_ShouldReturnErrorWhenScheduleNotFound()
    {
        // Arrange
        _patientRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Patient("", "", "", ""));

        _scheduleRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(value: null);

        // Act
        var result = await _service.AddAppointmentAsync(_fixture.GenerateAddAppointmentDto());

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Agenda não encontrada", result.Errors);
        _appointmentRepoMock.Verify(x => x.AddAsync(It.IsAny<Appointment>()), times: Times.Never);
    }

    [Fact]
    [Trait("Category", "AddAppointmentAsync")]
    public async Task AddAppointmentAsync_ShouldReturnErrorWhenAppointmentAlreadySet()
    {
        // Arrange
        var dto = _fixture.GenerateAddAppointmentDto();

        _patientRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Patient("", "", "", ""));

        _scheduleRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Schedule
            {
                Appointments = new List<Appointment>
                {
                    new Appointment{StartDate = dto.StartDate, EndDate = dto.EndDate}
                }
            });

        // Act
        var result = await _service.AddAppointmentAsync(dto);

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Já existe uma consulta marcada neste horário", result.Errors);
        _appointmentRepoMock.Verify(x => x.AddAsync(It.IsAny<Appointment>()), times: Times.Never);
    }

    [Fact]
    [Trait("Category", "AddAppointmentAsync")]
    public async Task AddAppointmentAsync_ShouldReturSuccess()
    {
        // Arrange
        _patientRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Patient("", "", "", ""));

        _scheduleRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Schedule
            {
                Appointments = new List<Appointment>()
            });

        // Act
        var result = await _service.AddAppointmentAsync(_fixture.GenerateAddAppointmentDto());

        // Assert
        Assert.False(result.HasError);
        _appointmentRepoMock.Verify(x => x.AddAsync(It.IsAny<Appointment>()), times: Times.Once);
    }

    [Fact]
    [Trait("Category", "DeleteAppointmentAsync")]
    public async Task DeleteAppointmentAsync_ShouldReturErrorWhenAppointmentNotFound()
    {
        // Arrange
        _appointmentRepoMock.Setup(x => x.GetAppointmentByIdAndPatientId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(value: null);

        // Act
        var result = await _service.DeleteAppointmentAsync(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Consulta não encontrada", result.Errors);
        _appointmentRepoMock.Verify(x => x.RemoveAsync(It.IsAny<Appointment>()), times: Times.Never);
    }

    [Fact]
    [Trait("Category", "DeleteAppointmentAsync")]
    public async Task DeleteAppointmentAsync_ShouldReturSuccess()
    {
        // Arrange
        _appointmentRepoMock.Setup(x => x.GetAppointmentByIdAndPatientId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(new Appointment());

        // Act
        var result = await _service.DeleteAppointmentAsync(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.False(result.HasError);
        _appointmentRepoMock.Verify(x => x.RemoveAsync(It.IsAny<Appointment>()), times: Times.Once);
    }
}
