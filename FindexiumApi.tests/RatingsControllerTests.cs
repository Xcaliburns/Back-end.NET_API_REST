using System.Collections.Generic;
using System.Threading.Tasks;
using Findexium.Api.Controllers;
using Findexium.Api.Models;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Microsoft.AspNetCore.Http;
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
            var result = await _controller.GetRatings();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnRatings = Assert.IsType<List<RatingResponse>>(okResult.Value);
            Assert.Equal(3, returnRatings.Count);
            Assert.Collection(returnRatings,
                item => Assert.Equal(1, item.Id),
                item => Assert.Equal(2, item.Id),
                item => Assert.Equal(3, item.Id));
        }

        [Fact]
        public async Task GetRatings_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var mockRatingServices = new Mock<IRatingServices>();

            _mockRatingService.Setup(service => service.GetAllRatingsAsync())
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetRatings();

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }

        [Fact]
        public async Task GetRating_ById_ReturnsOkResult_WithRating()
        {
            // Arrange
            var rating = new Rating
            {
                Id = -1,
                MoodysRating = "A1",
                SandPRating = "A+",
                FitchRating = "A",
                OrderNumber = 1
            };
            _mockRatingService.Setup(service => service.GetRatingByIdAsync(-1)).ReturnsAsync(rating);

            // Act
            var result = await _controller.GetRating(-1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnRating = Assert.IsType<Rating>(okResult.Value);
            Assert.Equal(-1, returnRating.Id);
        }
        [Fact]
        public async Task GetRating_ReturnsNotFound_WhenRatingIsNull()
        {
            // Arrange
            int testId = 1;
            _mockRatingService.Setup(service => service.GetRatingByIdAsync(testId))
                .ReturnsAsync((Rating)null);

            // Act
            var result = await _controller.GetRating(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetRating_ReturnsInternalServerError_OnException()
        {
            // Arrange
            int testId = 1;
            _mockRatingService.Setup(service => service.GetRatingByIdAsync(testId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetRating(testId);

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }

        [Fact]
        public async Task PostRating_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var request = new RatingRequest
            {
                MoodysRating = "A1",
                SandPRating = "A+",
                FitchRating = "A",
                OrderNumber = 1
            };

            _mockRatingService.Setup(service => service.AddRatingAsync(It.IsAny<Rating>())).Returns(Task.CompletedTask);
            // _mockRatingService.Setup(service=> service)
            // Act
            var result = await _controller.PostRating(request);

            // Assert

            Assert.IsType<CreatedResult>(result);

        }
        [Fact]
        public async Task PostRating_ReturnsBadRequestResult_WhenRatingIsNull()
        {
            // Arrange
            _controller.ModelState.AddModelError("MoodysRating", "Required");
            var request = new RatingRequest
            {
                MoodysRating = null,
                SandPRating = "A+",
                FitchRating = "A",
                OrderNumber = 1
            };

            // Act
            var result = await _controller.PostRating(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PostRating_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var request = new RatingRequest
            {
                MoodysRating = "A1",
                SandPRating = "A+",
                FitchRating = "A",
                OrderNumber = 1
            };

            _mockRatingService.Setup(service => service.AddRatingAsync(It.IsAny<Rating>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.PostRating(request);

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }


        [Fact]
        public async Task DeleteRating_ReturnsNoContentResult()
        {
            // Arrange
            var rating = new Rating { Id = 1, MoodysRating = "A1", SandPRating = "A+", FitchRating = "A", OrderNumber = 1 };
            _mockRatingService.Setup(service => service.GetRatingByIdAsync(1)).ReturnsAsync(rating);
            _mockRatingService.Setup(service => service.DeleteRatingAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteRating(rating.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteRating_ReturnsNotFound_WhenRatingIsNull()
        {
            // Arrange
            _mockRatingService.Setup(service => service.GetRatingByIdAsync(1)).ReturnsAsync((Rating)null);

            // Act
            var result = await _controller.DeleteRating(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteRating_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var rating = new Rating { Id = 1, MoodysRating = "A1", SandPRating = "A+", FitchRating = "A", OrderNumber = 1 };
            _mockRatingService.Setup(service => service.GetRatingByIdAsync(1)).ReturnsAsync(rating);
            _mockRatingService.Setup(service => service.DeleteRatingAsync(1))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.DeleteRating(1);

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }

        [Fact]
        public async Task PutRating_ReturnsNoContentResult()
        {
            // Arrange
            var rating = new Rating
            {
                Id = 1,
                MoodysRating = "A1",
                SandPRating = "A+",
                FitchRating = "A",
                OrderNumber = 1
            };
            _mockRatingService.Setup(service => service.UpdateRatingAsync( rating)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PutRating(1, rating);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task PutRating_ReturnsBadRequestResult_WhenIdMismatch()
        {
            // Arrange
            var ratingId = 1;
            var rating = new Rating { Id = 2 };

            // Act
            var result = await _controller.PutRating(ratingId, rating);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutRating_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
        {
            // Arrange
            var rating = new Rating
            {
                Id = 1,
                MoodysRating = "A1",
                SandPRating = "A+",
                FitchRating = "A",
                OrderNumber = 1
            };

            _mockRatingService.Setup(service => service.UpdateRatingAsync(It.IsAny<Rating>()))
                .ThrowsAsync(new ArgumentException("Invalid argument"));

            // Act
            var result = await _controller.PutRating(rating.Id, rating);

            // Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid argument", actionResult.Value);
        }
        [Fact]
        public async Task PutRating_ShouldReturnNotFound_WhenExceptionIsThrownAndRatingDoesNotExist()
        {
            // Arrange
            var rating = new Rating
            {
                Id = 1,
                MoodysRating = "A1",
                SandPRating = "A+",
                FitchRating = "A",
                OrderNumber = 1
            };

            _mockRatingService.Setup(service => service.UpdateRatingAsync(It.IsAny<Rating>()))
                .ThrowsAsync(new Exception("Some error"));

            _mockRatingService.Setup(service => service.GetRatingByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Rating)null);

            // Act
            var result = await _controller.PutRating(rating.Id, rating);

            // Assert
            var actionResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutRating_ShouldReturnInternalServerError_WhenExceptionIsThrownAndRatingExists()
        {
            // Arrange
            var rating = new Rating
            {
                Id = 1,
                MoodysRating = "A1",
                SandPRating = "A+",
                FitchRating = "A",
                OrderNumber = 1
            };

            _mockRatingService.Setup(service => service.UpdateRatingAsync(It.IsAny<Rating>()))
                .ThrowsAsync(new Exception("Some error"));

            _mockRatingService.Setup(service => service.GetRatingByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(rating);

            // Act
            var result = await _controller.PutRating(rating.Id, rating);

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }
    } 
}
