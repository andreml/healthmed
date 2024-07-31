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
    private readonly IScheduleRepository _scheduleRepository;

    public ScheduleService(
                    IDoctorRepository doctorRepository,
                    IAppointmentRepository appointmentRepository,
                    IScheduleRepository scheduleRepository)
    {
        _doctorRepository = doctorRepository;
        _appointmentRepository = appointmentRepository;
        _scheduleRepository = scheduleRepository;
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

        var existingSchedules = await _scheduleRepository.GetByDoctorIdAndIntervalAsync(dto.DoctorId, dto.StartAvailabilityDate, dto.EndAvailabilityDate);
        if (existingSchedules != null && existingSchedules.Any())
        {
            response.AddError("Existem agendas criadas nesse período");
            return response;
        }

        var schedule = new Schedule
        {
            Doctor = doctor,
            DoctorId = dto.DoctorId,
            StartAvailabilityDate = dto.StartAvailabilityDate,
            EndAvailabilityDate = dto.EndAvailabilityDate
        };

        await _scheduleRepository.AddAsync(schedule);

        response.AddData("Agenda adicionada com sucesso!");
        return response;
    }

    public async Task<ResponseBase> UpdateScheduleAsync(UpdateScheduleDto dto)
    {
        var response = new ResponseBase();

        dto.RemoveSeconds();

        var schedule = await _scheduleRepository.GetByIdAndDoctorIdAsync(dto.DoctorId, dto.ScheduleId);
        if (schedule is null)
        {
            response.AddError("Agenda não encontrada");
            return response;
        }

        var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);

        if (schedule.StartAvailabilityDate.Date != dto.StartAvailabilityDate.Date)
        {
            response.AddError("Não é possível alterar uma agenda para outro dia. Remova esta e crie uma nova agenda");
            return response;
        }

        var existingSchedules = await _scheduleRepository.GetByDoctorIdAndIntervalAsync(dto.DoctorId, dto.StartAvailabilityDate, dto.EndAvailabilityDate);
        if (existingSchedules is not null && existingSchedules.Any(x => x.Id != dto.ScheduleId))
        {
            response.AddError("Existem agendas criadas nesse período");
            return response;
        }

        //Affected appointments
        var affectedAppointments = schedule
                                        .Appointments
                                        .Where(x => x.StartDate >= dto.StartAvailabilityDate || x.EndDate <= dto.EndAvailabilityDate)
                                        .ToList();


        foreach (var appointment in affectedAppointments)
        {
            if (appointment.StartDate >= dto.StartAvailabilityDate || appointment.EndDate <= dto.EndAvailabilityDate)
            {
                schedule.Appointments.Remove(appointment);

                //TODO: Send email
            }
        }
        
        schedule.StartAvailabilityDate = dto.StartAvailabilityDate;
        schedule.EndAvailabilityDate = dto.EndAvailabilityDate;

        await _scheduleRepository.UpdateAsync(schedule);

        response.AddData("Agenda alterada com sucesso!");
        return response;
    }

    public async Task<ResponseBase> DeleteScheduleAsync(Guid doctorId, Guid scheduleId)
    {
        var response = new ResponseBase();

        var schedule = await _scheduleRepository.GetByIdAndDoctorIdAsync(doctorId, scheduleId);
        if (schedule is null)
        {
            response.AddError("Agenda não encontrada");
            return response;
        }

        foreach (var appointment in schedule.Appointments)
        {
            //TODO: Send email
        }

        await _scheduleRepository.RemoveAsync(schedule);

        response.AddData("Agenda deletada com sucesso!");
        return response;
    }

    public async Task<ResponseBase> GetAvailableSchedulesAsync(Guid doctorId, DateTime startDate, DateTime endDate)
    {
        var response = new ResponseBase();

        var doctor = await _doctorRepository.GetByIdAsync(doctorId);
        if (doctor is null)
        {
            response.AddError("Médico não encontrado");
            return response;
        }

        var schedules = await _scheduleRepository.GetByDoctorIdAndIntervalAsync(doctorId, startDate, endDate);

        var availableSchedulesViewModel = new AvailableSchedulesViewModel
        {
            Doctor = doctor.Name,
            Crm = doctor.Crm,
            AvailableSchedules = new()
        };

        foreach (var schedule in schedules)
        {
            var intervals = DateUtils.SplitInto30MinuteIntervals(schedule.StartAvailabilityDate, schedule.EndAvailabilityDate);

            foreach (var interval in intervals.Where(x => !schedule.Appointments.Any(y => y.StartDate == x.Item1)))
            {
                availableSchedulesViewModel.AvailableSchedules.Add(new AvailableSchedule
                {
                    ScheduleId = schedule.Id,
                    StartDate = interval.Item1,
                    EndDate = interval.Item2
                });
            }
        }

        response.AddData(availableSchedulesViewModel);
        return response;
    }

    public async Task<ResponseBase> GetSchedulesAsync(Guid doctorId, DateTime startDate, DateTime endDate)
    {
        var response = new ResponseBase();

        var schedule = await _scheduleRepository.GetByDoctorIdAndIntervalAsync(doctorId, startDate, endDate);

        if (schedule == null || !schedule.Any())
            return response;

        var schedulesViewModel = schedule.Select(x => new ScheduleViewModel
        {
            ScheduleId = x.Id,
            StartDate = x.StartAvailabilityDate,
            EndDate = x.EndAvailabilityDate,
            Appointments = x.Appointments.Select(y => new AppointmentViewModel
            {
                StartDate = y.StartDate,
                EndDate = y.EndDate,
                Patient = y.Patient.Name
            }).ToList()
        }).ToList();

        response.AddData(schedulesViewModel);
        return response;
    }
}
