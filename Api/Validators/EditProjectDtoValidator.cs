using FluentValidation;
using IndieForge.DTOs;

namespace IndieForge.Validators
{
    public class EditProjectDtoValidator : AbstractValidator<EditProjectDto>
    {
        public EditProjectDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome do projeto é obrigatório.")
                .MinimumLength(3).WithMessage("O nome do projeto deve ter pelo menos 3 caracteres.")
                .MaximumLength(200).WithMessage("O nome do projeto deve ter no máximo 200 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("A descrição do projeto é obrigatória.")
                .MinimumLength(10).WithMessage("A descrição deve ter pelo menos 10 caracteres.")
                .MaximumLength(2000).WithMessage("A descrição deve ter no máximo 2000 caracteres.");
        }
    }
}