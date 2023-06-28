using FluentValidation;
using Led.ContaCorrente.Domain.Abstractions.Repository;
using Led.ContaCorrente.Domain.Enums.Validadores;
using Led.ContaCorrente.Domain.Requests;

namespace Led.ContaCorrente.DomainService.Validadores
{
    public class MovementValidator : AbstractValidator<MovementRequest>
    {
        private readonly IAccountRepository accountRepository;
        public MovementValidator(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;

            RuleSet(ValidationRules.Deposito, () =>
            {
                ValidarContaCorrente(this.accountRepository);
                ValidarValorDeposito();
            });           

            RuleSet(ValidationRules.Saque, () =>
            {
                ValidarContaCorrente(this.accountRepository);
                ValidarValorSaque();
                
            });
        }

        private void ValidarValorDeposito()
        {
            RuleFor(x => x.Amount).GreaterThan(10).WithMessage("Valor tem que ser superior à 10.");
        }

        private void ValidarValorSaque()
        {
            RuleFor(x => x.Amount).GreaterThan(20).WithMessage("Valor tem que ser superior à 20.");
        }
        private void ValidarContaCorrente(IAccountRepository accountRepository)
        {
            RuleFor(x => x.AccountId)
                .NotEmpty().WithMessage("Conta Corrente Obrigatória.")
                .DependentRules(() =>
                {
                    RuleFor(x => x).Custom((request, context) =>
                    {
                        var account = accountRepository.GetAccountById(request.AccountId);

                        if (account == null)
                            context.AddFailure("AccountId", "A conta especificada não existe.");
                    });
                });
        }
    }
}