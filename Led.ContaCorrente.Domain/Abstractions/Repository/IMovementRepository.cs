using Led.ContaCorrente.Domain.Models;

namespace Led.ContaCorrente.Domain.Abstractions.Repository
{
    public interface IMovementRepository
    {
        void AddMovement(MovementModel movement);
        MovementModel? GetMovement(string movementId);
        IEnumerable<MovementModel> GetMovementsByAccount(AccountModel account);
    }
}
