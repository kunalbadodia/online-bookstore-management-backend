using System.ComponentModel.DataAnnotations;

namespace BookEcommerceAPI.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        // Navigation property
        public ICollection<Book> Books { get; set; }
    }
}