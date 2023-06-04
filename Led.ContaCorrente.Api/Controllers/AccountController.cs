using FluentValidation.AspNetCore;
using Led.ContaCorrente.Api.Controllers.Base;
using Led.ContaCorrente.Domain.Abstractions.Services;
using Led.ContaCorrente.Domain.Requests;
using Led.ContaCorrente.Domain.Responses.Base;
using Microsoft.AspNetCore.Mvc;

namespace Led.ContaCorrente.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/accounts")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] AccountRequest request)
        {
            var response = await _accountService.CreateAccount(request.Name, request.Limit);

            return response.PossuiErro ? HandleError(response) : Ok(response.Dados);
        }

        [HttpGet("{accountId}")]
        public IActionResult GetAccount(string accountId)
        {
            var response = _accountService.GetAccountById(accountId);

            return response.PossuiErro ? HandleError(response) : Ok(response.Dados);
        }

        [HttpPost("{accountId}/deposit")]
        public async  Task<IActionResult> Deposit([FromRoute] string accountId, [FromBody] MovementRequest request)
        {
            var response = await _accountService.Deposit(accountId, request.Amount);
            return response.PossuiErro ? HandleError(response) : Ok(response.Dados);
        }

        [HttpPost("{accountId}/withdraw")]
        public IActionResult Withdraw(string accountId, [FromBody] MovementRequest request)
        {
            var movement = _accountService.Withdraw(accountId, request.Amount);
            return Ok(movement);
        }

        [HttpPost("{sourceAccountId}/transfer/{targetAccountId}")]
        public IActionResult Transfer(string sourceAccountId, string targetAccountId, [FromBody] MovementRequest request)
        {
            var movement = _accountService.Transfer(sourceAccountId, targetAccountId, request.Amount);
            return Ok(movement);
        }

        [HttpGet("{accountId}/balance")]
        public IActionResult GetBalance(string accountId)
        {
            var balance = _accountService.GetBalance(accountId);
            return Ok(new { Balance = balance });
        }

        [HttpGet("{accountId}/statement")]
        public IActionResult GetAccountStatementByPeriod(string accountId, [FromQuery] StatementRequest request)
        {
            var statement = _accountService.GetAccountStatementByPeriod(accountId, request.StartDate, request.EndDate);
            return Ok(statement);
        }

        [HttpGet("{accountId}/statement/{type}")]
        public IActionResult GetAccountStatementByType(string accountId, string type)
        {
            var statement = _accountService.GetAccountStatementByType(accountId, type);
            return Ok(statement);
        }
    }
}
