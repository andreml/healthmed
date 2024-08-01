using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace HealthMed.Infra.Email;

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;
    private readonly IConfiguration _configuration;

    public EmailService(SmtpClient smtpClient, IConfiguration configuration)
    {
        _smtpClient = smtpClient;
        _configuration = configuration;
    }

    private async Task SendEmailAsync(string to, string emailTo, string subject, string body)
    {
        var from = _configuration["EmailConfigurations:smtpUser"];

        var email = new MimeMessage();

        email.To.Add(new MailboxAddress(to, emailTo));
        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Text)
        {
            Text = body
        };
        email.From.Add(new MailboxAddress("Health&Med", from));

        await _smtpClient.SendAsync(email);
    }

    public async Task SendScheduleUpdateToPatient(string patientEmail, string patientName, string doctorName, DateTime appointmentDate)
    {
        var subject = "Health&Med - Aviso Remarcação de Consulta";

        var body = 
        $"Olá, Sr. {patientName}!\r\n" +
        $"Houve uma alteração na agenda e sua consulta foi cancelada! \r\n" +
        $"Verifique um novo horário disponível e agende novamente. \r\n" +
        $"Médico: {doctorName}.\r\n" +
        $"Data e horário: {appointmentDate:dd/MM/yyyy} às {appointmentDate:HH:mm}.";

        await SendEmailAsync(patientName, patientEmail, subject, body);
    }

    public async Task SendAppointmentCanceledToDoctor(string doctorEmail, string doctorName, string patientName, DateTime appointmentDate)
    {
        var subject = "Health&Med - Consulta cancelada";

        var body =
        $"Olá, Dr. {doctorName}!\r\n" +
        $"Uma consulta foi cancelada! \r\n" +
        $"Paciente: {patientName}.\r\n" +
        $"Data e horário: {appointmentDate:dd/MM/yyyy} às {appointmentDate:HH:mm}.";

        await SendEmailAsync(doctorName, doctorEmail, subject, body);
    }

    public async Task SendNewAppointmentToDoctor(string doctorEmail, string doctorName, string patientName, DateTime appointmentDate)
    {
        var subject = "Health&Med - Nova consulta agendada";

        var body =
        $"Olá, Dr. {doctorName}!\r\n" +
        $"Você tem uma nova consulta marcada!" +
        $"\r\nPaciente: {patientName}.\r\n" +
        $"Data e horário: {appointmentDate:dd/MM/yyyy} às {appointmentDate:HH:mm}.";

        await SendEmailAsync(doctorName, doctorEmail, subject, body);
    }
}
