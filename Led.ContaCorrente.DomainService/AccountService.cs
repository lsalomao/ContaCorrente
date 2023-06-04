using FluentValidation;
using Led.ContaCorrente.Domain.Abstractions.Services;
using Led.ContaCorrente.Domain.Enums;
using Led.ContaCorrente.Domain.Models;
using Led.ContaCorrente.Domain.Responses.Base;
using Led.ContaCorrente.DomainService.Validadores;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Principal;

namespace Led.ContaCorrente.DomainService
{
    public class AccountService : IAccountService
    {
        private readonly IMemoryCache cache;
        private readonly AccountValidator accountValidator;
        private readonly MovementValidator movementValidator;

        public AccountService(IMemoryCache cache, AccountValidator accountValidator, MovementValidator movementValidator)
        {
            this.cache = cache;
            this.accountValidator = accountValidator;
            this.movementValidator = movementValidator;
        }

        public async Task<Response<AccountModel>> CreateAccount(string name, decimal limit)
        {
            var account = new AccountModel
            {
                Name = name,
                Balance = 0,
                Limit = limit
            };

            var validationResult = await accountValidator.ValidateAsync(account);
            if (!validationResult.IsValid)
            {
                return new Response<AccountModel>(MotivoErro.BadRequest, validationResult.Errors.Select(p => p.ErrorMessage));
            }

            // Armazenar a conta no cache
            cache.Set(account.Id, account);

            return GetResponseAccount(account.Id);
        }

        public Response<AccountModel> GetAccountById(string accountId)
        {
            return GetResponseAccount(accountId);
        }

        public async Task<Response<MovementModel>> Deposit(string accountId, decimal amount)
        {
            var account = GetAccount(accountId);
            if (account == null)
            {
                return new Response<MovementModel>(MotivoErro.NotFound, "Conta corrente informada não encontrada.");
            }

            var movement = new MovementModel
            {
                AccountId = accountId,
                Description = $"Depósito - {accountId}",
                Amount = amount,
                Type = "CREDIT"
            };

            var validationResult = await movementValidator.ValidateAsync(movement);
            if (!validationResult.IsValid)
            {
                return new Response<MovementModel>(MotivoErro.BadRequest, validationResult.Errors.Select(p => p.ErrorMessage));
            }

            account.Balance += amount;
            account.Movements.Add(movement.Id);
            //Atualiza o saldo
            cache.Set(account.Id, account);

            // Armazenar o movimento
            cache.Set(movement.Id, movement);

            return new Response<MovementModel>(movement);
        }

        public IEnumerable<MovementModel> GetAccountStatementByPeriod(string accountId, DateTime startDate, DateTime endDate)
        {
            var account = GetAccount(accountId) ?? throw new Exception("A conta especificada não existe.");

            List<MovementModel> result = new();
            foreach (var movementId in account.Movements)
            {
                var movement = GetMovement(movementId);
                if (movement == null) continue;

                result.Add(movement);
            }

            return result;
        }

        public IEnumerable<MovementModel> GetAccountStatementByType(string accountId, string type)
        {
            var account = GetAccount(accountId) ?? throw new Exception("A conta especificada não existe.");

            List<MovementModel> result = new();
            foreach (var movementId in account.Movements)
            {
                var movement = GetMovement(movementId);
                if (movement == null) continue;

                if (movement.Type.Equals(type) is false) continue;

                result.Add(movement);
            }

            return result;
        }

        public decimal GetBalance(string accountId)
        {
            var account = GetAccount(accountId);
            return account != null ? account.Balance : throw new Exception("A conta especificada não existe.");
        }

        public MovementModel Transfer(string sourceAccountId, string destinationAccountId, decimal amount)
        {
            //var sourceValidationResult = _movementValidator.Validate(sourceMovement);
            //if (!sourceValidationResult.IsValid)
            //{
            //    throw new ValidationException(sourceValidationResult.Errors);
            //}

            //var targetValidationResult = _movementValidator.Validate(targetMovement);
            //if (!targetValidationResult.IsValid)
            //{
            //    throw new ValidationException(targetValidationResult.Errors);
            //}

            var targetAccount = GetAccount(destinationAccountId) ?? throw new Exception("A conta de destino não existe.");
            var sourceAccount = GetAccount(sourceAccountId) ?? throw new Exception("A conta de origem não existe.");

            if (sourceAccount.Balance < amount)
            {
                throw new Exception("Saldo insuficiente para realizar a transferência.");
            }

            var sourceMovement = new MovementModel
            {
                AccountId = sourceAccountId,
                Description = $"Transferência para {destinationAccountId} - {targetAccount.Name}",
                Amount = amount,
                Type = "DEBIT"
            };

            var targetMovement = new MovementModel
            {
                AccountId = destinationAccountId,
                Description = $"Transferência de {sourceAccountId} - {sourceAccount.Name} ",
                Amount = amount,
                Type = "CREDIT"
            };

            sourceAccount.Balance -= amount;
            sourceAccount.Movements.Add(sourceMovement.Id);

            targetAccount.Balance += amount;
            targetAccount.Movements.Add(targetMovement.Id);

            // Armazenar o conta
            cache.Set(sourceAccount.Id, sourceAccount);
            cache.Set(targetAccount.Id, targetAccount);

            // Armazenar o movimento
            cache.Set(sourceMovement.Id, sourceMovement);
            cache.Set(targetMovement.Id, targetMovement);

            return sourceMovement;
        }

        public MovementModel Withdraw(string accountId, decimal amount)
        {
            var account = GetAccount(accountId) ?? throw new Exception("A conta especificada não existe.");

            if (amount > account.Balance) throw new Exception("Saldo Insuficiente.");

            var movement = new MovementModel
            {
                AccountId = accountId,
                Description = $"Saque - {accountId}",
                Amount = amount,
                Type = "DEBIT"
            };

            account.Balance -= amount;
            account.Movements.Add(movement.Id);

            cache.Set(accountId, account);
            // Armazenar o movimento
            cache.Set(movement.Id, movement);

            return movement;
        }

        private AccountModel? GetAccount(string accountId)
        {
            return cache.Get<AccountModel>(accountId);
        }

        private Response<AccountModel> GetResponseAccount(string accountId)
        {
            var account = cache.Get<AccountModel>(accountId);

            if (account == null) return new Response<AccountModel>(MotivoErro.NotFound);

            return new Response<AccountModel>(account);
        }

        private MovementModel? GetMovement(string movementId)
        {
            return cache.Get<MovementModel>(movementId);
        }
    }
}