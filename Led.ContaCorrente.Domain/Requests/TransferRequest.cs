namespace Led.ContaCorrente.Domain.Requests
{
    public class TransferRequest
    {
        public string SourceAccountId { get; set; } = string.Empty;
        public string TargetAccountId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
