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

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody, CustomizeValidator(RuleSet = ValidationRules.Deposito)] MovementRequest request)
        {
            var response = await accountService.Deposit(request);
            return response.PossuiErro ? HandleError(response) : Ok(response.Dados);
        }

        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody, CustomizeValidator(RuleSet = ValidationRules.Saque)] MovementRequest request)
        {
            var response = accountService.Withdraw(request);
            return response.PossuiErro ? HandleError(response) : Ok(response.Dados);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody, CustomizeValidator(RuleSet = ValidationRules.Transfencia)] TransferRequest request)
        {
            var response = await accountService.Transfer(request);
            return response.PossuiErro ? HandleError(response) : Ok(response.Dados);
        }

        [HttpGet("{accountId}/balance")]
        public IActionResult GetBalance(string accountId)
        {
            var response = accountService.GetBalance(accountId);
            return response.PossuiErro ? HandleError(response) : Ok(response.Dados);
        }

        [HttpGet("{accountId}/statement")]
        public IActionResult GetAccountStatementByPeriod([FromRoute] string accountId, [FromQuery] StatementRequest request)
        {
            var response = accountService.GetAccountStatementByPeriod(accountId, request.StartDate, request.EndDate);
            return response.PossuiErro ? HandleError(response) : Ok(response.Dados);
        }

        [HttpGet("{accountId}/statement/{type}")]
        public IActionResult GetAccountStatementByType([FromRoute] string accountId, TipoMovimento type)
        {
            var response = accountService.GetAccountStatementByType(accountId, type);
            return response.PossuiErro ? HandleError(response) : Ok(response.Dados);
        }
    }
}
