using FluentAssertions;
using GymTrack.Application.Services;
using GymTrack.Domain.Entities;
using GymTrack.Domain.Interfaces;
using GymTrack.Infrastructure.Persistence.Repositories;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace GymTrack.Tests
{
    public class StatsServiceIntegrationTests
    {
        private readonly DbContextOptions<GymDbContext> _dbOptions;
        public StatsServiceIntegrationTests()
        {
            _dbOptions = new DbContextOptionsBuilder<GymDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }
        private StatsService CreateService(GymDbContext context, string userId)
        {
            var trainingRepository = new TrainingRepository(context);
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(u => u.GetUserId()).Returns(userId);
            var loggerMock = new Mock<ILogger<StatsService>>();

            return new StatsService(trainingRepository, userContextMock.Object, loggerMock.Object);
        }


        [Fact]

        public async Task GetUserStatsAsync_WhenTrainingsExist_ShouldReturnAggregatedStats()
        {
            // Arrange

            using var context = new GymDbContext(_dbOptions);
            var userId = Guid.NewGuid().ToString();

            var training1 = new Training
            {
                GymUserId = userId,
                Date = DateTime.Today,
                Exercises = new List<ExerciseData>
                {
                    new ExerciseData { ExerciseId = 1, Reps = 10, Weight = 80 },
                    new ExerciseData { ExerciseId = 2, Reps = 10, Weight = 0 },
                }
            };
            var training2 = new Training
            {
                GymUserId = userId,
                Date = DateTime.Today.AddDays(-1),
                Exercises = new List<ExerciseData>
                {
                    new ExerciseData { ExerciseId = 1, Reps = 10, Weight = 90 },
                    new ExerciseData { ExerciseId = 2, Reps = 10, Weight = 10 },
                }
            };

            context.Trainings.AddRange(training1, training2);
            await context.SaveChangesAsync();

            var service = CreateService(context, userId);

            // Act

            var result = await service.GetUserStatsAsync();

            // Assert

            result.TrainingsCount.Should().Be(2);
            result.SetsCount.Should().Be(4);
            result.RepsCount.Should().Be(40);
            result.TotalWeight.Should().Be(1800);

        }

        [Fact]

        public async Task GetUserStatsAsync_WhenNoTrainingsExist_ShouldReturnZeros()
        {
            // Arrange

            using var context = new GymDbContext(_dbOptions);
            var userId = Guid.NewGuid().ToString();

            var service = CreateService(context, userId);

            // Act

            var result = await service.GetUserStatsAsync();

            // Assert

            result.Should().NotBeNull();
            result.TrainingsCount.Should().Be(0);
            result.SetsCount.Should().Be(0);
            result.RepsCount.Should().Be(0);
            result.TotalWeight.Should().Be(0);
        }

        [Fact]

        public async Task GetExerciseProgressAsync_WhenTrainingsExist_ShouldReturnOrderedProgress()
        {
            // Arrange

            using var context = new GymDbContext(_dbOptions);
            var userId = Guid.NewGuid().ToString();

            var training1 = new Training
            {
                GymUserId = userId,
                Date = new DateTime(2025, 9, 15),
                Exercises = new List<ExerciseData>
                {
                    new ExerciseData { ExerciseId = 1, Reps = 10, Weight = 90 },
                }
            };
            var training2 = new Training
            {
                GymUserId = userId,
                Date = new DateTime(2025, 9, 1),
                Exercises = new List<ExerciseData>
                {
                    new ExerciseData { ExerciseId = 1, Reps = 10, Weight = 80 },
                }
            };
            context.Trainings.AddRange(training1, training2);
            await context.SaveChangesAsync();

            var service = CreateService(context, userId);

            // Act

            var result = await service.GetExerciseProgressAsync(1);

            // Assert

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Date.Should().Be(new DateTime(2025, 9, 1));
            result[0].Weight.Should().Be(80);
            result[1].Date.Should().Be(new DateTime(2025, 9, 15));
            result[1].Weight.Should().Be(90);
            
        }

        [Fact]

        public async Task GetStatsDataAsync_WhenTrainingsExist_ShouldReturnCombinedStatsDto()
        {
            // Arrange

            using var context = new GymDbContext(_dbOptions);
            var userId = Guid.NewGuid().ToString();


            var training1 = new Training
            {
                GymUserId = userId,
                Date = new DateTime(2025, 9, 15),
                Exercises = new List<ExerciseData>
                {
                    new ExerciseData { ExerciseId = 1, Reps = 10, Weight = 90 },
                    new ExerciseData { ExerciseId = 2, Reps = 10, Weight = 10 },
                    new ExerciseData { ExerciseId = 3, Reps = 10, Weight = 220 },
                }
            };
            var training2 = new Training
            {
                GymUserId = userId,
                Date = new DateTime(2025, 9, 1),
                Exercises = new List<ExerciseData>
                {
                    new ExerciseData { ExerciseId = 1, Reps = 10, Weight = 80 },
                    new ExerciseData { ExerciseId = 2, Reps = 10, Weight = 0 },
                    new ExerciseData { ExerciseId = 3, Reps = 10, Weight = 200 },
                }
            };
            context.Trainings.AddRange(training1, training2);
            await context.SaveChangesAsync();

            var service = CreateService(context, userId);

            // Act

            var result = await service.GetStatsDataAsync();

            // Assert

            result.Should().NotBeNull();
            result.UserStats.TrainingsCount.Should().Be(2);
            result.Benchpress.Should().HaveCount(2);
            result.Shoulderpress.Should().HaveCount(2);
            result.Incline.Should().HaveCount(2);
        }
    }
}
