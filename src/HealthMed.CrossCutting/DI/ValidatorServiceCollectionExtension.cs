using FluentValidation;
using HealthMed.Application.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.CrossCutting.DI;

public static class ValidatorServiceCollectionExtension
{
    public static IServiceCollection AddValidators(this IServiceCollection service)
    {
        service.AddScoped<IValidator<AddPatientDto>, AddPatientDtoValidator>();
        service.AddScoped<IValidator<AddDoctorDto>, AddDoctorDtoValidator>();
        service.AddScoped<IValidator<AddAppointmentDto>, AddAppointmentDtoValidator>();
        service.AddScoped<IValidator<AddScheduleDto>, AddScheduleDtoValidator>();
        service.AddScoped<IValidator<AuthDto>, AuthDtoValidator>();
        service.AddScoped<IValidator<UpdateScheduleDto>, UpdateScheduleDtoValidator>();

        return service;
    }
}
