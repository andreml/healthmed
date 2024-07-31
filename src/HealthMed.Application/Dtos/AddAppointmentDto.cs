using FluentValidation;
using System.Text.Json.Serialization;

namespace HealthMed.Application.Dtos;

public class AddAppointmentDto
{
    [JsonIgnore]
    public Guid PatientId { get; set; }
    public Guid ScheduleId { get; set; }
    public Guid DoctorId { get; set; }
}

public class AddAppointmentDtoValidator : AbstractValidator<AddAppointmentDto>
{
    public AddAppointmentDtoValidator()
    {
        RuleFor(x => x.DoctorId)
            .NotEmpty().WithMessage("DoctorId é obrigatório");

        RuleFor(x => x.ScheduleId)
            .NotEmpty().WithMessage("ScheduleId é obrigatório");
    }
}
