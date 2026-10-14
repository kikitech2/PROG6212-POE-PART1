using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayManagementAPI.Controllers;
using RaceDayManagementAPI.Models;
using RaceDayManagementAPI.RaceDayData;
using Xunit;

namespace RaceDayManagementAPI.Tests
{
    public class AuthControllerTests
    {
        private RDApplicationDbInfo GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<RDApplicationDbInfo>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new RDApplicationDbInfo(options);
        }

        [Fact]
        public async Task Register_CreatesUser_WhenValid()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new AuthController(context);

            var registerDto = new RegisterDto
            {
                Name = "John",
                Email = "john@example.com",
                PasswordHash = "Password123!",
                Role = "Participant"
            };

            // Act
            var result = await controller.Register(registerDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, context.Users.Count());
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenEmailAlreadyExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Users.Add(new User { Email = "john@example.com", PasswordHash = "hashed" });
            await context.SaveChangesAsync();

            var controller = new AuthController(context);

            var registerDto = new RegisterDto
            {
                Name = "John",
                Email = "john@example.com",
                PasswordHash = "Password123!"
            };

            // Act
            var result = await controller.Register(registerDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsValid()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var password = "Password123!";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            context.Users.Add(new User
            {
                UserId = 1,
                FirstName = "John",
                Email = "john@example.com",
                PasswordHash = hashedPassword,
                Role = "Participant"
            });
            await context.SaveChangesAsync();

            var controller = new AuthController(context);

            var loginDto = new LoginDto
            {
                Email = "john@example.com",
                PasswordHash = password
            };

            // Act
            var result = await controller.Login(loginDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsInvalid()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new AuthController(context);

            var loginDto = new LoginDto
            {
                Email = "nonexistent@example.com",
                PasswordHash = "WrongPassword"
            };

            // Act
            var result = await controller.Login(loginDto);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}
