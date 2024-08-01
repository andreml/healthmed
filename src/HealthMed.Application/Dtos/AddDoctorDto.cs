using FluentValidation;
using HealthMed.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Application.Dtos
{
    public class AddDoctorDto
    {
        public string Name { get; set; } = default!;
        public string Cpf { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string CRM { get; set; } = default!;
    }

    public class AddDoctorDtoValidator : AbstractValidator<AddDoctorDto>
    {
        public AddDoctorDtoValidator()
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
                .Must(ValidateDocument.IsCpf).WithMessage("Cpf inválido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória")
                .MinimumLength(8).WithMessage("Senha deve conter no mínimo 8 caracteres")
                .Matches(RegexUtils.PasswordValidator)
                .WithMessage("Senha deve ter pelo menos uma letra maiúscila, uma minúscula, um número e um caractere especial");

            RuleFor(x => x.CRM)
                .NotEmpty().WithMessage("CRM é obrigatório")
                .MaximumLength(20).WithMessage("CRM deve ter até 20 caracteres");
        }
    }
}
