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
        RuleFor(x => x.DoctorId)
            .NotEmpty().WithMessage("DoctorId é obrigatório");

        RuleFor(x => x.StartAvailabilityDate)
            .NotEmpty().WithMessage("StartAvailabilityDate é obrigatório")
            .Must(DateUtils.IsValid).WithMessage("StartAvailabilityDate deve terminar com minutos 00 ou 30")
            .Must((dto, startDate) => DateUtils.ValidScheduleRange(startDate, dto.EndAvailabilityDate)).WithMessage("Agendas devem no mínimo um intervalo de 30 minutos");

        RuleFor(x => x.EndAvailabilityDate)
            .NotEmpty().WithMessage("EndAvailabilityDate é obrigatório")
            .Must(DateUtils.IsValid).WithMessage("EndAvailabilityDate deve terminar com minutos 00 ou 30");

        RuleFor(x => x)
            .Must(BeOnTheSameDay).WithMessage("Uma agenda deve iniciar e finalizar no mesmo dia");
    }

    private bool BeOnTheSameDay(AddScheduleDto dto) =>
        dto.StartAvailabilityDate.Date == dto.EndAvailabilityDate.Date;
}