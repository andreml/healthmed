﻿using FluentValidation;
using HealthMed.Domain.Utils;
using System.Text.Json.Serialization;

namespace HealthMed.Application.Dtos;

public class AddAppointmentDto
{
    [JsonIgnore]
    public Guid PatientId { get; set; }
    public Guid ScheduleId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class AddAppointmentDtoValidator : AbstractValidator<AddAppointmentDto>
{
    public AddAppointmentDtoValidator()
    {
        RuleFor(x => x.ScheduleId)
            .NotEmpty().WithMessage("ScheduleId é obrigatório");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("StartDate é obrigatório")
            .Must(DateUtils.IsValid).WithMessage("São permitidos apenas horários com final 00min e 30min");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("EndDate é obrigatório")
            .Must(DateUtils.IsValid).WithMessage("São permitidos apenas horários com final 00min e 30min");

        RuleFor(x => x)
            .Must(x => x.EndDate > x.StartDate).WithMessage("StartDate deve ser menor que EndDate");

        RuleFor(x => x)
            .Must(x => DateUtils.ValidScheduleRange(x.StartDate, x.EndDate)).WithMessage("Consultas devem ter 30 minutos de intervalo");
    }
}
