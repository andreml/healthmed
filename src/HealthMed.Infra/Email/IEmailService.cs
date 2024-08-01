namespace HealthMed.Infra.Email;

public interface IEmailService
{
    Task SendScheduleUpdateToPatient(string patientEmail, string patientName, string doctorName, DateTime appointmentDate);
    Task SendAppointmentCanceledToDoctor(string doctorEmail, string doctorName, string patientName, DateTime appointmentDate);
    Task SendNewAppointmentToDoctor(string doctorEmail, string doctorName, string patientName, DateTime appointmentDate);
}
