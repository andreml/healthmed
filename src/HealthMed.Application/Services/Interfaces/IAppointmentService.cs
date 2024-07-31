using HealthMed.Application.Dtos;
using HealthMed.Application.ViewModels;

namespace HealthMed.Application.Services.Interfaces;

public interface IAppointmentService
{
    Task<ResponseBase> AddAppointmentAsync(AddAppointmentDto dto);
    Task<ResponseBase> DeleteAppointmentAsync(Guid patientId, Guid appointmentId);
    Task<ResponseBase> GetPatientAppointmentsAsync(Guid patientId, DateTime startDate, DateTime endDate);
}
