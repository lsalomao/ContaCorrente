using FluentValidation;
using Led.ContaCorrente.Domain.Models;

namespace Led.ContaCorrente.DomainService.Validadores
{
    public class MovementValidator : AbstractValidator<MovementModel>
    {
        public MovementValidator()
        {
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(10).WithMessage("Valor tem que ser superior à 10.");
        }
    }
}
