namespace BookEcommerceAPI.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string? Description { get; set; }
        public string ISBN { get; set; }
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Publisher { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string? Language { get; set; }
        public int? Pages { get; set; }
        public string? CoverImage { get; set; }
        public decimal Rating { get; set; }
        public int ReviewCount { get; set; }
        public bool IsNew { get; set; }
        public bool IsFeatured { get; set; }
    }

    public class BookListResponseDTO
    {
        public List<BookDTO> Books { get; set; }
        public int Total { get; set; }
    }
}