using Led.ContaCorrente.Domain.Abstractions.Repository;
using Led.ContaCorrente.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Led.ContaCorrente.DomainRepository
{
    public class MovementRepository : IMovementRepository
    {
        private readonly IMemoryCache cache;

        public MovementRepository(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public void AddMovement(MovementModel movement)
        {
            cache.Set(movement.Id, movement);
        }

        public MovementModel? GetMovement(string movementId)
        {
            return cache.Get<MovementModel>(movementId);
        }

        public IEnumerable<MovementModel> GetMovementsByAccount(AccountModel account)
        {
            List<MovementModel> movements = new();
            account.Movements.ForEach(movementId =>
            {
                var movement = GetMovement(movementId);
                if (movement != null)
                    movements.Add(movement);
            });

            return movements;
        }
    }
}
