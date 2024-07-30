﻿using FluentValidation;

namespace HealthMed.Application.Dtos;

public class AuthDto
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class AutenticarDtoValidator : AbstractValidator<AuthDto>
{
    public AutenticarDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória");
    }
}
