using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;

namespace HealthMed.CrossCutting.DI;

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
                Title = "HealthMed API",
                Description = "APIs for HealthMed"
            });

            c.SchemaFilter<CustomDateSchemaFilter>();


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

class CustomDateSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Type == "string" && schema.Format == "date-time")
        {
            schema.Example = new OpenApiString(DateTime.Now.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture));
        }

        if (schema.Properties != null)
        {
            foreach (var property in schema.Properties.Values)
            {
                Apply(property, context);
            }
        }
    }
}
