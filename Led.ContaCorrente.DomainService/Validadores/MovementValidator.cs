using FluentValidation;
using Led.ContaCorrente.Domain.Enums.Validadores;
using Led.ContaCorrente.Domain.Requests;

namespace Led.ContaCorrente.DomainService.Validadores
{
    public class MovementValidator : AbstractValidator<MovementRequest>
    {
        public MovementValidator()
        {
            RuleSet(ValidationRules.Deposito, () =>
            {
                RuleFor(x => x.Amount).GreaterThan(10).WithMessage("Valor tem que ser superior à 10.");
            });

            RuleSet(ValidationRules.Transfencia, () =>
            {
                RuleFor(x => x.Amount).GreaterThan(50).WithMessage("Valor tem que ser superior à 50.");
            });

            RuleSet(ValidationRules.Saque, () =>
            {
                RuleFor(x => x.Amount).GreaterThan(20).WithMessage("Valor tem que ser superior à 20.");
            });
        }
    }
}