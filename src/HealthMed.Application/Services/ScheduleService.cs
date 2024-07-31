using HealthMed.Application.Dtos;
using HealthMed.Application.Services.Interfaces;
using HealthMed.Application.ViewModels;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Repository;
using HealthMed.Domain.Utils;

namespace HealthMed.Application.Services;

public class ScheduleService : IScheduleService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public ScheduleService(
                    IDoctorRepository doctorRepository,
                    IAppointmentRepository appointmentRepository)
    {
        _doctorRepository = doctorRepository;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<ResponseBase> AddScheduleAsync(AddScheduleDto dto)
    {
        var response = new ResponseBase();

        dto.RemoveSeconds();

        var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
        if (doctor is null)
        {
            response.AddError("Médico não encontrado");
            return response;
        }

        var existingSchedules = await _appointmentRepository.GetAppointmentsOfDayByDoctorIdAsync(dto.DoctorId, dto.StartAvailabilityDate.Date);
        foreach (var existingSchedule in existingSchedules)
        {
            if (existingSchedule.StartDate >= dto.StartAvailabilityDate && existingSchedule.EndDate <= dto.EndAvailabilityDate)
            {
                response.AddError("Existem agendas criadas nesse período");
                return response;
            }
        }

        var intervals = DateUtils.SplitInto30MinuteIntervals(dto.StartAvailabilityDate, dto.EndAvailabilityDate);

        var scheduleId = Guid.NewGuid();
        foreach (var interval in intervals)
        {
            var schedule = new Appointment
            {
                ScheduleId = scheduleId,
                Doctor = doctor,
                StartDate = interval.Item1,
                EndDate = interval.Item2
            };

            await _appointmentRepository.AddAsync(schedule);
        }

        response.AddData("Agenda adicionada com sucesso!");
        return response;
    }

    public async Task<ResponseBase> UpdateScheduleAsync(UpdateScheduleDto dto)
    {
        var response = new ResponseBase();

        dto.RemoveSeconds();

        var schedules = await _appointmentRepository.GetAppointmentsByIdAndDoctorIdAsync(dto.ScheduleId, dto.DoctorId);
        if (schedules is null || !schedules.Any())
        {
            response.AddError("Agenda não encontrada");
            return response;
        }

        var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);

        if (schedules.First().StartDate.Date != dto.StartAvailabilityDate.Date)
        {
            response.AddError("Não é possível alterar uma agenda para outro dia. Remova esta e crie uma nova agenda");
            return response;
        }

        var existingSchedules = await _appointmentRepository.GetAppointmentsOfDayByDoctorIdAsync(dto.DoctorId, dto.StartAvailabilityDate.Date);
        foreach (var existingSchedule in existingSchedules)
        {
            if (existingSchedule.StartDate >= dto.StartAvailabilityDate && existingSchedule.EndDate <= dto.EndAvailabilityDate
                && existingSchedule.ScheduleId != dto.ScheduleId)
            {
                response.AddError("Existem agendas criadas nesse período");
                return response;
            }
        }

        //create new intervals of range
        var newScheduleInterval = DateUtils.SplitInto30MinuteIntervals(dto.StartAvailabilityDate, dto.EndAvailabilityDate);
        foreach (var scheduleInterval in newScheduleInterval)
        {
            if (!schedules.Any(x => x.StartDate == scheduleInterval.Item1 && x.EndDate == scheduleInterval.Item2))
            {
                var schedule = new Appointment
                {
                    ScheduleId = dto.ScheduleId,
                    Doctor = doctor!,
                    StartDate = scheduleInterval.Item1,
                    EndDate = scheduleInterval.Item2
                };

                await _appointmentRepository.AddAsync(schedule);
            }
        }

        //Affected appointments and delete offrange schedules
        foreach (var schedule in schedules)
        {
            if (schedule.StartDate >= dto.StartAvailabilityDate || schedule.EndDate <= dto.EndAvailabilityDate)
            {
                //if (schedule.Patient is not null)
                //TODO: Send email

                await _appointmentRepository.RemoveAsync(schedule);
            }

        }

        response.AddData("Agenda alterada com sucesso!");
        return response;
    }

    public async Task<ResponseBase> DeleteScheduleAsync(Guid doctorId, Guid scheduleId)
    {
        var response = new ResponseBase();

        var schedules = await _appointmentRepository.GetAppointmentsByIdAndDoctorIdAsync(doctorId, scheduleId);
        if (schedules is null || !schedules.Any())
        {
            response.AddError("Agenda não encontrada");
            return response;
        }

        foreach (var schedule in schedules)
        {
            //if(schedule.Patient is not null)
            //TODO: Send email

            await _appointmentRepository.RemoveAsync(schedule);
        }

        response.AddData("Agenda deletada com sucesso!");
        return response;
    }

    public async Task<ResponseBase> GetAvailableSchedulesAsync(Guid doctorId, DateTime startDate, DateTime endDate)
    {
        var response = new ResponseBase();

        var schedule = await _appointmentRepository.GetAppointmentsByDoctorIdAndIntervalAsync(doctorId, startDate, endDate);

        var availableSchedules = schedule.Where(x => x.Patient is null);

        if (availableSchedules == null || !availableSchedules.Any())
            return response;

        var availableSchedulesViewModel = new AvailableSchedulesViewModel
        {
            Doctor = availableSchedules.First().Doctor.Name,
            Crm = availableSchedules.First().Doctor.Crm,
            Schedules = availableSchedules.Select(x => new AvailableSchedule
            {
                AppointmentId = x.Id,
                StartDate = x.StartDate,
                EndDate = x.EndDate
            }).ToList()
        };

        response.AddData(availableSchedulesViewModel);
        return response;
    }

    public async Task<ResponseBase> GetSchedulesAsync(Guid doctorId, DateTime startDate, DateTime endDate)
    {
        var response = new ResponseBase();

        var schedule = await _appointmentRepository.GetAppointmentsByDoctorIdAndIntervalAsync(doctorId, startDate, endDate);

        if (schedule == null || !schedule.Any())
            return response;

        var schedulesIds = schedule.GroupBy(x => x.ScheduleId);

        var schedulesViewModel = schedulesIds.Select(x => new ScheduleViewModel
        {
            ScheduleId = x.Key,
            StartDate = schedule.Where(y => y.ScheduleId == x.Key).Min(y => y.StartDate),
            EndDate = schedule.Where(y => y.ScheduleId == x.Key).Max(y => y.EndDate),
            Appointments = schedule.Where(y => y.ScheduleId == x.Key && y.Patient is not null).Select(z => new AppointmentViewModel
            {
                StartDate = z.StartDate,
                EndDate = z.EndDate,
                Patient = z.Patient!.Name
            }).ToList()
        }).ToList();

        response.AddData(schedulesViewModel);
        return response;
    }
}
