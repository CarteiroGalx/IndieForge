using FluentValidation;
using IndieForge.DTOs;

namespace IndieForge.Validators
{
    public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
    {
        public CreateProjectDtoValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome do projeto é obrigatório.")
                .MinimumLength(3).WithMessage("O nome do projeto deve ter pelo menos 3 caracteres.")
                .MaximumLength(200).WithMessage("O nome do projeto deve ter no máximo 200 caracteres.");

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição do projeto é obrigatória.")
                .MinimumLength(50).WithMessage("A descrição deve ter pelo menos 50 caracteres.")
                .MaximumLength(2000).WithMessage("A descrição deve ter no máximo 2000 caracteres.");

            RuleFor(x => x.MetaFinanceira)
                .GreaterThan(0).WithMessage("A meta financeira deve ser maior que zero.");
        }
    }
}