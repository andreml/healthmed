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

            string host = configuration["EmailConfigurations:smtpServer"]!;
            int port = int.Parse(configuration["EmailConfigurations:smtpPort"]!);
            string username = configuration["EmailConfigurations:smtpUser"]!;
            string password = configuration["EmailConfigurations:smtpPassword"]!;

            var smtpClient = new SmtpClient();
            smtpClient.Connect(host, port);
            smtpClient.Authenticate(username, password);

            return smtpClient;
        });

        return service;
    }
}
