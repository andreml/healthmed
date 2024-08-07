﻿using HealthMed.Application.Services;
using HealthMed.Application.Services.Interfaces;
using HealthMed.Infra.Email;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.CrossCutting.DI;

public static class ServicesServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection service)
    {
        service.AddScoped<IPatientService, PatientService>();
        service.AddScoped<IAppointmentService, AppointmentService>();
        service.AddScoped<IDoctorService, DoctorService>();
        service.AddScoped<IScheduleService, ScheduleService>();

        service.AddScoped<IEmailService, EmailService>();

        return service;
    }
}
