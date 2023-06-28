using FluentValidation;
using Led.ContaCorrente.Domain.Abstractions.Repository;
using Led.ContaCorrente.Domain.Enums.Validadores;
using Led.ContaCorrente.Domain.Requests;

namespace Led.ContaCorrente.DomainService.Validadores
{
    public class TransferValidator : AbstractValidator<TransferRequest>
    {
        private readonly IAccountRepository accountRepository;
        public TransferValidator(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;

            RuleSet(ValidationRules.Transfencia, () =>
            {
                ValidarContas(this.accountRepository);
                ValidarValor();
            });
        }

        private void ValidarValor()
        {
            RuleFor(x => x.Amount).GreaterThan(50).WithMessage("Valor tem que ser superior à 50.");
        }
        private void ValidarContas(IAccountRepository accountRepository)
        {
            RuleFor(x => x.SourceAccountId)
                .NotEmpty().WithMessage("Conta Corrente origem obrigatória.")
                .DependentRules(() =>
                {
                    RuleFor(x => x).Custom((request, context) =>
                    {
                        var account = accountRepository.GetAccountById(request.SourceAccountId);

                        if (account == null)
                            context.AddFailure("A conta de origem especificada não existe.");
                    });
                });

            RuleFor(x => x.TargetAccountId)
                .NotEmpty().WithMessage("Conta Corrente destino obrigatória.")
                .DependentRules(() =>
                 {
                     RuleFor(x => x).Custom((request, context) =>
                     {
                         var account = accountRepository.GetAccountById(request.TargetAccountId);

                         if (account == null)
                             context.AddFailure("A conta de destino especificada não existe.");
                     });
                 });
        }
    }
}