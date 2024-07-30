using HealthMed.Domain.Repository;
using HealthMed.Infra.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.CrossCutting.InjecaoDependencia;

public static class RepositoryServiceCollectionExtension
{
    public static IServiceCollection AddRepository(this IServiceCollection service)
    {
        service.AddScoped<IDoctorRepository, DoctorRepository>();
        service.AddScoped<IPatientRepository, PatientRepository>();
        service.AddScoped<IAppointmentRepository, AppointmentRepository>();
        service.AddScoped<IScheduleRepository, ScheduleRepository>();

        return service;
    }
}
