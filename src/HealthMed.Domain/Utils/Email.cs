using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace HealthMed.Domain.Utils
{
    public static class Email
    {

        public static void EnviarEmail(MimeMessage template, IConfiguration configuration)
        {
            template.From.Add(new MailboxAddress("Health&Med", configuration["EmailConfigurations:smtpUser"]));
            using (var smtp = new SmtpClient())
            {
                smtp.Connect(configuration["EmailConfigurations:smtpServer"], Convert.ToInt32(configuration["EmailConfigurations:smtpPort"]));
                smtp.Authenticate(configuration["EmailConfigurations:smtpUser"], configuration["EmailConfigurations:smtpPassword"]);
                smtp.Send(template);
                smtp.Disconnect(true);
            }
        }

        public static MimeMessage FormatarTemplateConfirmacaoConsulta(string nomeMedico, string nomePaciente, DateTime dataConsulta, string destinatario)
        {
            var email = new MimeMessage();

            email.To.Add(new MailboxAddress(nomePaciente, destinatario));

            email.Subject = "Health&Med - Nova consulta agendada";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = $"Olá, Dr. {nomeMedico}!\r\n Você tem uma nova consulta marcada! \r\nPaciente: {nomePaciente}.\r\n Data e horário: {dataConsulta:dd/MM/yyyy} às {dataConsulta:HH:mm}."
            };
            return email;
        }
    }
}
