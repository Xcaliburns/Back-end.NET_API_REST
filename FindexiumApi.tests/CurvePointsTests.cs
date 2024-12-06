using System.Collections.Generic;
using System.Threading.Tasks;
using Findexium.Api.Controllers;
using Findexium.Api.Models;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;


namespace FindexiumApi.tests
{
    public class CurvePointsTests
    {
        private readonly Mock<ICurvePointServices> _mockCurvePointService;
        private readonly Mock<ILogger<CurvePointsController>> _mockLogger;
        private readonly CurvePointsController _controller;

        public CurvePointsTests()
        {
            _mockCurvePointService = new Mock<ICurvePointServices>();
            _mockLogger = new Mock<ILogger<CurvePointsController>>();
            _controller = new CurvePointsController(_mockCurvePointService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetCurvePoints_ReturnsOkResult()
        {
            // Arrange
            var curvepoints = new List<CurvePoint>
                {
                    new CurvePoint{Id = 1, CurveId = 1, AsOfDate = DateTime.Now, Term = 1, CurvePointValue = 1, CreationDate = DateTime.Now},
                    new CurvePoint{Id = 2, CurveId = 2, AsOfDate = DateTime.Now, Term = 2, CurvePointValue = 2, CreationDate = DateTime.Now},
                    new CurvePoint{Id = 3, CurveId = 3, AsOfDate = DateTime.Now, Term = 3, CurvePointValue = 3, CreationDate = DateTime.Now}
                };
            _mockCurvePointService.Setup(service => service.GetAllAsync()).ReturnsAsync(curvepoints);

            // Act
            var result = await _controller.GetCurvePoints();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnCurvePoints = Assert.IsType<List<CurvePointResponse>>(okResult.Value);
            Assert.Equal(3, returnCurvePoints.Count);
            Assert.Collection(returnCurvePoints,
                item => Assert.Equal(1, item.Id),
                item => Assert.Equal(2, item.Id),
                item => Assert.Equal(3, item.Id)
            );
        }

        [Fact]
        public async Task GetCurvePoints_ReturnsInternalServerError()
        {
            // Arrange
            _mockCurvePointService.Setup(service => service.GetAllAsync())
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetCurvePoints();

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }

        [Fact]
        public async Task GetCurvePoint_ReturnsOkResult()
        {
            // Arrange
            var curvePoint = new CurvePoint
            {
                Id = 1,
                CurveId = 1,
                Term = 1,
                CurvePointValue = 1,
               
            };

            _mockCurvePointService.Setup(service => service.GetByIdAsync(curvePoint.Id))
                .ReturnsAsync(curvePoint);

            // Act
            var result = await _controller.GetCurvePoint(curvePoint.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnCurvePoint = Assert.IsType<CurvePointResponse>(okResult.Value);
            Assert.Equal(curvePoint.Id, returnCurvePoint.Id);
            Assert.Equal(curvePoint.CurveId, returnCurvePoint.CurveId);
            Assert.Equal(curvePoint.Term, returnCurvePoint.Term);
            Assert.Equal(curvePoint.CurvePointValue, returnCurvePoint.CurvePointValue);
        }

        [Fact]
        public async Task GetCurvePoint_ReturnsNotFoundWhenCurvePointIsNull()
        {
            // Arrange
            int testId = 1;
            _mockCurvePointService.Setup(service => service.GetByIdAsync(testId))
                .ReturnsAsync((CurvePoint?)null);

            // Act
            var result = await _controller.GetCurvePoint(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetCurvePoint_ReturnsInternalServerError()
        {
            // Arrange
            int testId = 1;
            _mockCurvePointService.Setup(service => service.GetByIdAsync(testId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetCurvePoint(testId);

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, (result.Result as ObjectResult)?.StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }

        [Fact]
        public async Task PutCurvePoint_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var id = 1;
            var curvePointRequest = new CurvePointRequest
            {
                CurveId = 1,
                Term = 1.0,
                CurvePointValue = 1.0
            };

            _controller.ModelState.AddModelError("CurveId", "Required");

            // Act
            var result = await _controller.PutCurvePoint(id, curvePointRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task PutCurvePoint_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var id = 1;
            var curvePointRequest = new CurvePointRequest
            {
                CurveId = 1,
                Term = 1.0,
                CurvePointValue = 1.0
            };

            var existingCurvePoint = new CurvePoint
            {
                Id = id,
                CurveId = 1,
                AsOfDate = DateTime.Now,
                Term = 1.0,
                CurvePointValue = 1.0,
                CreationDate = DateTime.Now
            };

            _mockCurvePointService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync(existingCurvePoint);
            _mockCurvePointService.Setup(service => service.UpdateAsync(It.IsAny<CurvePoint>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PutCurvePoint(id, curvePointRequest);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            _mockCurvePointService.Verify(service => service.UpdateAsync(It.Is<CurvePoint>(cp => cp.Id == id && cp.CurveId == curvePointRequest.CurveId && cp.Term == curvePointRequest.Term && cp.CurvePointValue == curvePointRequest.CurvePointValue)), Times.Once);
        }

        [Fact]
        public async Task PutCurvePoint_ReturnsNotFound_WhenBidDoesNotExist()
        {
            // Arrange
            var curvePointId = 1;
            var curvePointRequest = new CurvePointRequest
            {
                CurveId = 2,
                Term = 1.0,
                CurvePointValue = 1.0
            };
            _mockCurvePointService.Setup(service => service.UpdateAsync(It.IsAny<CurvePoint>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PutCurvePoint(curvePointId, curvePointRequest);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutCurvePoint_DbUpdateConcurrencyException_CurvePointNotFound_ReturnsNotFound()
        {
            // Arrange
            var curvePointRequest = new CurvePointRequest
            {
                CurveId = 1,
                Term = 1.0,
                CurvePointValue = 1.0
            };
            _mockCurvePointService.Setup(service => service.UpdateAsync(It.IsAny<CurvePoint>()))
                .ThrowsAsync(new DbUpdateConcurrencyException());

            // Act
            var result = await _controller.PutCurvePoint(1, curvePointRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

      
        [Fact]
        public async Task PutCurvePoint_ReturnsInternalServerError()
        {
            // Arrange
            var id = 1;
            var curvePointRequest = new CurvePointRequest
            {
                CurveId = 1,
                Term = 1.0,
                CurvePointValue = 1.0
            };

            var existingCurvePoint = new CurvePoint
            {
                Id = id,
                CurveId = 1,              
                Term = 1.0,
                CurvePointValue = 1.0,
               
            };

            _mockCurvePointService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync(existingCurvePoint);
            _mockCurvePointService.Setup(service => service.UpdateAsync(It.IsAny<CurvePoint>())).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.PutCurvePoint(id, curvePointRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Internal server error", statusCodeResult.Value);
        }

        [Fact]
        public async Task PostCurvePoint_ValidRequest()
        {
            // Arrange
            var request = new CurvePointRequest
            {
                CurveId = 1,             
                Term = 1.0,
                CurvePointValue = 100.0,              
            };

            _mockCurvePointService.Setup(service => service.AddAsync(It.IsAny<CurvePoint>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PostCurvePoint(request);

            // Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task PostCurvePoint_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("CurveId", "Required");

            var request = new CurvePointRequest
            {
               
                Term = 1.0,
                CurvePointValue = 100.0,
              
            };

            // Act
            var result = await _controller.PostCurvePoint(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task PostCurvePoint_ReturnsInternalServerError()
        {
            // Arrange
            var request = new CurvePointRequest
            {
                CurveId = 1,              
                Term = 1.0,
                CurvePointValue = 100.0,              
            };

            _mockCurvePointService.Setup(service => service.AddAsync(It.IsAny<CurvePoint>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = await _controller.PostCurvePoint(request);

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }

        [Fact]
        public async Task DeleteCurvePoint_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var curvePoint = new CurvePoint { Id = 1 };
            _mockCurvePointService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(curvePoint);
            _mockCurvePointService.Setup(service => service.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCurvePoint(1);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }

        [Fact]
        public async Task DeleteCurvePoint_ReturnsNotFound_WhenRatingIsNull()
        {
            // Arrange
            _mockCurvePointService.Setup(service => service.GetByIdAsync(1))
                .ReturnsAsync((CurvePoint?)null);

            // Act
            var result = await _controller.DeleteCurvePoint(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteCurvePoint_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            _mockCurvePointService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(new CurvePoint { Id = 1 });
            _mockCurvePointService.Setup(service => service.DeleteAsync(1)).Throws(new Exception("Test exception"));

            // Act
            var result = await _controller.DeleteCurvePoint(1);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
        }
    }
}
