using BookEcommerceAPI.Data;
using BookEcommerceAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookEcommerceAPI.Controllers
{
    [Route("api/books")]
    [ApiController]

    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BooksController> _logger;

        public BooksController(ApplicationDbContext context, ILogger<BooksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<BookListResponseDTO>> GetBooks(
            [FromQuery] string? search = null,
            [FromQuery] int? category = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] int? rating = null,
            [FromQuery] string? sort = null,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 12)
        {
            try
            {
                _logger.LogInformation($"Getting books with params: search={search}, category={category}, page={page}, limit={limit}");

                var query = _context.Books
                    .Include(b => b.Category)
                    .AsQueryable();

                // Apply search filter
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(b =>
                        b.Title.Contains(search) ||
                        b.Author.Contains(search) ||
                        (b.Description != null && b.Description.Contains(search)));
                }

                // Apply category filter
                if (category.HasValue)
                {
                    query = query.Where(b => b.CategoryId == category.Value);
                }

                // Apply price range filter
                if (minPrice.HasValue)
                {
                    query = query.Where(b => b.Price >= minPrice.Value);
                }

                if (maxPrice.HasValue)
                {
                    query = query.Where(b => b.Price <= maxPrice.Value);
                }

                // Apply rating filter
                if (rating.HasValue)
                {
                    query = query.Where(b => b.Rating >= rating.Value);
                }

                // Apply sorting
                query = sort switch
                {
                    "price_low" => query.OrderBy(b => b.Price),
                    "price_high" => query.OrderByDescending(b => b.Price),
                    "rating" => query.OrderByDescending(b => b.Rating),
                    "bestselling" => query.OrderByDescending(b => b.ReviewCount),
                    _ => query.OrderByDescending(b => b.CreatedAt) // Default: newest
                };

                // Get total count before pagination
                var totalBooks = await query.CountAsync();

                // Apply pagination
                var books = await query
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .Select(b => new BookDTO
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Author = b.Author,
                        Description = b.Description,
                        ISBN = b.ISBN,
                        Price = b.Price,
                        CategoryId = b.CategoryId,
                        CategoryName = b.Category.Name,
                        Publisher = b.Publisher,
                        PublicationDate = b.PublicationDate,
                        Language = b.Language,
                        Pages = b.Pages,
                        CoverImage = b.CoverImage,
                        Rating = b.Rating,
                        ReviewCount = b.ReviewCount,
                        IsNew = b.IsNew,
                        IsFeatured = b.IsFeatured
                    })
                    .ToListAsync();

                _logger.LogInformation($"Found {books.Count} books out of {totalBooks} total");

                return new BookListResponseDTO
                {
                    Books = books,
                    Total = totalBooks
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting books");
                return StatusCode(500, new { message = "An error occurred while retrieving books" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(int id)
        {
            try
            {
                _logger.LogInformation($"Getting book with id: {id}");

                var book = await _context.Books
                    .Include(b => b.Category)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (book == null)
                {
                    _logger.LogWarning($"Book with id {id} not found");
                    return NotFound(new { message = "Book not found" });
                }

                return new BookDTO
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Description = book.Description,
                    ISBN = book.ISBN,
                    Price = book.Price,
                    CategoryId = book.CategoryId,
                    CategoryName = book.Category?.Name,
                    Publisher = book.Publisher,
                    PublicationDate = book.PublicationDate,
                    Language = book.Language,
                    Pages = book.Pages,
                    CoverImage = book.CoverImage,
                    Rating = book.Rating,
                    ReviewCount = book.ReviewCount,
                    IsNew = book.IsNew,
                    IsFeatured = book.IsFeatured
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting book with id: {id}");
                return StatusCode(500, new { message = "An error occurred while retrieving the book" });
            }
        }

        [HttpGet("featured")]
        public async Task<ActionResult<List<BookDTO>>> GetFeaturedBooks()
        {
            try
            {
                _logger.LogInformation("Getting featured books");

                var featuredBooks = await _context.Books
                    .Include(b => b.Category)
                    .Where(b => b.IsFeatured)
                    .OrderByDescending(b => b.Rating)
                    .Take(8)
                    .Select(b => new BookDTO
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Author = b.Author,
                        Description = b.Description,
                        ISBN = b.ISBN,
                        Price = b.Price,
                        CategoryId = b.CategoryId,
                        CategoryName = b.Category.Name,
                        CoverImage = b.CoverImage,
                        Rating = b.Rating,
                        ReviewCount = b.ReviewCount,
                        IsNew = b.IsNew,
                        IsFeatured = b.IsFeatured
                    })
                    .ToListAsync();

                _logger.LogInformation($"Found {featuredBooks.Count} featured books");
                return featuredBooks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting featured books");
                return StatusCode(500, new { message = "An error occurred while retrieving featured books" });
            }
        }

        [HttpGet("new-releases")]
        public async Task<ActionResult<List<BookDTO>>> GetNewReleases()
        {
            try
            {
                _logger.LogInformation("Getting new releases");

                var newReleases = await _context.Books
                    .Include(b => b.Category)
                    .Where(b => b.IsNew)
                    .OrderByDescending(b => b.CreatedAt)
                    .Take(8)
                    .Select(b => new BookDTO
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Author = b.Author,
                        Description = b.Description,
                        ISBN = b.ISBN,
                        Price = b.Price,
                        CategoryId = b.CategoryId,
                        CategoryName = b.Category.Name,
                        CoverImage = b.CoverImage,
                        Rating = b.Rating,
                        ReviewCount = b.ReviewCount,
                        IsNew = b.IsNew,
                        IsFeatured = b.IsFeatured
                    })
                    .ToListAsync();

                _logger.LogInformation($"Found {newReleases.Count} new releases");
                return newReleases;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting new releases");
                return StatusCode(500, new { message = "An error occurred while retrieving new releases" });
            }
        }

        [HttpGet("related")]
        public async Task<ActionResult<List<BookDTO>>> GetRelatedBooks(int bookId, int? category)
        {
            try
            {
                _logger.LogInformation($"Getting related books for bookId: {bookId}, category: {category}");

                var query = _context.Books
                    .Include(b => b.Category)
                    .Where(b => b.Id != bookId);

                if (category.HasValue)
                {
                    query = query.Where(b => b.CategoryId == category.Value);
                }

                var relatedBooks = await query
                    .OrderByDescending(b => b.Rating)
                    .Take(4)
                    .Select(b => new BookDTO
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Author = b.Author,
                        Price = b.Price,
                        CoverImage = b.CoverImage,
                        Rating = b.Rating,
                        ReviewCount = b.ReviewCount,
                        IsNew = b.IsNew
                    })
                    .ToListAsync();

                _logger.LogInformation($"Found {relatedBooks.Count} related books");
                return relatedBooks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting related books for bookId: {bookId}");
                return StatusCode(500, new { message = "An error occurred while retrieving related books" });
            }
        }

        [HttpGet("categories")]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategories()
        {
            try
            {
                _logger.LogInformation("Getting categories");

                var categories = await _context.Categories
                    .Select(c => new CategoryDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    })
                    .ToListAsync();

                _logger.LogInformation($"Found {categories.Count} categories");
                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories");
                return StatusCode(500, new { message = "An error occurred while retrieving categories" });
            }
        }

        [HttpGet("category/{categoryName}")]
        public async Task<ActionResult<BookListResponseDTO>> GetBooksByCategory(
            string categoryName,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 12)
        {
            try
            {
                _logger.LogInformation($"Getting books by category: {categoryName}, page: {page}, limit: {limit}");

                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());

                if (category == null)
                {
                    _logger.LogWarning($"Category {categoryName} not found");
                    return NotFound(new { message = "Category not found" });
                }

                var query = _context.Books
                    .Include(b => b.Category)
                    .Where(b => b.CategoryId == category.Id);

                // Get total count before pagination
                var totalBooks = await query.CountAsync();

                // Apply pagination
                var books = await query
                    .OrderByDescending(b => b.CreatedAt)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .Select(b => new BookDTO
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Author = b.Author,
                        Description = b.Description,
                        ISBN = b.ISBN,
                        Price = b.Price,
                        CategoryId = b.CategoryId,
                        CategoryName = b.Category.Name,
                        CoverImage = b.CoverImage,
                        Rating = b.Rating,
                        ReviewCount = b.ReviewCount,
                        IsNew = b.IsNew,
                        IsFeatured = b.IsFeatured
                    })
                    .ToListAsync();

                _logger.LogInformation($"Found {books.Count} books in category {categoryName}");

                return new BookListResponseDTO
                {
                    Books = books,
                    Total = totalBooks
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting books by category: {categoryName}");
                return StatusCode(500, new { message = "An error occurred while retrieving books by category" });
            }
        }
    }

    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}