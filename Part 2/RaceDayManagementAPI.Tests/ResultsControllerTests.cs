using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayManagementAPI.Controllers;
using RaceDayManagementAPI.Models;
using RaceDayManagementAPI.RaceDayData;
using Xunit;

namespace RaceDayManagementAPI.Tests
{
    public class ResultsControllerTests
    {
        private RDApplicationDbInfo GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<RDApplicationDbInfo>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new RDApplicationDbInfo(options);
        }

        [Fact]
        public async Task GetResults_ReturnsAllResults()
        {
            // Arrange
            using var context = GetInMemoryDbContext();

            // Seed navigation entities required by .ThenInclude()
            var user1 = new User();
            var user2 = new User();
            var evt1 = new Event { EventId = 1 };
            var evt2 = new Event { EventId = 2 };

            var enrolment1 = new Enrolment { EnrolmentId = 1, EventId = 1, ParticipantId = 1, Participant = user1, Event = evt1 };
            var enrolment2 = new Enrolment { EnrolmentId = 2, EventId = 2, ParticipantId = 2, Participant = user2, Event = evt2 };

            context.Enrolments.AddRange(enrolment1, enrolment2);
            context.Results.Add(new Result { ResultId = 1, FinishingPosition = 1, FinishTime = TimeSpan.FromHours(1), EnrolmentId = 1, Enrolment = enrolment1 });
            context.Results.Add(new Result { ResultId = 2, FinishingPosition = 2, FinishTime = TimeSpan.FromHours(1.1), EnrolmentId = 2, Enrolment = enrolment2 });
            await context.SaveChangesAsync();

            var controller = new ResultsController(context);

            // Act
            var actionResult = await controller.GetResults();

            // Assert
            var results = Assert.IsAssignableFrom<IEnumerable<Result>>(actionResult.Value);
            Assert.Equal(2, results.Count());
        }

        [Fact]
        public async Task GetResult_ReturnsResult_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();

            var user = new User();
            var evt = new Event { EventId = 1 };
            var enrolment = new Enrolment { EnrolmentId = 1, EventId = 1, ParticipantId = 1, Participant = user, Event = evt };

            context.Enrolments.Add(enrolment);
            context.Results.Add(new Result { ResultId = 1, FinishingPosition = 1, FinishTime = TimeSpan.FromHours(1), EnrolmentId = 1, Enrolment = enrolment });
            await context.SaveChangesAsync();

            var controller = new ResultsController(context);

            // Act
            var actionResult = await controller.GetResult(1);

            // Assert
            var result = Assert.IsType<Result>(actionResult.Value);
            Assert.Equal(1, result.FinishingPosition);
        }

        [Fact]
        public async Task GetResult_ReturnsNotFound_WhenDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new ResultsController(context);

            // Act
            var actionResult = await controller.GetResult(99);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task PostResult_AddsResultAndReturnsCreatedAtAction()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new ResultsController(context);
            var newResult = new Result { ResultId = 1, FinishingPosition = 3, FinishTime = TimeSpan.FromHours(1.2) };

            // Act
            var actionResult = await controller.PostResult(newResult);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var resultObj = Assert.IsType<Result>(createdResult.Value);
            Assert.Equal(3, resultObj.FinishingPosition);
            Assert.Equal(1, context.Results.Count());
        }

        [Fact]
        public async Task DeleteResult_RemovesResult_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Results.Add(new Result { ResultId = 1, FinishingPosition = 1 });
            await context.SaveChangesAsync();

            var controller = new ResultsController(context);

            // Act
            var result = await controller.DeleteResult(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, context.Results.Count());
        }
    }
}
