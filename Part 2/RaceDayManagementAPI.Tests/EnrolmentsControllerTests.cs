using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayManagementAPI.Controllers;
using RaceDayManagementAPI.Models;
using RaceDayManagementAPI.RaceDayData;
using Xunit;

namespace RaceDayManagementAPI.Tests
{
    public class EnrolmentsControllerTests
    {
        private RDApplicationDbInfo GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<RDApplicationDbInfo>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new RDApplicationDbInfo(options);
        }

        [Fact]
        public async Task GetEnrolments_ReturnsAllEnrolments()
        {
            // Arrange
            using var context = GetInMemoryDbContext();

            var user1 = new User();
            var user2 = new User();
            var evt1 = new Event { EventId = 1 };
            var evt2 = new Event { EventId = 2 };

            var enrolment1 = new Enrolment { EnrolmentId = 1, EventId = 1, ParticipantId = 1, Participant = user1, Event = evt1 };
            var enrolment2 = new Enrolment { EnrolmentId = 2, EventId = 2, ParticipantId = 2, Participant = user2, Event = evt2 };

            context.Enrolments.AddRange(enrolment1, enrolment2);
            await context.SaveChangesAsync();

            var controller = new EnrolmentsController(context);

            // Act
            var actionResult = await controller.GetEnrolments();

            // Assert
            var enrolments = Assert.IsAssignableFrom<IEnumerable<Enrolment>>(actionResult.Value);
            Assert.Equal(2, enrolments.Count());
        }

        [Fact]
        public async Task PostEnrolment_AddsEnrolmentAndReturnsCreatedAtAction()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new EnrolmentsController(context);
            var newEnrolment = new Enrolment { EnrolmentId = 1, EventId = 1, ParticipantId = 1 };

            // Act
            var actionResult = await controller.PostEnrolment(newEnrolment);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var enrolmentObj = Assert.IsType<Enrolment>(createdResult.Value);
            Assert.Equal(1, enrolmentObj.EnrolmentId);
            Assert.Equal(1, context.Enrolments.Count());
        }

        [Fact]
        public async Task DeleteEnrolment_RemovesEnrolment_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Enrolments.Add(new Enrolment { EnrolmentId = 1, EventId = 1, ParticipantId = 1 });
            await context.SaveChangesAsync();

            var controller = new EnrolmentsController(context);

            // Act
            var result = await controller.DeleteEnrolment(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, context.Enrolments.Count());
        }

        [Fact]
        public async Task DeleteEnrolment_ReturnsNotFound_WhenDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new EnrolmentsController(context);

            // Act
            var result = await controller.DeleteEnrolment(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
