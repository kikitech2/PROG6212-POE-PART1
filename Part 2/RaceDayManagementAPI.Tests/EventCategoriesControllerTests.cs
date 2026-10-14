using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayManagementAPI.Controllers;
using RaceDayManagementAPI.Models;
using RaceDayManagementAPI.RaceDayData;
using Xunit;

namespace RaceDayManagementAPI.Tests
{
    public class EventCategoriesControllerTests
    {
        private RDApplicationDbInfo GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<RDApplicationDbInfo>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new RDApplicationDbInfo(options);
        }

        [Fact]
        public async Task GetEventCategories_ReturnsAllCategories()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.EventsCategories.Add(new EventCategory { CategoryId = 1, CategoryName = "Senior" });
            context.EventsCategories.Add(new EventCategory { CategoryId = 2, CategoryName = "Junior" });
            await context.SaveChangesAsync();

            var controller = new EventCategoriesController(context);

            // Act
            var result = await controller.GetEventCategories();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<EventCategory>>>(result);
            var categories = Assert.IsAssignableFrom<IEnumerable<EventCategory>>(actionResult.Value);
            Assert.Equal(2, categories.Count());
        }

        [Fact]
        public async Task GetEventCategory_ReturnsCategory_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.EventsCategories.Add(new EventCategory { CategoryId = 1, CategoryName = "Senior" });
            await context.SaveChangesAsync();

            var controller = new EventCategoriesController(context);

            // Act
            var result = await controller.GetEventCategory(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<EventCategory>>(result);
            var category = Assert.IsType<EventCategory>(actionResult.Value);
            Assert.Equal("Senior", category.CategoryName);
        }

        [Fact]
        public async Task GetEventCategory_ReturnsNotFound_WhenDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new EventCategoriesController(context);

            // Act
            var result = await controller.GetEventCategory(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateEventCategory_AddsCategoryAndReturnsCreatedAtAction()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new EventCategoriesController(context);
            var newCategory = new EventCategory { CategoryId = 1, CategoryName = "Masters" };

            // Act
            var result = await controller.CreateEventCategory(newCategory);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdCategory = Assert.IsType<EventCategory>(actionResult.Value);
            Assert.Equal("Masters", createdCategory.CategoryName);
            Assert.Equal(1, context.EventsCategories.Count());
        }

        [Fact]
        public async Task DeleteEventCategory_RemovesCategory_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.EventsCategories.Add(new EventCategory { CategoryId = 1, CategoryName = "Masters" });
            await context.SaveChangesAsync();

            var controller = new EventCategoriesController(context);

            // Act
            var result = await controller.DeleteEventCategory(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, context.EventsCategories.Count());
        }
    }
}
