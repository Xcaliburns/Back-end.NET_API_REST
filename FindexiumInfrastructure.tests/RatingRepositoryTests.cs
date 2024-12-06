using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Findexium.Infrastructure.Models;
using Findexium.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace FindexiumInfrastructure.tests
{
    public class RatingRepositoryTests
    {
        private readonly DbContextOptions<LocalDbContext> _dbContextOptions;
        private readonly Mock<ILogger<RatingRepository>> _mockLogger;

        public RatingRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<LocalDbContext>()
                .UseInMemoryDatabase(databaseName: "FindexiumTestDb")
                .Options;

            _mockLogger = new Mock<ILogger<RatingRepository>>();
        }

        private RatingRepository CreateRepository(LocalDbContext context)
        {
            return new RatingRepository(context, _mockLogger.Object);
        }

        private LocalDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new LocalDbContext(options);
        }

        private async Task CleanDatabase(LocalDbContext context)
        {
            context.Ratings.RemoveRange(context.Ratings);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllRatings()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_GetallAsync"))
            {
                // Clear existing data
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var ratings = new List<RatingDto>
                    {
                        new RatingDto(1, "A1", "A+", "A", 1),
                        new RatingDto(2, "A2", "A", "A-", 2),
                        new RatingDto(3, "A3", "A-", "BBB+", 3)
                    };

                context.Ratings.AddRange(ratings);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                Assert.Equal(3, result.Count());
                Assert.Equal(1, result.First().Id);
                Assert.Equal("A1", result.First().MoodysRating);
            }
        }

        [Fact]
        public async Task GetAllAsync_WhenExceptionIsThrown()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_GetAllAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<RatingRepository>>();
                var repository = new RatingRepository(context, mockLogger.Object);

                var mockSet = new Mock<DbSet<RatingDto>>();
                mockSet.As<IQueryable<RatingDto>>().Setup(m => m.Provider)
                    .Throws(new Exception("An error occurred while retrieving all ratings."));
                context.Ratings = mockSet.Object;

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.GetAllAsync());
                Assert.Equal("An error occurred while retrieving all ratings.", exception.Message);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsRating()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_GetByIdAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var rating = new RatingDto(1, "A1", "A+", "A", 1);
                context.Ratings.Add(rating);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetByIdAsync(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal("A1", result.MoodysRating);
            }
        }

        [Fact]
        public async Task GetByIdAsync_WhenExceptionIsThrown()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_GetByIdAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<RatingRepository>>();
                var repository = new RatingRepository(context, mockLogger.Object);

                var mockSet = new Mock<DbSet<RatingDto>>();
                mockSet.Setup(m => m.FindAsync(It.IsAny<int>()))
                    .Throws(new Exception("An error occurred while retrieving the rating."));
                context.Ratings = mockSet.Object;

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.GetByIdAsync(1));
                Assert.Equal("An error occurred while retrieving the rating.", exception.Message);
            }
        }

        [Fact]
        public async Task AddAsync_AddsRating()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_AddAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var rating = new Rating
                {
                    Id = 1,
                    MoodysRating = "A1",
                    SandPRating = "A+",
                    FitchRating = "A",
                    OrderNumber = 1
                };

                // Act
                await repository.AddAsync(rating);

                // Assert
                var addedRating = await context.Ratings.FindAsync(1);
                Assert.NotNull(addedRating);
                Assert.Equal("A1", addedRating.MoodysRating);
            }
        }

        [Fact]
        public async Task AddAsync_WhenExceptionIsThrown()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_AddAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<RatingRepository>>();
                var repository = new RatingRepository(context, mockLogger.Object);

                var mockSet = new Mock<DbSet<RatingDto>>();
                mockSet.Setup(m => m.Add(It.IsAny<RatingDto>()))
                    .Throws(new Exception("An error occurred while adding the rating."));
                context.Ratings = mockSet.Object;

                var rating = new Rating
                {
                    Id = 1,
                    MoodysRating = "A1",
                    SandPRating = "A+",
                    FitchRating = "A",
                    OrderNumber = 1
                };

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.AddAsync(rating));
                Assert.Equal("An error occurred while adding the rating.", exception.Message);
            }
        }

        [Fact]
        public async Task UpdateAsync_UpdatesRating()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_UpdateAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var rating = new RatingDto(1, "A1", "A+", "A", 1);
                context.Ratings.Add(rating);
                await context.SaveChangesAsync();

                var updatedRating = new Rating
                {
                    Id = 1,
                    MoodysRating = "A2",
                    SandPRating = "A",
                    FitchRating = "A-",
                    OrderNumber = 2
                };

                // Act
                await repository.UpdateAsync(updatedRating);

                // Assert
                var result = await context.Ratings.FindAsync(1);
                Assert.NotNull(result);
                Assert.Equal("A2", result.MoodysRating);
                Assert.Equal("A", result.SandPRating);
                Assert.Equal("A-", result.FitchRating);
                Assert.Equal(2, result.OrderNumber);
            }
        }

        [Fact]
        public async Task UpdateAsync_WhenExceptionIsThrown()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_UpdateAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<RatingRepository>>();
                var repository = new RatingRepository(context, mockLogger.Object);

                var mockSet = new Mock<DbSet<RatingDto>>();
                mockSet.Setup(m => m.FindAsync(It.IsAny<int>()))
                    .Throws(new Exception("An error occurred while updating the rating."));
                context.Ratings = mockSet.Object;

                var rating = new Rating
                {
                    Id = 1,
                    MoodysRating = "A2",
                    SandPRating = "A",
                    FitchRating = "A-",
                    OrderNumber = 2
                };

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.UpdateAsync(rating));
                Assert.Equal("An error occurred while updating the rating.", exception.Message);
            }
        }

        [Fact]
        public async Task DeleteAsync_DeletesRating()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_DeleteAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var rating = new RatingDto(1, "A1", "A+", "A", 1);
                context.Ratings.Add(rating);
                await context.SaveChangesAsync();

                // Act
                await repository.DeleteAsync(1);

                // Assert
                var deletedRating = await context.Ratings.FindAsync(1);
                Assert.Null(deletedRating);
            }
        }     
      
      
        [Fact]
        public async Task ExistsAsync_WhenExceptionIsThrown()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_ExistsAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<RatingRepository>>();
                var repository = new RatingRepository(context, mockLogger.Object);

                var mockSet = new Mock<DbSet<RatingDto>>();
                mockSet.As<IQueryable<RatingDto>>().Setup(m => m.Provider)
                    .Throws(new Exception("An error occurred while checking if the rating exists."));
                context.Ratings = mockSet.Object;

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.ExistsAsync(1));
                Assert.Equal("An error occurred while checking if the rating exists.", exception.Message);
            }
        }
    }
}
