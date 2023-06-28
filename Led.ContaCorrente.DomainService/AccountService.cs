using FluentValidation;
using Led.ContaCorrente.Domain.Abstractions.Repository;
using Led.ContaCorrente.Domain.Abstractions.Services;
using Led.ContaCorrente.Domain.Enums;
using Led.ContaCorrente.Domain.Models;
using Led.ContaCorrente.Domain.Requests;
using Led.ContaCorrente.Domain.Responses.Base;
using MediatR;

namespace Led.ContaCorrente.DomainService
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IMovementRepository movementRepository;

        public AccountService(IAccountRepository accountRepository, IMovementRepository movementRepository)
        {
            this.accountRepository = accountRepository;
            this.movementRepository = movementRepository;
        }

        public Response<AccountModel> CreateAccount(string name, decimal limit)
        {
            var account = new AccountModel
            {
                Name = name,
                Balance = 0,
                Limit = limit
            };

            accountRepository.AddAccount(account);

            return new Response<AccountModel>(account);
        }

        public Response<AccountModel> GetAccountById(string accountId)
        {
            return GetResponseAccount(accountId);
        }

        public async Task<Response<MovementModel>> Deposit(MovementRequest request)
        {
            await Task.CompletedTask;

            var account = accountRepository.GetAccountById(request.AccountId);
            if (account == null) return new Response<MovementModel>(MotivoErro.NotFound, "A conta especificada não existe.");

            var movement = new MovementModel
            {
                AccountId = request.AccountId,
                Description = $"Depósito - {request.AccountId}",
                Amount = request.Amount,
                Type = TipoMovimento.Credito
            };

            account.Balance += request.Amount;
            account.Movements.Add(movement.Id);

            accountRepository.UpdateAccount(account);
            movementRepository.AddMovement(movement);

            return new Response<MovementModel>(movement);
        }

        public Response<IEnumerable<MovementModel>> GetAccountStatementByPeriod(string accountId, DateTime startDate, DateTime endDate)
        {
            var account = accountRepository.GetAccountById(accountId);
            if (account == null) return new Response<IEnumerable<MovementModel>>(MotivoErro.NotFound, "A conta especificada não existe.");

            var result = movementRepository.GetMovementsByAccount(account);

            return result.Any() ?
                            new Response<IEnumerable<MovementModel>>(result.Where(p => p.Date > startDate && p.Date < endDate))
                            : new Response<IEnumerable<MovementModel>>(MotivoErro.NotFound);
        }

        public Response<IEnumerable<MovementModel>> GetAccountStatementByType(string accountId, TipoMovimento type)
        {
            var accountResponse = GetResponseAccount(accountId);
            if (accountResponse.PossuiErro) return new Response<IEnumerable<MovementModel>>(MotivoErro.NotFound, "A conta especificada não existe.");

            var account = accountResponse.Dados!;

            var result = movementRepository.GetMovementsByAccount(account);

            return result.Any() ?
                            new Response<IEnumerable<MovementModel>>(result.Where(p => p.Type.Equals(type)))
                            : new Response<IEnumerable<MovementModel>>(MotivoErro.NotFound);
        }

        public Response<decimal> GetBalance(string accountId)
        {
            var accountResponse = GetResponseAccount(accountId);
            if (accountResponse.PossuiErro) return new Response<decimal>(MotivoErro.NotFound, "A conta especificada não existe.");

            var account = accountResponse.Dados!;

            return new Response<decimal>(account.Balance);
        }

        public async Task<Response<MovementModel>> Transfer(TransferRequest request)
        {
            await Task.CompletedTask;

            var targetAccount = accountRepository.GetAccountById(request.TargetAccountId);
            if (targetAccount == null) return new Response<MovementModel>(MotivoErro.NotFound, "Conta corrente de destino informada não encontrada.");

            var sourceAccount = accountRepository.GetAccountById(request.SourceAccountId);
            if (sourceAccount == null) return new Response<MovementModel>(MotivoErro.NotFound, "Conta corrente informada não encontrada.");


            if (sourceAccount.Balance < request.Amount)
                return new Response<MovementModel>(MotivoErro.BadRequest, "Saldo insuficiente para realizar a transferência.");

            var sourceMovement = new MovementModel
            {
                AccountId = request.SourceAccountId,
                Description = $"Transferência para {request.TargetAccountId} - {targetAccount.Name}",
                Amount = request.Amount,
                Type = TipoMovimento.Debito
            };

            var targetMovement = new MovementModel
            {
                AccountId = request.TargetAccountId,
                Description = $"Transferência de {request.SourceAccountId} - {sourceAccount.Name} ",
                Amount = request.Amount,
                Type = TipoMovimento.Credito
            };

            sourceAccount.Balance -= request.Amount;
            sourceAccount.Movements.Add(sourceMovement.Id);

            targetAccount.Balance += request.Amount;
            targetAccount.Movements.Add(targetMovement.Id);

            accountRepository.UpdateAccount(sourceAccount);
            accountRepository.UpdateAccount(targetAccount);

            movementRepository.AddMovement(sourceMovement);
            movementRepository.AddMovement(targetMovement);

            return new Response<MovementModel>(sourceMovement);
        }

        public Response<MovementModel> Withdraw(MovementRequest request)
        {
            var account = accountRepository.GetAccountById(request.AccountId);
            if (account == null) return new Response<MovementModel>(MotivoErro.NotFound, "A conta especificada não existe.");

            if (request.Amount > account.Balance) return new Response<MovementModel>(MotivoErro.NotFound, "Saldo Insuficiente.");

            var movement = new MovementModel
            {
                AccountId = request.AccountId,
                Description = $"Saque - {request.AccountId}",
                Amount = request.Amount,
                Type = TipoMovimento.Debito
            };

            account.Balance -= request.Amount;
            account.Movements.Add(movement.Id);

            accountRepository.UpdateAccount(account);
            movementRepository.AddMovement(movement);

            return new Response<MovementModel>(movement);
        }

        private Response<AccountModel> GetResponseAccount(string accountId)
        {
            var account = accountRepository.GetAccountById(accountId);

            if (account == null) return new Response<AccountModel>(MotivoErro.NotFound, "A conta especificada não existe.");

            return new Response<AccountModel>(account);
        }
    }
}