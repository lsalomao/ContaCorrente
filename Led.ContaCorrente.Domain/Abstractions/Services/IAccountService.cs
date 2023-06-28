using Led.ContaCorrente.Domain.Enums;
using Led.ContaCorrente.Domain.Models;
using Led.ContaCorrente.Domain.Requests;
using Led.ContaCorrente.Domain.Responses.Base;

namespace Led.ContaCorrente.Domain.Abstractions.Services
{
    public interface IAccountService
    {
        Response<AccountModel> CreateAccount(string name, decimal limit);
        Response<AccountModel> GetAccountById(string accountId);
        Task<Response<MovementModel>> Deposit(MovementRequest request);
        Response<MovementModel> Withdraw(MovementRequest request);
        Task<Response<MovementModel>> Transfer(TransferRequest request);
        Response<decimal> GetBalance(string accountId);
        Response<IEnumerable<MovementModel>> GetAccountStatementByPeriod(string accountId, DateTime startDate, DateTime endDate);
        Response<IEnumerable<MovementModel>> GetAccountStatementByType(string accountId, TipoMovimento type);
    }
}
