using Led.ContaCorrente.Domain.Enums;
using Led.ContaCorrente.Domain.Models;
using Led.ContaCorrente.Domain.Responses.Base;

namespace Led.ContaCorrente.Domain.Abstractions.Services
{
    public interface IAccountService
    {
        Response<AccountModel> CreateAccount(string name, decimal limit);
        Response<AccountModel> GetAccountById(string accountId);
        Task<Response<MovementModel>> Deposit(string accountId, decimal amount);
        Response<MovementModel> Withdraw(string accountId, decimal amount);
        Task<Response<MovementModel>> Transfer(string sourceAccountId, string destinationAccountId, decimal amount);
        Response<decimal> GetBalance(string accountId);
        Response<IEnumerable<MovementModel>> GetAccountStatementByPeriod(string accountId, DateTime startDate, DateTime endDate);
        Response<IEnumerable<MovementModel>> GetAccountStatementByType(string accountId, TipoMovimento type);
    }
}
