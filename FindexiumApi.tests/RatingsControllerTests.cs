using System.Collections.Generic;
using System.Threading.Tasks;
using Findexium.Api.Controllers;
using Findexium.Api.Models;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
namespace FindexiumApi.tests
{
    public class RatingsControllerTests
    {
        private readonly Mock<IRatingServices> _mockRatingService;
        private readonly Mock<ILogger<RatingsController>> _mockLogger;
        private readonly RatingsController _controller;

        public RatingsControllerTests()
        {
            _mockRatingService = new Mock<IRatingServices>();
            _mockLogger = new Mock<ILogger<RatingsController>>();
            _controller = new RatingsController(_mockRatingService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetRating_ReturnsOkResult_WithListOfThreeRatings()
        {
            // Arrange
            var ratings = new List<Rating>
            {
                new Rating { Id = 1, MoodysRating = "A1", SandPRating = "A+", FitchRating = "A", OrderNumber = 1 },
                new Rating { Id = 2, MoodysRating = "A2", SandPRating = "A", FitchRating = "A-", OrderNumber = 2 },
                new Rating { Id = 3, MoodysRating = "A3", SandPRating = "A-", FitchRating = "BBB+", OrderNumber = 3 }
            };
            _mockRatingService.Setup(service => service.GetAllRatingsAsync()).ReturnsAsync(ratings);

            // Act
            var result = await _controller.GetRating();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnRatings = Assert.IsType<List<Rating>>(okResult.Value);
            Assert.Equal(3, returnRatings.Count);
            Assert.Collection(returnRatings,
                item => Assert.Equal(1, item.Id),
                item => Assert.Equal(2, item.Id),
                item => Assert.Equal(3, item.Id));
        }
        [Fact]
        public async Task GetRating_ById_ReturnsOkResult_WithRating()
        {
            // Arrange
            var rating = new Rating { Id = 1, MoodysRating = "A1", SandPRating = "A+", FitchRating = "A", OrderNumber = 1 };
            _mockRatingService.Setup(service => service.GetRatingByIdAsync(1)).ReturnsAsync(rating);

            // Act
            var result = await _controller.GetRating(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnRating = Assert.IsType<Rating>(okResult.Value);
            Assert.Equal(1, returnRating.Id);
        }
        [Fact]
        public async Task PostRating_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var request = new RatingRequest { MoodysRating = "A1", SandPrating = "A+", FitchRating = "A", OrderNumber = 1 };
            var rating = request.ToRating();
            _mockRatingService.Setup(service => service.AddRatingAsync(It.IsAny<Rating>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PostRating(request);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnRating = Assert.IsType<RatingRequest>(createdAtActionResult.Value);
            Assert.Equal(request.MoodysRating, returnRating.MoodysRating);
        }
        [Fact]
        public async Task DeleteRating_ReturnsNoContentResult()
        {
            // Arrange
            var rating = new Rating { Id = 1, MoodysRating = "A1", SandPRating = "A+", FitchRating = "A", OrderNumber = 1 };
            _mockRatingService.Setup(service => service.GetRatingByIdAsync(1)).ReturnsAsync(rating);
            _mockRatingService.Setup(service => service.DeleteRatingAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteRating(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task PutRating_ReturnsNoContentResult()
        {
            // Arrange
            var rating = new Rating { Id = 1, MoodysRating = "A1", SandPRating = "A+", FitchRating = "A", OrderNumber = 1 };
            _mockRatingService.Setup(service => service.UpdateRatingAsync(1, rating)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PutRating(1, rating);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
