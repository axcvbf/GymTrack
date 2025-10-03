using FluentAssertions;
using GymTrack.Application.DTOs;
using GymTrack.Application.Services;
using GymTrack.Domain.Interfaces;
using GymTrack.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GymTrack.Tests
{
    public class TrainingServiceIntegrationTests
    {
        private readonly DbContextOptions<GymDbContext> _dbOptions;
        public TrainingServiceIntegrationTests()
        {
            _dbOptions = new DbContextOptionsBuilder<GymDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }
        [Fact]
        public async Task GetTrainingDataAsync_WhenTrainingWasSaved_ShouldReturnSavedTraining()
        {
            // Arrange

            using var context = new GymDbContext(_dbOptions);

            var trainingRepo = new TrainingRepository(context);
            var exerciseRepo = new ExerciseRepository(context);

            var userContextMock = new Mock<IUserContext>();
            var userId = Guid.NewGuid().ToString();
            userContextMock.Setup(u => u.GetUserId()).Returns(userId);

            var loggerMock = new Mock<ILogger<TrainingService>>();

            var service = new TrainingService(trainingRepo, exerciseRepo, userContextMock.Object, loggerMock.Object);
            var trainingDto = new TrainingDto
            {
                Date = DateTime.Today,
                Exercises = new[]
                {
                    new ExerciseDto { Name = "Bench Press", Category = "Push", Reps = 10, Weight = 80 },
                    new ExerciseDto { Name = "Pull Up", Category = "Pull", Reps = 10, Weight = 0 },
                    new ExerciseDto { Name = "Leg Press", Category = "Legs", Reps = 10, Weight = 200 }
                }.ToList()
            };


            // Act

            await service.SaveTrainingAsync(trainingDto);

            var result = await service.GetTrainingDataAsync(DateTime.Today);

            // Assert

            result.Should().NotBeNull();
            result.Date.Should().Be(DateTime.Today);
            result.Exercises.Should().HaveCount(3);
            result.Exercises.Should().ContainSingle(e => e.Name == "Bench Press" && e.Reps == 10 && e.Weight == 80);
            result.Exercises.Should().ContainSingle(e => e.Name == "Pull Up" && e.Reps == 10 && e.Weight == 0);
            result.Exercises.Should().ContainSingle(e => e.Name == "Leg Press" && e.Reps == 10 && e.Weight == 200);
        }
        [Fact]
        public async Task GetTrainingDataAsync_WhenNoTrainingExists_ShouldReturnNull()
        {
            // Arrange

            using var context = new GymDbContext(_dbOptions);

            var trainingRepository = new TrainingRepository(context);
            var exerciseRepository = new ExerciseRepository(context);

            var userContextMock = new Mock<IUserContext>();
            var userId = Guid.NewGuid().ToString();
            userContextMock.Setup(u => u.GetUserId()).Returns(userId);

            var loggerMock = new Mock<ILogger<TrainingService>>();
            var service = new TrainingService(trainingRepository, exerciseRepository, userContextMock.Object, loggerMock.Object);

            // Act

            var result = await service.GetTrainingDataAsync(DateTime.Today);

            // Assert

            result.Should().NotBeNull();
            result.Date.Should().Be(DateTime.Today);
            result.Exercises.Should().BeEmpty();
        }
    }
}
