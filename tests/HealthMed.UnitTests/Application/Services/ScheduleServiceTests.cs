using HealthMed.Application.Services;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Repository;
using HealthMed.Infra.Email;
using HealthMed.UnitTests.Fixtures;
using Moq;
using Xunit;

namespace HealthMed.UnitTests.Application.Services;

[Collection(nameof(ScheduleFixtureCollection))]
public class ScheduleFixtureCollection
{
    private readonly ScheduleFixture _fixture;
    private readonly Mock<IDoctorRepository> _doctorRepoMock;
    private readonly Mock<IScheduleRepository> _scheduleRepoMock;
    private readonly Mock<IEmailService> _emailService;

    private readonly ScheduleService _service;

    public ScheduleFixtureCollection(ScheduleFixture fixture)
    {
        _fixture = fixture;
        _doctorRepoMock = new Mock<IDoctorRepository>();
        _scheduleRepoMock = new Mock<IScheduleRepository>();
        _emailService = new Mock<IEmailService>();

        _service = new ScheduleService(
                            _doctorRepoMock.Object,
                            _scheduleRepoMock.Object,
                            _emailService.Object);
    }

    [Fact]
    [Trait("Category", "AddScheduleAsync")]
    public async Task AddScheduleAsync_ShouldReturnErrorWhenDoctorNotFound()
    {
        // Arrange
        _doctorRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(value: null);

        // Act
        var result = await _service.AddScheduleAsync(_fixture.GenerateAddScheduleDto());

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Médico não encontrado", result.Errors);
        _scheduleRepoMock.Verify(x => x.AddAsync(It.IsAny<Schedule>()), times: Times.Never);
    }

    [Fact]
    [Trait("Category", "AddScheduleAsync")]
    public async Task AddScheduleAsync_ShouldReturnErrorWhenExistingSchedule()
    {
        // Arrange
        _doctorRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Doctor("", "", "", "", ""));

        _scheduleRepoMock.Setup(x => x.GetByDoctorIdAndIntervalAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<Schedule> { new Schedule() });

