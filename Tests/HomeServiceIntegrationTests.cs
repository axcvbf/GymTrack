using FluentAssertions;
using GymTrack.Application.Services;
using GymTrack.Domain.Entities;
using GymTrack.Domain.Interfaces;
using GymTrack.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace GymTrack.Tests
{
    public class HomeServiceIntegrationTests
    {
        private readonly DbContextOptions<GymDbContext> _dbOptions;
        public HomeServiceIntegrationTests()
        {
            _dbOptions = new DbContextOptionsBuilder<GymDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetHomeDataAsync_WhenTrainingsExist_ShouldReturnDates()
        {
            // Arrange

            using var context = new GymDbContext(_dbOptions);

            var userId = Guid.NewGuid().ToString();
            context.Trainings.Add(new Training { Date = new DateTime(2025, 9, 1), GymUserId = userId });
            context.Trainings.Add(new Training { Date = new DateTime(2025, 9, 15), GymUserId = userId });
            await context.SaveChangesAsync();

            var trainingRepository = new TrainingRepository(context);

            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(u => u.GetUserId()).Returns(userId);

            var loggerMock = new Mock<ILogger<HomeService>>();
            var service = new HomeService(userContextMock.Object, trainingRepository, loggerMock.Object);

            // Act

            var result = await service.GetHomeDataAsync(9, 2025);

            // Assert

            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.Trainingdays.Should().Contain(new DateTime(2025, 9, 1));
            result.Trainingdays.Should().Contain(new DateTime(2025, 9, 15));
            result.Trainingdays.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetHomeDataAsync_WhenNoTrainingsExist_ShouldReturnEmptyList()
        {
            // Arrange

            using var context = new GymDbContext(_dbOptions);

            var userId = Guid.NewGuid().ToString();
            var trainingRepository = new TrainingRepository(context);

            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(u => u.GetUserId()).Returns(userId);

            var loggerMock = new Mock<ILogger<HomeService>>();
            var service = new HomeService(userContextMock.Object, trainingRepository, loggerMock.Object);

            // Act

            var result = await service.GetHomeDataAsync(9, 2025);

            // Assert

            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.Trainingdays.Should().BeEmpty();

        }
    }
}
