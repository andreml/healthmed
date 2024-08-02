using FluentValidation;
using HealthMed.Domain.Utils;
using System.Text.Json.Serialization;

namespace HealthMed.Application.Dtos;

public class AddScheduleDto
{
    [JsonIgnore]
    public Guid DoctorId { get; set; }
    public DateTime StartAvailabilityDate { get; set; }
    public DateTime EndAvailabilityDate { get; set; }

    public void RemoveSeconds()
    {
        StartAvailabilityDate = StartAvailabilityDate.RemoveSeconds();
        EndAvailabilityDate = EndAvailabilityDate.RemoveSeconds();
    }
}

public class AddScheduleDtoValidator : AbstractValidator<AddScheduleDto>
{
    public AddScheduleDtoValidator()
    {
        RuleFor(x => x.StartAvailabilityDate)
            .NotEmpty().WithMessage("Hora início é obrigatório")
            .Must(DateUtils.IsValid).WithMessage("Hora início deve terminar com minutos 00 ou 30")
            .Must(x=> x > DateTime.Now).WithMessage("Hora início deve ser uma data futura")
            .Must((dto, startDate) => DateUtils.ValidScheduleRange(startDate, dto.EndAvailabilityDate)).WithMessage("Agendas devem no mínimo um intervalo de 30 minutos");

        RuleFor(x => x.EndAvailabilityDate)
            .NotEmpty().WithMessage("Hora fim é obrigatório")
            .Must(DateUtils.IsValid).WithMessage("Hora fim deve terminar com minutos 00 ou 30");

        RuleFor(x => x)
            .Must(BeOnTheSameDay).WithMessage("Uma agenda deve iniciar e finalizar no mesmo dia");

        RuleFor(x => x)
            .Must(x => x.EndAvailabilityDate > x.StartAvailabilityDate).WithMessage("Data início deve ser menor que Data fim");
    }

    private bool BeOnTheSameDay(AddScheduleDto dto) =>
        dto.StartAvailabilityDate.Date == dto.EndAvailabilityDate.Date;
}