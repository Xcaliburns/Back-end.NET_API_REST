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
            //Arrange
            var curvepoints = new List<CurvePoint>
            {
                new CurvePoint{Id = 1, CurveId = 1, AsOfDate = DateTime.Now, Term = 1, CurvePointValue = 1, CreationDate = DateTime.Now},
                new CurvePoint{Id = 2, CurveId = 2, AsOfDate = DateTime.Now, Term = 2, CurvePointValue = 2, CreationDate = DateTime.Now},
                new CurvePoint{Id = 3, CurveId = 3, AsOfDate = DateTime.Now, Term = 3, CurvePointValue = 3, CreationDate = DateTime.Now}
            };
            _mockCurvePointService.Setup(service => service.GetAllAsync()).ReturnsAsync(curvepoints);

            //Act
            var result = await _controller.GetCurvePoints();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnCurvePoints = Assert.IsType<List<CurvePoint>>(okResult.Value);
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
            //Arrange
            _mockCurvePointService.Setup(service => service.GetAllAsync()).
                ThrowsAsync(new Exception("Test exception"));

            //Act
            var result = await _controller.GetCurvePoints();

            //Assert
            var actionResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, (result.Result as ObjectResult).StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }

        [Fact]
        public async Task GetCurvePoint_ReturnsOkResult()
        {
            //Arrange
            var curvePoint = new CurvePoint
            { 
                Id = -1,
                CurveId = 1,
                AsOfDate = DateTime.Now,
                Term = 1,
                CurvePointValue = 1,
                CreationDate = DateTime.Now
            };
            _mockCurvePointService.Setup(service => service.GetByIdAsync(curvePoint.Id)).ReturnsAsync(curvePoint);

            //Act
            var result = await _controller.GetCurvePoint(curvePoint.Id);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnCurvePoint = Assert.IsType<CurvePoint>(okResult.Value);
            Assert.Equal(curvePoint.Id, returnCurvePoint.Id);
        }
        [Fact]
        public async Task GetCurvePoint_ReturnsNotFoundWhenCurvePointIsNull()
        {
            //Arrange
            int testId = 1;
            _mockCurvePointService.Setup(service => service.GetByIdAsync(testId))
                .ReturnsAsync((CurvePoint)null);

            //Act
            var result = await _controller.GetCurvePoint(testId);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetCurvePoint_ReturnsInternalServerError()
        {
            //Arrange
            int testId = 1;
            _mockCurvePointService.Setup(service => service.GetByIdAsync(testId))
                .ThrowsAsync(new Exception("Test exception"));

            //Act
            var result = await _controller.GetCurvePoint(testId);

            //Assert
            var actionResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, (result.Result as ObjectResult).StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }



    }
}
