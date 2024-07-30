using FluentValidation;
using HealthMed.Application.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.CrossCutting.DI;

public static class ValidatorServiceCollectionExtension
{
    public static IServiceCollection AddValidators(this IServiceCollection service)
    {
        service.AddScoped<IValidator<AddPatientDto>, AddPatientDtoValidator>();

        return service;
    }
}
