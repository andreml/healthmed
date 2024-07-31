using FluentValidation;
using HealthMed.Domain.Utils;
using System.Text.Json.Serialization;

namespace HealthMed.Application.Dtos;

public class AddAppointmentDto
{
    [JsonIgnore]
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public void RemoveSeconds()
    {
        StartDate = StartDate.RemoveSeconds();
        EndDate = EndDate.RemoveSeconds();
    }
}

public class AddAppointmentDtoValidator : AbstractValidator<AddAppointmentDto>
{
    public AddAppointmentDtoValidator()
    {
        RuleFor(x => x.DoctorId)
            .NotEmpty().WithMessage("DoctorId é obrigatório");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("StartDate é obrigatório")
            .Must(DateUtils.IsValid).WithMessage("StartDate deve terminar com minutos 00 ou 30")
            .Must((dto, startDate) => DateUtils.ValidAppointmentRange(startDate, dto.EndDate)).WithMessage("Consultas devem ter um intervalo de 30 minutos");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("EndDate é obrigatório")
            .Must(DateUtils.IsValid).WithMessage("EndDate deve terminar com minutos 00 ou 30");
    }
}
