using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Findexium.Infrastructure.Models;
using Findexium.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FindexiumInfrastructure.tests
{
    public class RatingRepositoryTests
    {
        private readonly DbContextOptions<LocalDbContext> _dbContextOptions;

        public RatingRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<LocalDbContext>()
                .UseInMemoryDatabase(databaseName: "FindexiumTestDb")
                .Options;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllRatings()
        {
            // Arrange
            using (var context = new LocalDbContext(_dbContextOptions))
            {
                // Clear existing data
                context.Ratings.RemoveRange(context.Ratings);
                await context.SaveChangesAsync();

                context.Ratings.AddRange(
                      new RatingDto(1, "A1", "A+", "A", 1),
                      new RatingDto(2, "A2", "A", "A-", 2),
                      new RatingDto(3, "A3", "A-", "BBB+", 3)
                );
                await context.SaveChangesAsync();
            }

            using (var context = new LocalDbContext(_dbContextOptions))
            {
                var repository = new RatingRepository(context);

                // Act
                var result = (await repository.GetAllAsync()).ToList();

                // Assert
                Assert.Equal(3, result.Count);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsRating()
        {
            // Arrange
            using (var context = new LocalDbContext(_dbContextOptions))
            {
                context.Ratings.Add(new RatingDto(-1, "A1", "A+", "A", 1));
                await context.SaveChangesAsync();
            }

            using (var context = new LocalDbContext(_dbContextOptions))
            {
                var repository = new RatingRepository(context);

                // Act
                var result = await repository.GetByIdAsync(-1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(-1, result.Id);
                Assert.Equal("A1", result.MoodysRating);
            }
        }

        [Fact]
        public async Task AddAsync_AddsRating()
        {
            // Arrange
            var rating = new Rating { Id = -1, MoodysRating = "A1", SandPRating = "A+", FitchRating = "A", OrderNumber = 1 };

            using (var context = new LocalDbContext(_dbContextOptions))
            {
                var repository = new RatingRepository(context);

                // Act
                await repository.AddAsync(rating);
                await context.SaveChangesAsync();
            }

            using (var context = new LocalDbContext(_dbContextOptions))
            {
                // Assert
                Assert.Equal(1, context.Ratings.Count());
                Assert.Equal("A1", context.Ratings.First().MoodysRating);
            }
        }

        [Fact]
        public async Task UpdateAsync_UpdatesRating()
        {
            // Arrange
            using (var context = new LocalDbContext(_dbContextOptions))
            {
                // Clear existing data
                context.Ratings.RemoveRange(context.Ratings);
                await context.SaveChangesAsync();

                context.Ratings.Add(new RatingDto(-1, "A1", "A+", "A", 1));
                await context.SaveChangesAsync();
            }

            using (var context = new LocalDbContext(_dbContextOptions))
            {
                var repository = new RatingRepository(context);
                var rating = await repository.GetByIdAsync(-1);
                rating.MoodysRating = "A2";

                // Act
                await repository.UpdateAsync(rating);
                await context.SaveChangesAsync();
            }

            using (var context = new LocalDbContext(_dbContextOptions))
            {
                // Assert
                Assert.Equal("A2", context.Ratings.First().MoodysRating);
            }
        }

        [Fact]
        public async Task DeleteAsync_DeletesRating()
        {
            // Arrange
            using (var context = new LocalDbContext(_dbContextOptions))
            {
                // Clear existing data
                context.Ratings.RemoveRange(context.Ratings);
                await context.SaveChangesAsync();

                context.Ratings.Add(new RatingDto(-1, "A1", "A+", "A", 1)); // Utilisation d'un identifiant unique
                await context.SaveChangesAsync();
            }

            using (var context = new LocalDbContext(_dbContextOptions))
            {
                var repository = new RatingRepository(context);

                // Act
                await repository.DeleteAsync(-1); // Utilisation du même identifiant unique
                await context.SaveChangesAsync();
            }

            using (var context = new LocalDbContext(_dbContextOptions))
            {
                // Assert
                Assert.Equal(0, context.Ratings.Count());
            }
        }
    }
}
