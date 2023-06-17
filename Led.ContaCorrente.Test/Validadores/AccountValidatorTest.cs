using FluentValidation.TestHelper;
using Led.ContaCorrente.Domain.Enums.Validadores;
using Led.ContaCorrente.Domain.Models;
using Led.ContaCorrente.Domain.Requests;
using Led.ContaCorrente.DomainService.Validadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Led.ContaCorrente.Test.Validadores
{
    public class AccountValidatorTest
    {
        public AccountValidatorTest()
        {
        }

        public static AccountValidator GetValidator()
        {
            return new AccountValidator();
        }

        static AccountRequest GetAccount(string name, decimal limit)
        {
            return new AccountRequest { Limit = limit, Name = name };
        }

        [Fact]
        private void ValidarNomeNulo()
        {
            //Arrange
            string name = "";
            decimal limit = 1000;

            var account = GetAccount(name, limit);
            var validator = GetValidator();
            //Act

            var result = validator.TestValidate(account, x => x.IncludeRuleSets(ValidationRules.Criar));

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("O nome da conta é obrigatório.");
        }

        [Fact]
        private void ValidarLimiteForaLimitePadrao()
        {
            //Arrange
            string name = "João Messias";
            decimal limit = 25;

            var account = GetAccount(name, limit);
            var validator = GetValidator();
            //Act

            var result = validator.TestValidate(account, x => x.IncludeRuleSets(ValidationRules.Criar));

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Limit).WithErrorMessage("O limite da conta deve ser maior ou igual a 50.");
        }
    }
}
