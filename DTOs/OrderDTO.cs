namespace BookEcommerceAPI.DTOs
{
    public class CreateOrderDTO
    {
        public ShippingAddressDTO ShippingAddress { get; set; }
        public List<OrderItemDTO> Items { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class OrderItemDTO
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class ShippingAddressDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    public class OrderResponseDTO
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public ShippingAddressDTO ShippingAddress { get; set; }
        public List<OrderItemResponseDTO> Items { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Shipping { get; set; }
        public decimal Total { get; set; }
        public string Email { get; set; }
    }

    public class OrderItemResponseDTO
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string CoverImage { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}