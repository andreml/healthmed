using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace HealthMed.CrossCutting.InjecaoDependencia;

public static class SwaggerExtensionCollection
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            var xmlFile = "HealthMed.Api.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

            c.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Healh Med",
                Description = "Scheduling Medical Appointments"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Bearer {your token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }},
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
