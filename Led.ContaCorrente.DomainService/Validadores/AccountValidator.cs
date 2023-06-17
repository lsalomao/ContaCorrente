using FluentValidation;
using Led.ContaCorrente.Domain.Enums.Validadores;
using Led.ContaCorrente.Domain.Requests;

namespace Led.ContaCorrente.DomainService.Validadores
{
    public class AccountValidator : AbstractValidator<AccountRequest>
    {
        public AccountValidator()
        {
            RuleSet(ValidationRules.Criar, () =>
            {
                RuleFor(account => account.Name).NotEmpty().WithMessage("O nome da conta é obrigatório.");
                RuleFor(account => account.Limit).GreaterThanOrEqualTo(50).WithMessage("O limite da conta deve ser maior ou igual a 50.");
            });
        }
    }
}
