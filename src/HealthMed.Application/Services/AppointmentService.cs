using HealthMed.Application.Dtos;
using HealthMed.Application.Services.Interfaces;
using HealthMed.Application.ViewModels;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Repository;
using HealthMed.Domain.Utils;

namespace HealthMed.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientrepository _patientRepository;

    public AppointmentService(
                IDoctorRepository doctorRepository, 
                IScheduleRepository scheduleRepository, 
                IAppointmentRepository appointmentRepository, 
                IPatientrepository patientRepository)
    {
        _doctorRepository = doctorRepository;
        _scheduleRepository = scheduleRepository;
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
    }

    public async Task<ResponseBase> AddAppointmentAsync(AddAppointmentDto dto)
    {
        var response = new ResponseBase();

        dto.RemoveSeconds();

        var patient = await _patientRepository.GetByIdAsync(dto.PatientId);
        if (patient is null)
        {
            response.AddError("Paciente não encontrado");
            return response;
        }

        var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
        if (doctor is null)
        {
            response.AddError("Médico não encontrado");
            return response;
        }

        var schedule = await _scheduleRepository.GetScheduleByDoctorIdAndIntervalAsync(dto.DoctorId, dto.StartDate, dto.EndDate);
        if (schedule is null)
        {
            response.AddError("Horário indisponível");
            return response;
        }

        var existingAppointment = await _appointmentRepository.GetByDoctorIdAndStartAndEnd(dto.DoctorId, dto.StartDate, dto.EndDate);
        if (existingAppointment is not null)
        {
            if (existingAppointment.Patient.Id == dto.PatientId)
                response.AddError("Você já agendou esta consulta neste horário");
            else
                response.AddError("Horário indisponível");

            return response;
        }

        var appointment = new Appointment
        {
            Patient = patient,
            DoctorId = dto.DoctorId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };

        await _appointmentRepository.AddAsync(appointment);

        response.AddData("Consulta agendada com sucesso!");
        return response;
    }

    public async Task<ResponseBase> DeleteAppointmentAsync(Guid patientId, Guid appointmentId)
    {
        var response = new ResponseBase();

        var appointment = await _appointmentRepository.GetByIdAndPatientId(appointmentId, patientId);
        if (appointment is null)
        {
            response.AddError("Consulta não encontrada");
            return response;
        }

        await _appointmentRepository.RemoveAsync(appointment);

        response.AddData("Consulta desmarcada com sucesso!");
        return response;
    }

    public async Task<ResponseBase> GetPatientAppointmentsAsync(Guid patientId, DateTime startDate, DateTime endDate)
    {
        var response = new ResponseBase();

        var appointments = await _appointmentRepository.GetByPatientIdAndInterval(patientId, startDate, endDate);
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

    public async Task<ResponseBase> GetDoctorAppointmentsAsync(Guid doctorId, DateTime startDate, DateTime endDate)
    {
        var response = new ResponseBase();

        var appointments = await _appointmentRepository.GetByDoctorIdAndInterval(doctorId, startDate, endDate);
        if (!appointments.Any())
            return response;

        var appointmentsResponse = new DoctorAppointmentsViewModel
        {
            Appointments = appointments.Select(x => new DoctorAppointment
            {
                Patient = x.Patient.Name,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
            }).ToList()
        };

        response.AddData(appointmentsResponse);
        return response;
    }
}
