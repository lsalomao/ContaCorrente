using Led.ContaCorrente.Domain.Abstractions.Repository;
using Led.ContaCorrente.Domain.Abstractions.Services;
using Led.ContaCorrente.Domain.Enums;
using Led.ContaCorrente.Domain.Models;
using Led.ContaCorrente.Domain.Responses.Base;
using Led.ContaCorrente.DomainService;
using Microsoft.Extensions.Caching.Memory;

namespace Led.ContaCorrente.Test
{
    public class AccountServiceTests
    {
        private readonly IAccountService _accountService;
        private readonly Mock<IAccountRepository> _accountRepositoryMock = new();
        private readonly Mock<IMovementRepository> _movementRepositoryMock = new();

        public AccountServiceTests()
        {
            _accountService = new AccountService(_accountRepositoryMock.Object, _movementRepositoryMock.Object);
        }

        static AccountModel GetAccount(string accountId)
        {
            return new AccountModel { Id = accountId, Balance = 500, Limit = 1000, Name = "João Messias" };
        }

        static MovementModel GetMovement(string accountId, decimal amount, TipoMovimento type)
        {
            return new MovementModel { Id = "1", AccountId = accountId, Amount = amount, Type = type };
        }

        static List<MovementModel> GetMovements(string accountId)
        {
            return new List<MovementModel>()
            {
                new MovementModel { AccountId = accountId, Amount = It.IsAny<decimal>(), Type = TipoMovimento.Credito },
                new MovementModel { AccountId = accountId, Amount = It.IsAny<decimal>(), Type = TipoMovimento.Debito },
                new MovementModel { AccountId = accountId, Amount = It.IsAny<decimal>(), Type = TipoMovimento.Debito },
                new MovementModel { AccountId = accountId, Amount = It.IsAny<decimal>(), Type = TipoMovimento.Credito },
                new MovementModel { AccountId = accountId, Amount = It.IsAny<decimal>(), Type = TipoMovimento.Debito }
            };
        }

        [Fact]
        public async Task CreateAccount_ValidData_ReturnsSuccessResponse()
        {
            await Task.CompletedTask;
            // Arrange
            string name = "João Messias";
            decimal limit = 1000m;

            // Act
            var result = _accountService.CreateAccount(name, limit);

            // Assert                        
            Assert.IsType<Response<AccountModel>>(result);
            Assert.False(result.PossuiErro);
        }

        [Fact]
        public void GetAccountById_ValidId_ReturnsAccount()
        {
            // Arrange
            string accountId = "1";

            var account = GetAccount(accountId);
            _accountRepositoryMock.Setup(p => p.GetAccountById(accountId)).Returns(account);

            // Act
            var result = _accountService.GetAccountById(accountId);

            //// Assert
            Assert.False(result.PossuiErro);
            Assert.Equal(account, result.Dados);
        }

        [Fact]
        public async Task Deposit_ValidData_ReturnsSuccessResponse()
        {
            // Arrange
            string accountId = "1";
            var account = GetAccount(accountId);
            decimal amount = 100m;
            var movementModel = GetMovement(accountId, amount, TipoMovimento.Credito);

            _accountRepositoryMock.Setup(p => p.GetAccountById(accountId)).Returns(account);
            // Act
            var result = await _accountService.Deposit(accountId, amount);

            // Assert
            Assert.IsType<Response<MovementModel>>(result);
            Assert.False(result.PossuiErro);
        }

        [Fact]
        public async Task Withdraw_ValidData_ReturnsSuccessResponse()
        {
            await Task.CompletedTask;

            // Arrange
            string accountId = "1";
            var account = GetAccount(accountId);
            decimal amount = 100m;

            _accountRepositoryMock.Setup(p => p.GetAccountById(accountId)).Returns(account);

            // Act
            var result = _accountService.Withdraw(accountId, amount);

            // Assert
            Assert.IsType<Response<MovementModel>>(result);
            Assert.False(result.PossuiErro);
        }

        [Fact]
        public async Task Transfer_ValidData_ReturnsSuccessResponse()
        {
            // Arrange
            string sourceAccountId = "1";

            var sourceAccount = GetAccount(sourceAccountId);
            string destinationAccountId = "2";

            var destinationAccount = GetAccount(destinationAccountId);
            decimal amount = 100m;

            _accountRepositoryMock.Setup(p => p.GetAccountById(sourceAccountId)).Returns(sourceAccount);
            _accountRepositoryMock.Setup(p => p.GetAccountById(destinationAccountId)).Returns(destinationAccount);

            // Act
            var result = await _accountService.Transfer(sourceAccountId, destinationAccountId, amount);

            // Assert
            Assert.IsType<Response<MovementModel>>(result);
            Assert.False(result.PossuiErro);
        }

        [Fact]
        public void GetBalance_ValidAccountId_ReturnsBalance()
        {
            // Arrange
            string accountId = "1";
            var account = GetAccount(accountId);

            _accountRepositoryMock.Setup(p => p.GetAccountById(accountId)).Returns(account);

            // Act
            var result = _accountService.GetBalance(accountId);

            // Assert
            Assert.IsType<Response<decimal>>(result);
            Assert.False(result.PossuiErro);
        }

        [Fact]
        public void GetAccountStatementByPeriod_ValidData_ReturnsStatement()
        {
            // Arrange
            string accountId = "1";
            var account = GetAccount(accountId);
            var movements = GetMovements(accountId);
            account.Movements = movements.Select(p => p.Id).ToList();
            DateTime startDate = new(2023, 1, 1);
            DateTime endDate = new(2023, 12, 31);

            _accountRepositoryMock.Setup(p => p.GetAccountById(accountId)).Returns(account);

            _movementRepositoryMock.Setup(repo => repo.GetMovementsByAccount(account))
                .Returns(movements);

            // Act
            var result = _accountService.GetAccountStatementByPeriod(accountId, startDate, endDate);

            // Assert
            Assert.IsType<Response<IEnumerable<MovementModel>>>(result);
            Assert.False(result.PossuiErro);
        }

        [Fact]
        public void GetAccountStatementByType_ValidData_ReturnsStatement()
        {
            // Arrange            
            TipoMovimento type = TipoMovimento.Credito;
            string accountId = "1";
            var account = GetAccount(accountId);
            var movements = GetMovements(accountId);
            account.Movements = movements.Select(p => p.Id).ToList();

            _accountRepositoryMock.Setup(p => p.GetAccountById(accountId)).Returns(account);

            _movementRepositoryMock.Setup(repo => repo.GetMovementsByAccount(account)).Returns(movements);

            // Act
            var result = _accountService.GetAccountStatementByType(accountId, type);

            // Assert
            Assert.IsType<Response<IEnumerable<MovementModel>>>(result);
            Assert.False(result.PossuiErro);
        }
    }
}
