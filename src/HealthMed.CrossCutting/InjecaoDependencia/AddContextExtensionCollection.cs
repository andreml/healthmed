﻿using HealthMed.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.CrossCutting.InjecaoDependencia;

public static class AddContextExtensionCollection
{
    public static IServiceCollection AddContextConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ApplicationDbContext>();

        services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DataBase")));

        return services;
    }
}