        // Act
        var result = await _service.AddScheduleAsync(_fixture.GenerateAddScheduleDto());

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Existem agendas criadas nesse período", result.Errors);
        _scheduleRepoMock.Verify(x => x.AddAsync(It.IsAny<Schedule>()), times: Times.Never);
    }

    [Fact]
    [Trait("Category", "AddScheduleAsync")]
    public async Task AddScheduleAsync_ShouldReturnSuccess()
    {
        // Arrange
        _doctorRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Doctor("", "", "", "", ""));

        _scheduleRepoMock.Setup(x => x.GetByDoctorIdAndIntervalAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<Schedule>());

        // Act
        var result = await _service.AddScheduleAsync(_fixture.GenerateAddScheduleDto());

        // Assert
        Assert.False(result.HasError);
        _scheduleRepoMock.Verify(x => x.AddAsync(It.IsAny<Schedule>()), times: Times.Once);
    }

    [Fact]
    [Trait("Category", "UpdateScheduleAsync")]
    public async Task UpdateScheduleAsync_ShouldReturnErrorWhenScheduleNotFound()
    {
        // Arrange
        _scheduleRepoMock.Setup(x => x.GetByIdAndDoctorIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(value: null);

        // Act
        var result = await _service.UpdateScheduleAsync(_fixture.GenerateUpdateScheduleDto());

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Agenda não encontrada", result.Errors);
        _scheduleRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Schedule>()), times: Times.Never);
    }

    [Fact]
    [Trait("Category", "UpdateScheduleAsync")]
    public async Task UpdateScheduleAsync_ShouldReturnErrorWhenChangingDay()
    {
        // Arrange
        var dto = _fixture.GenerateUpdateScheduleDto();
        dto.StartAvailabilityDate = DateTime.Now.AddDays(1);

        _scheduleRepoMock.Setup(x => x.GetByIdAndDoctorIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(new Schedule
            {
                StartAvailabilityDate = DateTime.Now,
            });

        // Act
        var result = await _service.UpdateScheduleAsync(dto);

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Não é possível alterar uma agenda para outro dia. Remova esta e crie uma nova agenda", result.Errors);
        _scheduleRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Schedule>()), times: Times.Never);
    }

    [Fact]
    [Trait("Category", "UpdateScheduleAsync")]
    public async Task UpdateScheduleAsync_ShouldReturnErrorWhenExistingSchedule()
    {
        // Arrange
        var dto = _fixture.GenerateUpdateScheduleDto();
        dto.StartAvailabilityDate = GetBaseDate().AddHours(1);

        _scheduleRepoMock.Setup(x => x.GetByIdAndDoctorIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(new Schedule() { StartAvailabilityDate = GetBaseDate().AddHours(2) });

        _scheduleRepoMock.Setup(x => x.GetByDoctorIdAndIntervalAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<Schedule> { new Schedule() });

        // Act
        var result = await _service.UpdateScheduleAsync(dto);

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Existem agendas criadas nesse período", result.Errors);
        _scheduleRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Schedule>()), times: Times.Never);
    }

    [Fact]
    [Trait("Category", "UpdateScheduleAsync")]
    public async Task UpdateScheduleAsync_ShouldReturnErrorWhenScheduleInProgress()
    {
        // Arrange
        var dto = _fixture.GenerateUpdateScheduleDto();
        dto.StartAvailabilityDate = DateTime.Now.AddMinutes(3);

        _scheduleRepoMock.Setup(x => x.GetByIdAndDoctorIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(new Schedule
            {
                StartAvailabilityDate = DateTime.Now.AddMinutes(-3),
                EndAvailabilityDate = DateTime.Now.AddMinutes(3)
            });

        _scheduleRepoMock.Setup(x => x.GetByDoctorIdAndIntervalAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<Schedule>());

        // Act
        var result = await _service.UpdateScheduleAsync(dto);

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Não é possível alterar uma agenda em andamento", result.Errors);
        _scheduleRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Schedule>()), times: Times.Never);
    }

    [Fact]
    [Trait("Category", "UpdateScheduleAsync")]
    public async Task UpdateScheduleAsync_ShouldReturnErrorWhenOldSchedule()
    {
        // Arrange
        var dto = _fixture.GenerateUpdateScheduleDto();
        dto.StartAvailabilityDate = DateTime.Now.AddMinutes(1);
        dto.StartAvailabilityDate = DateTime.Now.AddMinutes(2);

        _scheduleRepoMock.Setup(x => x.GetByIdAndDoctorIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(new Schedule
            {
                StartAvailabilityDate = DateTime.Now.AddMinutes(-2),
                EndAvailabilityDate = DateTime.Now.AddMinutes(-1)
            });

        _scheduleRepoMock.Setup(x => x.GetByDoctorIdAndIntervalAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<Schedule>());

        // Act
        var result = await _service.UpdateScheduleAsync(dto);

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Não é possível alterar uma agenda do passado", result.Errors);
        _scheduleRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Schedule>()), times: Times.Never);
    }

    [Fact]
    [Trait("Category", "UpdateScheduleAsync")]
    public async Task UpdateScheduleAsync_ShouldReturnSuccessAndNotifyPatients()
    {
        // Arrange
        var dto = _fixture.GenerateUpdateScheduleDto();
        dto.StartAvailabilityDate = GetBaseDate().AddHours(1);
        dto.EndAvailabilityDate = GetBaseDate().AddHours(2);

        _scheduleRepoMock.Setup(x => x.GetByIdAndDoctorIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(new Schedule
            {
                StartAvailabilityDate = GetBaseDate().AddHours(3),
                EndAvailabilityDate = GetBaseDate().AddHours(5),
                Appointments = new List<Appointment>
                {
                    new Appointment
                    {
                        Patient = new Patient("", "", "", ""),
                        StartDate = GetBaseDate().AddHours(4),
                        EndDate = GetBaseDate().AddHours(4).AddMinutes(30),
                        Schedule = new Schedule
                        {
                            Doctor = new Doctor("", "", "", "", "")
                        }
                    }
                }
            });

        _scheduleRepoMock.Setup(x => x.GetByDoctorIdAndIntervalAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<Schedule>());

        // Act
        var result = await _service.UpdateScheduleAsync(dto);

        // Assert
        Assert.False(result.HasError);
        _scheduleRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Schedule>()), times: Times.Once);
        _emailService.Verify(x => x.SendScheduleUpdateToPatientAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), times: Times.Once);
    }

    [Fact]
    [Trait("Category", "DeleteScheduleAsync")]
    public async Task DeleteScheduleAsync_ShouldReturnErrorWhenScheduleNotFound()
    {
        // Arrange
        _scheduleRepoMock.Setup(x => x.GetByIdAndDoctorIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(value: null);

        // Act
        var result = await _service.DeleteScheduleAsync(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Agenda não encontrada", result.Errors);
        _scheduleRepoMock.Verify(x => x.RemoveAsync(It.IsAny<Schedule>()), times: Times.Never);
    }

    [Fact]
    [Trait("Category", "DeleteScheduleAsync")]
    public async Task DeleteScheduleAsync_ShouldReturnErrorWhenScheduleInProgress()
    {
        // Arrange
        _scheduleRepoMock.Setup(x => x.GetByIdAndDoctorIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(new Schedule
            {
                StartAvailabilityDate = DateTime.Now.AddMinutes(-1),
                EndAvailabilityDate = DateTime.Now.AddMinutes(1)
            });

        // Act
        var result = await _service.DeleteScheduleAsync(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Não é possível remover uma agenda em andamento", result.Errors);
        _scheduleRepoMock.Verify(x => x.RemoveAsync(It.IsAny<Schedule>()), times: Times.Never);
    }

    [Fact]
    [Trait("Category", "DeleteScheduleAsync")]
    public async Task DeleteScheduleAsync_ShouldReturnErrorWhenOldSchedule()
    {
        // Arrange
        _scheduleRepoMock.Setup(x => x.GetByIdAndDoctorIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(new Schedule
            {
                StartAvailabilityDate = DateTime.Now.AddMinutes(-2),
                EndAvailabilityDate = DateTime.Now.AddMinutes(-1)
            });

        // Act
        var result = await _service.DeleteScheduleAsync(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.True(result.HasError);
        Assert.Contains("Não é possível remover uma agenda do passado", result.Errors);
        _scheduleRepoMock.Verify(x => x.RemoveAsync(It.IsAny<Schedule>()), times: Times.Never);
    }

    [Fact]
    [Trait("Category", "DeleteScheduleAsync")]
    public async Task DeleteScheduleAsync_ShouldReturnSuccessAndNotifyPatients()
    {
        // Arrange
        _scheduleRepoMock.Setup(x => x.GetByIdAndDoctorIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(new Schedule
            {
                StartAvailabilityDate = GetBaseDate(),
                EndAvailabilityDate = GetBaseDate().AddHours(1),
                Appointments = new List<Appointment>
                {
                    new Appointment
                    {
                        Patient = new Patient("", "", "", ""),
                        Schedule = new Schedule
                        {
                            Doctor = new Doctor("", "", "", "", "")
                        }
                    }
                }
            });

        // Act
        var result = await _service.DeleteScheduleAsync(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.False(result.HasError);
        _scheduleRepoMock.Verify(x => x.RemoveAsync(It.IsAny<Schedule>()), times: Times.Once);
        _emailService.Verify(x => x.SendScheduleUpdateToPatientAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), times: Times.Once);
    }

    private static DateTime GetBaseDate() =>
        DateTime.Now.AddDays(1).Date.AddHours(12); //12pm tomorrow
}
