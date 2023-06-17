using FluentValidation;
using Led.ContaCorrente.Domain.Enums.Validadores;
using Led.ContaCorrente.Domain.Models;

namespace Led.ContaCorrente.DomainService.Validadores
{
    public class MovementValidator : AbstractValidator<MovementModel>
    {
        public MovementValidator()
        {
            RuleSet(ValidationRules.Deposito, () =>
            {
                RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Valor tem que ser superior à 10.");
            });
        }
    }
}


