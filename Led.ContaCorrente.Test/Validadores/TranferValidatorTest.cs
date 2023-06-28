using FluentValidation.TestHelper;
using Led.ContaCorrente.Domain.Abstractions.Repository;
using Led.ContaCorrente.Domain.Enums.Validadores;
using Led.ContaCorrente.Domain.Models;
using Led.ContaCorrente.Domain.Requests;
using Led.ContaCorrente.DomainService.Validadores;

namespace Led.ContaCorrente.Test.Validadores
{
    public class TranferValidatorTest
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock = new();

        public TranferValidatorTest()
        {
        }
        public TransferValidator GetValidator()
        {
            return new TransferValidator(_accountRepositoryMock.Object);
        }

        static AccountModel GetAccount(string accountId)
        {
            return new AccountModel { Id = accountId, Balance = 500, Limit = 1000, Name = "João Messias" };
        }

        static TransferRequest GetMovement(string sourceAccountId, string targetAccountId, decimal amount)
        {
            return new TransferRequest { SourceAccountId = sourceAccountId, TargetAccountId = targetAccountId, Amount = amount };
        }

        [Fact]
        public void ValidarTransferencia()
        {
            //Arrange            
            string sourceAccountId = "1";
            var sourceAccount = GetAccount(sourceAccountId);
            string targetAccountId = "2";
            var targetAccount = GetAccount(targetAccountId);
            decimal amount = 40;

            var validator = GetValidator();
            var movement = GetMovement(sourceAccountId, targetAccountId, amount);
            //Act

            _accountRepositoryMock.Setup(p => p.GetAccountById(sourceAccountId)).Returns(sourceAccount);
            _accountRepositoryMock.Setup(p => p.GetAccountById(targetAccountId)).Returns(targetAccount);

            var result = validator.TestValidate(movement, x => x.IncludeRuleSets(ValidationRules.Transfencia));

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Amount).WithErrorMessage("Valor tem que ser superior à 50.");
        }
    }
}
