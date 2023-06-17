using FluentValidation.TestHelper;
using Led.ContaCorrente.Domain.Enums.Validadores;
using Led.ContaCorrente.Domain.Requests;
using Led.ContaCorrente.DomainService.Validadores;

namespace Led.ContaCorrente.Test.Validadores
{
    public class MovementValidatorTest
    {
        public MovementValidatorTest() { }

        public static MovementValidator GetValidator()
        {
            return new MovementValidator();
        }

        static AccountRequest GetAccount(string name, decimal limit)
        {
            return new AccountRequest { Limit = limit, Name = name };
        }

        static MovementRequest GetMovement(decimal amount)
        {
            return new MovementRequest { Amount = amount };
        }


        [Fact]
        public void ValidarDeposito()
        {
            //Arrange            
            decimal amount = 5;
            var validator = GetValidator();
            var movement = GetMovement(amount);
            //Act

            var result = validator.TestValidate(movement, x => x.IncludeRuleSets(ValidationRules.Deposito));

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Amount).WithErrorMessage("Valor tem que ser superior à 10.");
        }

        [Fact]
        public void ValidarTransferencia()
        {
            //Arrange            
            decimal amount = 50;
            var validator = GetValidator();
            var movement = GetMovement(amount);
            //Act

            var result = validator.TestValidate(movement, x => x.IncludeRuleSets(ValidationRules.Transfencia));

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Amount).WithErrorMessage("Valor tem que ser superior à 50.");
        }

        [Fact]
        public void ValidarSaque()
        {
            //Arrange            
            decimal amount = 15;
            var validator = GetValidator();
            var movement = GetMovement(amount);
            //Act

            var result = validator.TestValidate(movement, x => x.IncludeRuleSets(ValidationRules.Saque));

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Amount).WithErrorMessage("Valor tem que ser superior à 20.");
        }
    }
}
