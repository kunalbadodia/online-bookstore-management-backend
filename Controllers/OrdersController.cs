using BookEcommerceAPI.Data;
using BookEcommerceAPI.DTOs;
using BookEcommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookEcommerceAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponseDTO>> CreateOrder(CreateOrderDTO createOrderDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Generate order number
            var orderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

            // Create new order
            var order = new Order
            {
                UserId = userId,
                OrderNumber = orderNumber,
                ShippingFirstName = createOrderDto.ShippingAddress.FirstName,
                ShippingLastName = createOrderDto.ShippingAddress.LastName,
                ShippingAddress = createOrderDto.ShippingAddress.Address,
                ShippingCity = createOrderDto.ShippingAddress.City,
                ShippingState = createOrderDto.ShippingAddress.State,
                ShippingZipCode = createOrderDto.ShippingAddress.ZipCode,
                Subtotal = createOrderDto.Items.Sum(i => i.Price * i.Quantity),
                Tax = createOrderDto.TotalAmount * 0.08m, // 8% tax
                Shipping = createOrderDto.TotalAmount > 120 ? 0 : 4.99m, // Free shipping over 120rs
                Total = createOrderDto.TotalAmount,
                OrderItems = new List<OrderItem>()
            };

            // Add order items
            foreach (var item in createOrderDto.Items)
            {
                var book = await _context.Books.FindAsync(item.BookId);
                if (book == null)
                {
                    return BadRequest(new { message = $"Book with ID {item.BookId} not found" });
                }

                order.OrderItems.Add(new OrderItem
                {
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return await GetOrderResponse(order.Id);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDTO>> GetOrder(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            // Check if the order belongs to the current user
            if (order.UserId != userId)
            {
                return Forbid();
            }

            return await GetOrderResponse(id);
        }

        [HttpGet("user")]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetUserOrders()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var orderDtos = new List<OrderResponseDTO>();
            foreach (var order in orders)
            {
                orderDtos.Add(await GetOrderResponse(order.Id));
            }

            return orderDtos;
        }

        private async Task<OrderResponseDTO> GetOrderResponse(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return new OrderResponseDTO
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                Status = order.Status,
                ShippingAddress = new ShippingAddressDTO
                {
                    FirstName = order.ShippingFirstName,
                    LastName = order.ShippingLastName,
                    Address = order.ShippingAddress,
                    City = order.ShippingCity,
                    State = order.ShippingState,
                    ZipCode = order.ShippingZipCode
                },
                Items = order.OrderItems.Select(oi => new OrderItemResponseDTO
                {
                    Id = oi.Id,
                    BookId = oi.BookId,
                    Title = oi.Book.Title,
                    Author = oi.Book.Author,
                    CoverImage = oi.Book.CoverImage,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList(),
                Subtotal = order.Subtotal,
                Tax = order.Tax,
                Shipping = order.Shipping,
                Total = order.Total,
                Email = order.User.Email
            };
        }
    }
}