namespace Led.ContaCorrente.Domain.Requests
{
    public class MovementRequest
    {
        public string AccountId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
