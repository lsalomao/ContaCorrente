using Led.ContaCorrente.Domain.Models;

namespace Led.ContaCorrente.Domain.Abstractions.Repository
{
    public interface IAccountRepository
    {
        void AddAccount(AccountModel account);
        void UpdateAccount(AccountModel account);
        AccountModel? GetAccountById(string accountId);
    }
}
