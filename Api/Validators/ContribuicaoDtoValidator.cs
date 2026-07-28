using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using IndieForge.DTOs;

namespace IndieForge.Validators
{
    public class ContribuicaoDtoValidator : AbstractValidator<ContribuicaoDto>
    {
        public ContribuicaoDtoValidator()
        {
            RuleFor(x => x.Valor)
                .GreaterThan(0).WithMessage("O valor da contribuição deve ser maior que zero.")
                .NotEmpty().WithMessage("O valor da contribuição é obrigatório.");

            RuleFor(x => x.DataContribuicao)
                .NotEmpty().WithMessage("A data da contribuição é obrigatória.");

            RuleFor(x => x.projetoContribuido)
                .NotNull().WithMessage("O projeto contribuído é obrigatório.");
        }
    }
}