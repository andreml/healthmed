using HealthMed.Application.Dtos;
using HealthMed.Application.Services.Interfaces;
using HealthMed.Application.ViewModels;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Repository;
using System.Data;

namespace HealthMed.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IScheduleRepository _scheduleRepository;

    public AppointmentService(
                IAppointmentRepository appointmentRepository,
                IPatientRepository patientRepository,
                IScheduleRepository scheduleRepository)
    {
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _scheduleRepository = scheduleRepository;
    }

    public async Task<ResponseBase> AddAppointmentAsync(AddAppointmentDto dto)
    {
        var response = new ResponseBase();

        try
        {
            var patient = await _patientRepository.GetByIdAsync(dto.PatientId);
            if (patient is null)
            {
                response.AddData("Paciente não encontrado");
                return response;
            }

            var schedule = await _scheduleRepository.GetByIdAsync(dto.ScheduleId);
            if (schedule is null)
            {
                response.AddData("Agenda não encontrada");
                return response;
            }

            var existingAppointment = schedule.Appointments.FirstOrDefault(x => x.StartDate == dto.StartDate);
            if (existingAppointment is not null)
            {
                response.AddData("Já existe uma consulta marcada neste horário");
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

            //TODO: enviar email avisando o agendamento
            //schedule.Doctor.Email
            //patient.Name
            //dto.StartDate
            //dto.EndDate

            response.AddData("Consulta agendada com sucesso!");
            return response;
        }
        catch (DBConcurrencyException)
        {
            response.AddError("Horário indisponível");
            return response;
        }
    }

    public async Task<ResponseBase> DeleteAppointmentAsync(Guid patientId, Guid appointmentId)
    {
        var response = new ResponseBase();

        var appointment = await _appointmentRepository.GetAppointmentByIdAndPatientId(appointmentId, patientId);
        if (appointment is null)
        {
            response.AddData("Agenda não encontrada");
            return response;
        }

        await _appointmentRepository.RemoveAsync(appointment);

        //TODO: enviar email avisando o cancelamento pelo paciente
        //appointment.Schedule.Doctor.Email
        //appointment.Patient.Name
        //appointment.StartDate
        //appointment.EndDate

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
