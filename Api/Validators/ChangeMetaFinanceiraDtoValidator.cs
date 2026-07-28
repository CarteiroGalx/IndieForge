using FluentValidation;
using IndieForge.DTOs;

namespace IndieForge.Validators
{
    public class ChangeMetaFinanceiraDtoValidator : AbstractValidator<ChangeMetaFinanceiraDto>
    {
        public ChangeMetaFinanceiraDtoValidator()
        {
            RuleFor(x => x.ProjetoId)
                .NotEmpty().WithMessage("O projeto é obrigatório.");

            RuleFor(x => x.NovoValor)
                .NotEmpty().WithMessage("Um valor é obrigatório.")
                .GreaterThanOrEqualTo(1).WithMessage("Valor mínimo de 1 real.")
                .LessThanOrEqualTo(999_999_999).WithMessage("Valor máximo de 999.999.999 reais ultrapassado.");
        }
    }
}