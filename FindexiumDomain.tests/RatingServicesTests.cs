using System.Collections.Generic;
using System.Threading.Tasks;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Domain.Services;
using Moq;
using Xunit;

namespace FindexiumDomain.tests
{
    public class RatingServicesTests
    {
        private readonly Mock<IRatingRepository> _mockRatingRepository;
        private readonly IRatingServices _ratingServices;

        public RatingServicesTests()
        {
            _mockRatingRepository = new Mock<IRatingRepository>();
            _ratingServices = new RatingService(_mockRatingRepository.Object);
        }

        [Fact]
        public async Task GetAllRatingsAsync_ReturnsListOfThreeRatings()
        {
            // Arrange
            var ratings = new List<Rating>
                    {
                        new Rating { Id = 1, MoodysRating = "A1", SandPRating = "A+", FitchRating = "A", OrderNumber = 1 },
                        new Rating { Id = 2, MoodysRating = "A2", SandPRating = "A", FitchRating = "A-", OrderNumber = 2 },
                        new Rating { Id = 3, MoodysRating = "A3", SandPRating = "A-", FitchRating = "BBB+", OrderNumber = 3 }
                    };
            _mockRatingRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(ratings);

            // Act
            var result = (await _ratingServices.GetAllRatingsAsync()).ToList();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Collection(result,
                item => Assert.Equal(1, item.Id),
                item => Assert.Equal(2, item.Id),
                item => Assert.Equal(3, item.Id));
        }

        [Fact]
        public async Task GetRatingByIdAsync_ReturnsRating()
        {
            // Arrange
            var rating = new Rating { Id = 1, MoodysRating = "A1", SandPRating = "A+", FitchRating = "A", OrderNumber = 1 };
            _mockRatingRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(rating);

            // Act
            var result = await _ratingServices.GetRatingByIdAsync(1);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("A1", result.MoodysRating);
        }

        [Fact]
        public async Task AddRatingAsync_AddsRating()
        {
            // Arrange
            var rating = new Rating { Id = 1, MoodysRating = "A1", SandPRating = "A+", FitchRating = "A", OrderNumber = 1 };
            _mockRatingRepository.Setup(repo => repo.AddAsync(rating)).Returns(Task.CompletedTask);

            // Act
            await _ratingServices.AddRatingAsync(rating);

            // Assert
            _mockRatingRepository.Verify(repo => repo.AddAsync(rating), Times.Once);
        }

        [Fact]
        public async Task UpdateRatingAsync_UpdatesRating()
        {
            // Arrange
            var rating = new Rating { Id = 1, MoodysRating = "A1", SandPRating = "A+", FitchRating = "A", OrderNumber = 1 };
            _mockRatingRepository.Setup(repo => repo.ExistsAsync(1)).ReturnsAsync(true);
            _mockRatingRepository.Setup(repo => repo.UpdateAsync(rating)).Returns(Task.CompletedTask);

            // Act
            await _ratingServices.UpdateRatingAsync(rating);

            // Assert
            _mockRatingRepository.Verify(repo => repo.UpdateAsync(rating), Times.Once);
        }

        [Fact]
        public async Task DeleteRatingAsync_DeletesRating()
        {
            // Arrange
            _mockRatingRepository.Setup(repo => repo.ExistsAsync(1)).ReturnsAsync(true);
            _mockRatingRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _ratingServices.DeleteRatingAsync(1);

            // Assert
            _mockRatingRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }
    }
}
