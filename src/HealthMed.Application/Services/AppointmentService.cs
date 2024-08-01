using HealthMed.Application.Dtos;
using HealthMed.Application.Services.Interfaces;
using HealthMed.Application.ViewModels;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Repository;
using HealthMed.Infra.Email;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HealthMed.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IEmailService _emailService;

    public AppointmentService(
                IAppointmentRepository appointmentRepository,
                IPatientRepository patientRepository,
                IScheduleRepository scheduleRepository,
                IEmailService emailService)
    {
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _scheduleRepository = scheduleRepository;
        _emailService = emailService;
    }

    public async Task<ResponseBase> AddAppointmentAsync(AddAppointmentDto dto)
    {
        var response = new ResponseBase();

        try
        {
            var patient = await _patientRepository.GetByIdAsync(dto.PatientId);
            if (patient is null)
            {
                response.AddError("Paciente não encontrado");
                return response;
            }

            var schedule = await _scheduleRepository.GetByIdAsync(dto.ScheduleId);
            if (schedule is null)
            {
                response.AddError("Agenda não encontrada");
                return response;
            }

            var existingAppointment = schedule.Appointments.FirstOrDefault(x => x.StartDate == dto.StartDate);
            if (existingAppointment is not null)
            {
                response.AddError("Já existe uma consulta marcada neste horário");
                return response;
            }

            var appointment = new Appointment
            {
                Schedule = schedule,
                ScheduleId = dto.ScheduleId,
                Patient = patient,
                PatientId = dto.PatientId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            await _appointmentRepository.AddAsync(appointment);

            await _emailService.SendNewAppointmentToDoctorAsync(schedule.Doctor.Email, schedule.Doctor.Name, patient.Name, appointment.StartDate);

            response.AddData("Consulta agendada com sucesso!");
            return response;
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                response.AddError("Horário indisponível");
                return response;
            }

            throw ex;
        }
    }

    public async Task<ResponseBase> DeleteAppointmentAsync(Guid patientId, Guid appointmentId)
    {
        var response = new ResponseBase();

        var appointment = await _appointmentRepository.GetAppointmentByIdAndPatientId(appointmentId, patientId);
        if (appointment is null)
        {
            response.AddError("Consulta não encontrada");
            return response;
        }

        if (appointment.StartDate < DateTime.Now && appointment.EndDate > DateTime.Now)
        {
            response.AddError("Não é possível remover uma consulta em andamento");
            return response;
        }

        if (appointment.EndDate < DateTime.Now)
        {
            response.AddError("Não é possível remover uma consulta do passado");
            return response;
        }

        await _appointmentRepository.RemoveAsync(appointment);

        await _emailService.SendAppointmentCanceledToDoctorAsync(appointment.Schedule.Doctor.Email, appointment.Schedule.Doctor.Name, appointment.Patient.Name, appointment.StartDate);

        response.AddData("Consulta desmarcada com sucesso!");
        return response;
    }

    public async Task<ResponseBase> GetPatientAppointmentsAsync(Guid patientId, DateTime startDate, DateTime endDate)
    {
        var response = new ResponseBase();

        var appointments = await _appointmentRepository.GetAppointmentsByPatientIdAndInterval(patientId, startDate, endDate);
        if (!appointments.Any())
            return response;

        var appointmentsResponse = new PatientAppointmentsViewModel
        {
            Appointments = appointments.Select(x => new PatientAppointment
            {
                Doctor = x.Schedule.Doctor.Name,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
            }).ToList()
        };

        response.AddData(appointmentsResponse);
        return response;
    }
}
