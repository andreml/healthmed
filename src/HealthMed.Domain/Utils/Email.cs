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

        public static MimeMessage FormatarTemplateAtualizacaoAgenda(string emailPaciente, string nomeMedico, string nomePaciente, DateTime dataConsulta)
        {
            var email = new MimeMessage();

            email.To.Add(new MailboxAddress(nomeMedico, emailPaciente));

            email.Subject = "Health&Med - Aviso Remarcação de Consulta";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = $"Olá, Sr. {nomePaciente}!\r\n Sua consulta deverá ser remarcada! Favor verificar disponbilidade de agenda. \r\nMédico: {nomeMedico}.\r\n Data e horário: {dataConsulta:dd/MM/yyyy} às {dataConsulta:HH:mm}."
            };
            return email;
        }

        public static MimeMessage FormatarTemplateCancelamentoConsulta(string emailMedico, string nomeMedico, string nomePaciente, DateTime dataConsulta)
        {
            var email = new MimeMessage();

            email.To.Add(new MailboxAddress(nomeMedico, emailMedico));

            email.Subject = "Health&Med - Consulta cancelada";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = $"Olá, Dr. {nomeMedico}!\r\n Uma consulta foi cancelada! \r\nPaciente: {nomePaciente}.\r\n Data e horário: {dataConsulta:dd/MM/yyyy} às {dataConsulta:HH:mm}."
            };
            return email;
        }

        public static MimeMessage FormatarTemplateConfirmacaoConsulta(string nomeMedico, string nomePaciente, DateTime dataConsulta, string emailMedico)
        {
            var email = new MimeMessage();

            email.To.Add(new MailboxAddress(nomePaciente, emailMedico));

            email.Subject = "Health&Med - Nova consulta agendada";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = $"Olá, Dr. {nomeMedico}!\r\n Você tem uma nova consulta marcada! \r\nPaciente: {nomePaciente}.\r\n Data e horário: {dataConsulta:dd/MM/yyyy} às {dataConsulta:HH:mm}."
            };
            return email;
        }
    }
}
