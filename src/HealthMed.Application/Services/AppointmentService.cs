using HealthMed.Application.Dtos;
using HealthMed.Application.Services.Interfaces;
using HealthMed.Application.ViewModels;
using HealthMed.Domain.Repository;

namespace HealthMed.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;

    public AppointmentService(
                IAppointmentRepository appointmentRepository,
                IPatientRepository patientRepository)
    {
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
    }

    public async Task<ResponseBase> AddAppointmentAsync(AddAppointmentDto dto)
    {
        var response = new ResponseBase();

        var schedule = await _appointmentRepository.GetAppointmentByIdAndDoctorId(dto.ScheduleId, dto.DoctorId);
        if (schedule is null)
        {
            response.AddData("Agenda não encontrada");
            return response;
        }

        if (schedule.Patient is not null)
        {
            response.AddData("Já existe uma consulta marcada neste horário");
            return response;
        }

        var patient = await _patientRepository.GetByIdAsync(dto.PatientId);
        if (patient is null)
        {
            response.AddData("Paciente não encontrado");
            return response;
        }

        schedule.Patient = patient;

        await _appointmentRepository.UpdateAsync(schedule);

        response.AddData("Consulta agendada com sucesso!");
        return response;
    }

    public async Task<ResponseBase> DeleteAppointmentAsync(Guid patientId, Guid scheduleId)
    {
        var response = new ResponseBase();

        var schedule = await _appointmentRepository.GetAppointmentByIdAndDoctorId(scheduleId, patientId);
        if (schedule is null)
        {
            response.AddData("Agenda não encontrada");
            return response;
        }

        if (schedule.Patient is null)
        {
            response.AddData("Não existem consultas marcadas para este horário");
            return response;
        }

        if (schedule.Patient.Id != patientId)
        {
            response.AddData("Não foi possível desmarcar a consulta");
            return response;
        }

        schedule.Patient = null;

        await _appointmentRepository.UpdateAsync(schedule);

        response.AddData("Consulta desmarcada com sucesso!");
        return response;
    }

    public async Task<ResponseBase> GetPatientAppointmentsAsync(Guid patientId, DateTime startDate, DateTime endDate)
    {
        var response = new ResponseBase();

        var appointments = await _appointmentRepository.GetAppointmentsByPatientIdAndInterval(patientId, startDate, endDate);
        if(!appointments.Any())
            return response;

        var appointmentsResponse = new PatientAppointmentsViewModel
        {
            Appointments = appointments.Select(x => new PatientAppointment
            {
                Doctor = x.Doctor.Name,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
            }).ToList()
        };

        response.AddData(appointmentsResponse);
        return response;
    }
}
