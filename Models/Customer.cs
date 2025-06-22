using System.ComponentModel.DataAnnotations;

namespace alacakverecektakip.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

}