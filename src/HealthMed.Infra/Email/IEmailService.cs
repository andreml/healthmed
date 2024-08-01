namespace HealthMed.Infra.Email;

public interface IEmailService
{
    Task SendScheduleUpdateToPatientAsync(string patientEmail, string patientName, string doctorName, DateTime appointmentDate);
    Task SendAppointmentCanceledToDoctorAsync(string doctorEmail, string doctorName, string patientName, DateTime appointmentDate);
    Task SendNewAppointmentToDoctorAsync(string doctorEmail, string doctorName, string patientName, DateTime appointmentDate);
}
