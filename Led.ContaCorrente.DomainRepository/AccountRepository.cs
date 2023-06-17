using Led.ContaCorrente.Domain.Abstractions.Repository;
using Led.ContaCorrente.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Led.ContaCorrente.DomainRepository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IMemoryCache _cache;

        public AccountRepository(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void AddAccount(AccountModel account)
        {
            _cache.Set(account.Id, account);
        }

        public void UpdateAccount(AccountModel account)
        {
            _cache.Set(account.Id, account);
        }

        public AccountModel? GetAccountById(string accountId)
        {
            _cache.TryGetValue(accountId, out AccountModel? account);
            return account;
        }
    }
}
