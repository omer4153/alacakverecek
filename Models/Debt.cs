using System.ComponentModel.DataAnnotations;

namespace alacakverecektakip.Models
{
    public class Debt
    {
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Type { get; set; } = string.Empty; // Alacak / Verecek
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Customer? Customer { get; set; }
    }
}