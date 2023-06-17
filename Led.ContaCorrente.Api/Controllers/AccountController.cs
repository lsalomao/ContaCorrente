using FluentValidation.AspNetCore;
using Led.ContaCorrente.Api.Controllers.Base;
using Led.ContaCorrente.Domain.Abstractions.Services;
using Led.ContaCorrente.Domain.Enums;
using Led.ContaCorrente.Domain.Enums.Validadores;
using Led.ContaCorrente.Domain.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Led.ContaCorrente.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/accounts")]
    public class AccountController : BaseController
    {
        private readonly IAccountService accountService;        

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody, CustomizeValidator(RuleSet = ValidationRules.Criar)] AccountRequest request)
        {
            await Task.CompletedTask;
            var response = accountService.CreateAccount(request.Name, request.Limit);
            return response.PossuiErro ? HandleError(response) : Ok(response.Dados);
        }

        [HttpGet("{accountId}")]
        public IActionResult GetAccount(string accountId)
        {
            var response = accountService.GetAccountById(accountId);
            return response.PossuiErro ? HandleError(response) : Ok(response.Dados);
        }

        [HttpPost("{accountId}/deposit")]
        public async Task<IActionResult> Deposit([FromRoute] string accountId, [FromBody] MovementRequest request)
        {
            var response = await accountService.Deposit(accountId, request.Amount);
            return response.PossuiErro ? HandleError(response) : Ok(response.Dados);
        }

        [HttpPost("{accountId}/withdraw")]
        public IActionResult Withdraw(string accountId, [FromBody] MovementRequest request)
        {
            var response = accountService.Withdraw(accountId, request.Amount);
            return response.PossuiErro ? HandleError(response) : Ok(response.Dados);
        }

        [HttpPost("{sourceAccountId}/transfer/{targetAccountId}")]
        public async Task<IActionResult> Transfer(string sourceAccountId, string targetAccountId, [FromBody] MovementRequest request)
        {
            var response = await accountService.Transfer(sourceAccountId, targetAccountId, request.Amount);
            return response.PossuiErro ? HandleError(response) : Ok(response.Dados);
        }

        [HttpGet("{accountId}/balance")]
        public IActionResult GetBalance(string accountId)
        {
            var response = accountService.GetBalance(accountId);
            return response.PossuiErro ? HandleError(response) : Ok(response.Dados);
        }

        [HttpGet("{accountId}/statement")]
        public IActionResult GetAccountStatementByPeriod(string accountId, [FromQuery] StatementRequest request)
        {
            var response = accountService.GetAccountStatementByPeriod(accountId, request.StartDate, request.EndDate);
            return response.PossuiErro ? HandleError(response) : Ok(response.Dados);
        }

        [HttpGet("{accountId}/statement/{type}")]
        public IActionResult GetAccountStatementByType(string accountId, TipoMovimento type)
        {
            var response = accountService.GetAccountStatementByType(accountId, type);
            return response.PossuiErro ? HandleError(response) : Ok(response.Dados);
        }
    }
}
