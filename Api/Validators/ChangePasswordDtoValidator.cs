using FluentValidation;
using IndieForge.DTOs;

namespace IndieForge.Validators
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.SenhaAtual)
                .NotEmpty().WithMessage("A senha atual é obrigatória.");

            RuleFor(x => x.NovaSenha)
                .NotEmpty().WithMessage("Uma nova senha deve ser inserida.")
                .MinimumLength(6).WithMessage("A nova senha deve ter pelo menos 6 caracteres.");

            RuleFor(x => x.SenhaConfirmacao)
                .NotEmpty().WithMessage("A confirmação de senha é obrigatória.")
                .Equal(x => x.NovaSenha).WithMessage("A confirmação de senha deve corresponder à nova senha.");
        }
    }
}
