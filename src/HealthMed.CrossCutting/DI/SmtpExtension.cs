using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.CrossCutting.DI;

public static class SmtpExtension
{
    public static IServiceCollection AddSmtpClient(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddTransient(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            string host = configuration["Smtp:Host"]!;
            int port = int.Parse(configuration["Smtp:Port"]!);
            bool enableSsl = bool.Parse(configuration["Smtp:EnableSsl"]!);
            string username = configuration["Smtp:Username"]!;
            string password = configuration["Smtp:Password"]!;

            var smtpClient = new SmtpClient();
            smtpClient.Connect(host, port, enableSsl);
            smtpClient.Authenticate(username, password);

            return smtpClient;
        });

        return service;
    }
}
