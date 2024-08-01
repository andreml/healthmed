using HealthMed.Domain.Repository;
using HealthMed.Infra.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.CrossCutting.DI;

public static class RepositoryServiceCollectionExtension
{
    public static IServiceCollection AddRepository(this IServiceCollection service)
    {
        service.AddScoped<IPatientRepository, PatientRepository>();
        service.AddScoped<IDoctorRepository, DoctorRepository>();
        service.AddScoped<IScheduleRepository, ScheduleRepository>();
        service.AddScoped<IAppointmentRepository, AppointmentRepository>();

        return service;
    }
}
