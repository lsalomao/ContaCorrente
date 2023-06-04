using FluentValidation;
using Led.ContaCorrente.Domain.Models;

namespace Led.ContaCorrente.DomainService.Validadores
{
    public class AccountValidator : AbstractValidator<AccountModel>
    {
        public AccountValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Nome não informado.");
            RuleFor(x => x.Limit).GreaterThanOrEqualTo(1000).WithMessage("Valor tem que ser superior à 1000.");
        }
    }
}
