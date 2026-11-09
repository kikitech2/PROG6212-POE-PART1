using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayManagementAPI.Controllers;
using RaceDayManagementAPI.Models;
using RaceDayManagementAPI.RaceDayData;
using Xunit;

namespace RaceDayManagementAPI.Tests
{
    public class RoutesControllerTests
    {
        private RDApplicationDbInfo GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<RDApplicationDbInfo>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new RDApplicationDbInfo(options);
        }

        [Fact]
        public async Task GetRoutes_ReturnsAllRoutes()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Routes.Add(new RouteInfo { RouteId = 1, StartPoint = "Main Gate", EndPoint = "North Park", TerrainType = "Paved" });
            context.Routes.Add(new RouteInfo { RouteId = 2, StartPoint = "South Beach", EndPoint = "Lighthouse", TerrainType = "Sand" });
            await context.SaveChangesAsync();

            var controller = new RoutesController(context);

            // Act
            var result = await controller.GetRoutes();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<RouteInfo>>>(result);
            var routes = Assert.IsAssignableFrom<IEnumerable<RouteInfo>>(actionResult.Value);
            Assert.Equal(2, routes.Count());
        }

        [Fact]
        public async Task GetRoute_ReturnsRoute_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Routes.Add(new RouteInfo { RouteId = 1, StartPoint = "Main Gate", EndPoint = "North Park", TerrainType = "Paved" });
            await context.SaveChangesAsync();

            var controller = new RoutesController(context);

            // Act
            var result = await controller.GetRoute(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<RouteInfo>>(result);
            var route = Assert.IsType<RouteInfo>(actionResult.Value);
            Assert.Equal("Main Gate", route.StartPoint);
        }

        [Fact]
        public async Task GetRoute_ReturnsNotFound_WhenDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new RoutesController(context);

            // Act
            var result = await controller.GetRoute(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateRoute_AddsRouteAndReturnsCreatedAtAction()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new RoutesController(context);
            var newRoute = new RouteInfo { RouteId = 1, StartPoint = "City Center", EndPoint = "Stadium", TerrainType = "Asphalt" };

            // Act
            var result = await controller.CreateRoute(newRoute);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdRoute = Assert.IsType<RouteInfo>(actionResult.Value);
            Assert.Equal("City Center", createdRoute.StartPoint);
            Assert.Equal(1, context.Routes.Count());
        }

        [Fact]
        public async Task DeleteRoute_RemovesRoute_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Routes.Add(new RouteInfo { RouteId = 1, StartPoint = "City Center", EndPoint = "Stadium", TerrainType = "Asphalt" });
            await context.SaveChangesAsync();

            var controller = new RoutesController(context);

            // Act
            var result = await controller.DeleteRoute(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, context.Routes.Count());
        }
    }
}
