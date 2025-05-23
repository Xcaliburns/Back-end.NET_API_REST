﻿using Findexium.Api.Controllers;
using Findexium.Api.Models;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


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
                new() { Id = 1, MoodysRating = "A1", SandPRating = "A+", FitchRating = "A", OrderNumber = 1 },
                new() { Id = 2, MoodysRating = "A2", SandPRating = "A", FitchRating = "A-", OrderNumber = 2 },
                new() { Id = 3, MoodysRating = "A3", SandPRating = "A-", FitchRating = "BBB+", OrderNumber = 3 }
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
            _mockRatingService.Setup(service => service.GetRatingByIdAsync(rating.Id))
                .ReturnsAsync(rating);

            // Act
            var result = await _controller.GetRating(rating.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnRating = Assert.IsType<RatingResponse>(okResult.Value);
            Assert.Equal(-1, returnRating.Id);
            Assert.Equal("A1", returnRating.MoodysRating);
            Assert.Equal("A+", returnRating.SandPRating);
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
        public async Task PostRating_ValidRequest()
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
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PostRating(request);

            // Assert

            Assert.IsType<CreatedResult>(result);

        }

        [Fact]
        public async Task PostRating_InvalidModelState_WhenRatingIsNull()
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
        public async Task PostRating_ReturnsInternalServerError()
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
            var rating = new Rating
            {
                Id = 1,
                MoodysRating = "A1",
                SandPRating = "A+",
                FitchRating = "A",
                OrderNumber = 1
            };
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
            _mockRatingService.Setup(service => service.GetRatingByIdAsync(1))
                .ReturnsAsync((Rating)null);

            // Act
            var result = await _controller.DeleteRating(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteRating_ReturnsInternalServerError_OnException()
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
            _mockRatingService.Setup(service => service.GetRatingByIdAsync(1))
                .ReturnsAsync(rating);
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
            var id = 1;
            var ratingRequest = new RatingRequest
            {
                MoodysRating = "A1",
                SandPRating = "A+",
                FitchRating = "A",
                OrderNumber = 1
            };

           var existingRating = new Rating
           {
               Id = 1,
               MoodysRating = "A1",
               SandPRating = "A+",
               FitchRating = "A",
               OrderNumber = 1
           };

            _mockRatingService.Setup(service => service.GetRatingByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(existingRating);
            _mockRatingService.Setup(service => service.UpdateRatingAsync(It.IsAny<Rating>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PutRating(id, ratingRequest);

            // Assert
            Assert.IsType<NoContentResult>(result);
            
        }

        [Fact]
        public async Task PutRating_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("MoodysRating", "Required");
            var ratingRequest = new RatingRequest
            {
                MoodysRating = null, // Invalid value to trigger model state error
                SandPRating = "AAA",
                FitchRating = "AAA",
                OrderNumber = 1
            };

            // Act
            var result = await _controller.PutRating(1, ratingRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }




        [Fact]
        public async Task PutRating_ReturnsBadNotFound_WhenRatingDoesntExists()
        {
            // Arrange
            var id = 1;
            var ratingRequest = new RatingRequest
            {
               
                MoodysRating = "A1",
                SandPRating = "A+",
                FitchRating = "A",
                OrderNumber = 1
            };

            _mockRatingService.Setup(service => service.GetRatingByIdAsync(id))
                .ReturnsAsync((Rating)null);

            // Act
            var result = await _controller.PutRating(id, ratingRequest);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
       
       
        [Fact]
        public async Task PutRating_ShouldReturnInternalServerError_WhenExceptionIsThrownAndRatingExists()
        {
            // Arrange
            var id = 1;
            var ratingRequest = new RatingRequest
            {
                MoodysRating = "A1",
                SandPRating = "A+",
                FitchRating = "A",
                OrderNumber = 1
            };

            var rating = ratingRequest.ToRating(); // Conversion en Rating avec l'ID défini
            rating.Id = id;

            _mockRatingService.Setup(service => service.UpdateRatingAsync(It.IsAny<Rating>()))
                .ThrowsAsync(new Exception("Some error"));

            _mockRatingService.Setup(service => service.GetRatingByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(rating); // Retourne une entité existante pour simuler le cas où la mise à jour échoue, mais l'entité existe

            // Act
            var result = await _controller.PutRating(id, ratingRequest);

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }

    }
}
