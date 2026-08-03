using FluentValidation;
using IndieForge.DTOs;

namespace IndieForge.Validators
{
    public class CreateContributionDtoValidator : AbstractValidator<CreateContributionDto>
    {
        public CreateContributionDtoValidator()
        {
            RuleFor(x => x.Valor)
                .GreaterThan(0).WithMessage("O valor da contribuição deve ser maior que zero.");
        }
    }
}