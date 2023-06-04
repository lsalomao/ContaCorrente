﻿namespace Led.ContaCorrente.Domain.Models
{
    public class MovementModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AccountId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
