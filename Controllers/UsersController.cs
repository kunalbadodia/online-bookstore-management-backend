using BookEcommerceAPI.Data;
using BookEcommerceAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookEcommerceAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserDTO>> GetUserProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Address = user.Address,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode
            };
        }

        [HttpPut("profile")]
        public async Task<ActionResult<UserDTO>> UpdateUserProfile(UpdateProfileDTO updateProfileDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Check if email is already taken by another user
            if (updateProfileDto.Email != user.Email)
            {
                var emailExists = await _context.Users.AnyAsync(u => u.Email == updateProfileDto.Email && u.Id != userId);
                if (emailExists)
                {
                    return BadRequest(new { message = "Email is already in use" });
                }
            }

            // Update password if provided
            if (!string.IsNullOrEmpty(updateProfileDto.CurrentPassword) && !string.IsNullOrEmpty(updateProfileDto.NewPassword))
            {
                // Verify current password
                if (!BCrypt.Net.BCrypt.Verify(updateProfileDto.CurrentPassword, user.PasswordHash))
                {
                    return BadRequest(new { message = "Current password is incorrect" });
                }

                // Update password
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateProfileDto.NewPassword);
            }

            // Update user profile
            user.FirstName = updateProfileDto.FirstName;
            user.LastName = updateProfileDto.LastName;
            user.Email = updateProfileDto.Email;
            user.Address = updateProfileDto.Address;
            user.City = updateProfileDto.City;
            user.State = updateProfileDto.State;
            user.ZipCode = updateProfileDto.ZipCode;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Address = user.Address,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode
            };
        }
    }
}