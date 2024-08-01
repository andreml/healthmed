using FluentValidation;
using HealthMed.Domain.Utils;

namespace HealthMed.Application.Dtos;

public class AddPatientDto
{
    public string Name { get; set; } = default!;
    public string Cpf { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class AddPatientDtoValidator : AbstractValidator<AddPatientDto>
{
    public AddPatientDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .Length(3, 100).WithMessage("Nome deve ter entre 3 e 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .Length(5, 100).WithMessage("Email deve ter entre 5 e 500 caracteres")
            .Matches(RegexUtils.EmailValidator).WithMessage("Email inválido");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("Cpf é obrigatório")
            .Must(ValidateDocument.ValidCpf).WithMessage("Cpf inválido");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória")
            .MinimumLength(8).WithMessage("Senha deve conter no mínimo 8 caracteres")
            .Matches(RegexUtils.PasswordValidator)
            .WithMessage("Senha deve ter pelo menos uma letra maiúscila, uma minúscula, um número e um caractere especial");
    }
}
