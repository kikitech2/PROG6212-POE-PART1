using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayManagementAPI.Models;
using RaceDayManagementAPI.RaceDayData;

namespace RaceDayManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RDApplicationDbInfo _context;

        public AuthController(RDApplicationDbInfo context)
        {
            _context = context;
        }

        /// <summary>
        /// Registers a new user with an Organiser or Participant role and hashes their password.
        /// </summary>
        /// <param name="dto">The registration details including name, email, password, and role.</param>
        /// <returns>A confirmation message and the newly generated user ID.</returns>
        /// <response code="200">User registered successfully.</response>
        /// <response code="400">If a user with the provided email already exists.</response>
        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest("User with this email already exists.");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);
            var user = new User
            {
                FirstName = dto.Name,
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Role = string.IsNullOrEmpty(dto.Role) ? "Participant" : dto.Role,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registration successful", userId = user.UserId });
        }

        /// <summary>
        /// Authenticates a user using email and password.
        /// </summary>
        /// <param name="dto">The login credentials.</param>
        /// <returns>User profile details including user ID, name, email, and assigned role.</returns>
        /// <response code="200">Login successful.</response>
        /// <response code="401">If the email or password is invalid.</response>
        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.PasswordHash, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(new
            {
                message = "Login successful",
                userId = user.UserId,
                name = user.FirstName,
                email = user.Email,
                role = user.Role
            });
        }
    }

    public class RegisterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Participant";
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
