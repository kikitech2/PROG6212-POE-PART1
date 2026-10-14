using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayManagementAPI.Controllers;
using RaceDayManagementAPI.Models;
using RaceDayManagementAPI.RaceDayData;
using Xunit;

namespace RaceDayManagementAPI.Tests
{
    public class EventsControllerTests
    {
        private RDApplicationDbInfo GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<RDApplicationDbInfo>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new RDApplicationDbInfo(options);
        }

        [Fact]
        public async Task GetEvents_ReturnsAllEvents()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Events.Add(new Event { EventId = 1, Name = "City Marathon" });
            context.Events.Add(new Event { EventId = 2, Name = "Trail Run" });
            await context.SaveChangesAsync();

            var controller = new EventsController(context);

            // Act
            var result = await controller.GetEvents();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Event>>>(result);
            var events = Assert.IsAssignableFrom<IEnumerable<Event>>(actionResult.Value);
            Assert.Equal(2, events.Count());
        }

        [Fact]
        public async Task GetEvent_ReturnsEvent_WhenEventExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Events.Add(new Event { EventId = 1, Name = "City Marathon" });
            await context.SaveChangesAsync();

            var controller = new EventsController(context);

            // Act
            var result = await controller.GetEvent(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Event>>(result);
            var eventObj = Assert.IsType<Event>(actionResult.Value);
            Assert.Equal("City Marathon", eventObj.Name);
        }

        [Fact]
        public async Task GetEvent_ReturnsNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new EventsController(context);

            // Act
            var result = await controller.GetEvent(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateEvent_AddsEventAndReturnsCreatedAtAction()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new EventsController(context);
            var newEvent = new Event { EventId = 1, Name = "10k Night Run" };

            // Act
            var result = await controller.CreateEvent(newEvent);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdEvent = Assert.IsType<Event>(actionResult.Value);
            Assert.Equal("10k Night Run", createdEvent.Name);
            Assert.Equal(1, context.Events.Count());
        }

        [Fact]
        public async Task DeleteEvent_RemovesEvent_WhenEventExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Events.Add(new Event { EventId = 1, Name = "10k Night Run" });
            await context.SaveChangesAsync();

            var controller = new EventsController(context);

            // Act
            var result = await controller.DeleteEvent(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, context.Events.Count());
        }
    }
}
