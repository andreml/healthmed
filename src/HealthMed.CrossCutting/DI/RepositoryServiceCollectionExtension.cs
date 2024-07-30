using HealthMed.Domain.Repository;
using HealthMed.Infra.Repositorio;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.CrossCutting.DI;

public static class RepositoryServiceCollectionExtension
{
    public static IServiceCollection AddRepository(this IServiceCollection service)
    {
        service.AddScoped<IPatientrepository, Patientrepository>();
        service.AddScoped<IDoctorRepository, DoctorRepository>();
        service.AddScoped<IAppointmentRepository, AppointmentRepository>();
        service.AddScoped<IScheduleRepository, ScheduleRepository>();

        return service;
    }
}
