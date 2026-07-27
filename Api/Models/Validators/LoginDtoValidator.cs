using FluentValidation;
using IndieForge.DTOs;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Nome de usuário é obrigatório.")
            .MinimumLength(5).WithMessage("Nome de usuário deve ter pelo menos 5 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres.");
    }
}