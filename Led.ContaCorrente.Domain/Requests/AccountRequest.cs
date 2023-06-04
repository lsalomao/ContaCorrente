namespace Led.ContaCorrente.Domain.Requests
{
    public class AccountRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal Limit { get; set; }
    }
}
