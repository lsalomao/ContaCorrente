using FluentValidation.TestHelper;
using Led.ContaCorrente.Domain.Abstractions.Repository;
using Led.ContaCorrente.Domain.Enums.Validadores;
using Led.ContaCorrente.Domain.Models;
using Led.ContaCorrente.Domain.Requests;
using Led.ContaCorrente.DomainService.Validadores;

namespace Led.ContaCorrente.Test.Validadores
{
    public class MovementValidatorTest
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock = new();
        public MovementValidatorTest() { }

        public MovementValidator GetValidator()
        {
            return new MovementValidator(_accountRepositoryMock.Object);
        }

        static AccountModel GetAccount(string accountId)
        {
            return new AccountModel { Id = accountId, Balance = 500, Limit = 1000, Name = "João Messias" };
        }

        static MovementRequest GetMovement(string accountId, decimal amount)
        {
            return new MovementRequest { AccountId = accountId,  Amount = amount };
        }


        [Fact]
        public void ValidarDeposito_Correto()
        {
            //Arrange            
            string accountId = "1";
            var account = GetAccount(accountId);
            decimal amount = 51;
            var validator = GetValidator();
            var movement = GetMovement(accountId, amount);
            //Act

            _accountRepositoryMock.Setup(p => p.GetAccountById(accountId)).Returns(account);

            var result = validator.TestValidate(movement, x => x.IncludeRuleSets(ValidationRules.Deposito));

            //Assert            
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ValidarDeposito_ContaCorrenteNaoInformada()
        {
            //Arrange            
            string accountId = "";
            var account = GetAccount(accountId);
            decimal amount = 5;
            var validator = GetValidator();
            var movement = GetMovement(accountId, amount);
            //Act

            var result = validator.TestValidate(movement, x => x.IncludeRuleSets(ValidationRules.Deposito));

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.AccountId).WithErrorMessage("Conta Corrente Obrigatória.");
            result.ShouldHaveValidationErrorFor(x => x.Amount).WithErrorMessage("Valor tem que ser superior à 10.");
        }

        [Fact]
        public void ValidarDeposito_ContaCorrenteInformadaNaoExiste()
        {
            //Arrange            
            string accountId = "1";
            var account = GetAccount(accountId);
            decimal amount = 5;
            var validator = GetValidator();
            var movement = GetMovement(accountId, amount);
            //Act

            var result = validator.TestValidate(movement, x => x.IncludeRuleSets(ValidationRules.Deposito));

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.AccountId).WithErrorMessage("A conta especificada não existe.");
        }



        [Fact]
        public void ValidarSaque_Correto()
        {
            //Arrange           
            string accountId = "1";
            var account = GetAccount(accountId);
            decimal amount = 51;
            var validator = GetValidator();
            var movement = GetMovement(accountId, amount);
            //Act
            _accountRepositoryMock.Setup(p => p.GetAccountById(accountId)).Returns(account);

            var result = validator.TestValidate(movement, x => x.IncludeRuleSets(ValidationRules.Saque));

            //Assert        

            Assert.True(result.IsValid);
        }

        [Fact]
        public void ValidarSaque_ContaCorrenteNaoInformada()
        {
            //Arrange            
            string accountId = "";
            var account = GetAccount(accountId);
            decimal amount = 5;
            var validator = GetValidator();
            var movement = GetMovement(accountId, amount);
            //Act

            var result = validator.TestValidate(movement, x => x.IncludeRuleSets(ValidationRules.Saque));

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.AccountId).WithErrorMessage("Conta Corrente Obrigatória.");
            result.ShouldHaveValidationErrorFor(x => x.Amount).WithErrorMessage("Valor tem que ser superior à 20.");
        }

        [Fact]
        public void ValidarSaque_ContaCorrenteInformadaNaoExiste()
        {
            //Arrange            
            string accountId = "1";
            var account = GetAccount(accountId);
            decimal amount = 5;
            var validator = GetValidator();
            var movement = GetMovement(accountId, amount);
            //Act

            var result = validator.TestValidate(movement, x => x.IncludeRuleSets(ValidationRules.Saque));

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.AccountId).WithErrorMessage("A conta especificada não existe.");
        }
    }
}
