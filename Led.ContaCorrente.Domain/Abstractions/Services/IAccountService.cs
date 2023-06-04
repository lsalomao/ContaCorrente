using Led.ContaCorrente.Domain.Models;
using Led.ContaCorrente.Domain.Responses.Base;

namespace Led.ContaCorrente.Domain.Abstractions.Services
{
    public interface IAccountService
    {
        Task<Response<AccountModel>> CreateAccount(string name, decimal limit);
        Response<AccountModel> GetAccountById(string accountId);
        Task<Response<MovementModel>> Deposit(string accountId, decimal amount);
        MovementModel Withdraw(string accountId, decimal amount);
        MovementModel Transfer(string sourceAccountId, string destinationAccountId, decimal amount);
        decimal GetBalance(string accountId);
        IEnumerable<MovementModel> GetAccountStatementByPeriod(string accountId, DateTime startDate, DateTime endDate);
        IEnumerable<MovementModel> GetAccountStatementByType(string accountId, string type);
    }
}
