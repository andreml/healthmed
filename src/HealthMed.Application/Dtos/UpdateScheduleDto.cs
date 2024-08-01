using FluentValidation;
using HealthMed.Domain.Utils;
using System.Text.Json.Serialization;

namespace HealthMed.Application.Dtos;

public class UpdateScheduleDto
{
    [JsonIgnore]
    public Guid DoctorId { get; set; }
    public Guid ScheduleId { get; set; }
    public DateTime StartAvailabilityDate { get; set; }
    public DateTime EndAvailabilityDate { get; set; }

    public void RemoveSeconds()
    {
        StartAvailabilityDate = StartAvailabilityDate.RemoveSeconds();
        EndAvailabilityDate = EndAvailabilityDate.RemoveSeconds();
    }
}

public class UpdateScheduleDtoValidator : AbstractValidator<UpdateScheduleDto>
{
    public UpdateScheduleDtoValidator()
    {
        RuleFor(x => x.ScheduleId)
            .NotEmpty().WithMessage("ScheduleId é obrigatório");

        RuleFor(x => x.StartAvailabilityDate)
            .NotEmpty().WithMessage("StartAvailabilityDate é obrigatório")
            .Must(DateUtils.IsValid).WithMessage("StartAvailabilityDate deve terminar com minutos 00 ou 30")
            .Must(x => x > DateTime.Now).WithMessage("StartAvailabilityDate deve ser uma data futura")
            .Must((dto, startDate) => DateUtils.ValidScheduleRange(startDate, dto.EndAvailabilityDate)).WithMessage("Agendas devem no mínimo um intervalo de 30 minutos");

        RuleFor(x => x.EndAvailabilityDate)
            .NotEmpty().WithMessage("EndAvailabilityDate é obrigatório")
            .Must(DateUtils.IsValid).WithMessage("EndAvailabilityDate deve terminar com minutos 00 ou 30");

        RuleFor(x => x)
            .Must(BeOnTheSameDay).WithMessage("Uma agenda deve iniciar e finalizar no mesmo dia");

        RuleFor(x => x)
            .Must(x => x.EndAvailabilityDate > x.StartAvailabilityDate).WithMessage("StartAvailabilityDate deve ser menor que EndAvailabilityDate");
    }

    private bool BeOnTheSameDay(UpdateScheduleDto dto) =>
        dto.StartAvailabilityDate.Date == dto.EndAvailabilityDate.Date;
}