using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace BookEcommerceAPI.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        public string? Description { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public int? CategoryId { get; set; }

        public string? Publisher { get; set; }

        public DateTime? PublicationDate { get; set; }

        public string? Language { get; set; }

        public int? Pages { get; set; }

        public string? CoverImage { get; set; }

        [Column(TypeName = "decimal(3, 1)")]
        public decimal Rating { get; set; } = 0;

        public int ReviewCount { get; set; } = 0;

        public bool IsNew { get; set; } = false;

        public bool IsFeatured { get; set; } = false;

        public int InStock { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Category? Category { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}