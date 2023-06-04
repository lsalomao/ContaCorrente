namespace Led.ContaCorrente.Domain.Models
{
    public class AccountModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public decimal Limit { get; set; }
        public List<string> Movements { get; set; } = new List<string>();
    }
}
