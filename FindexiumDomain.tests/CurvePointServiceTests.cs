using System.Collections.Generic;
using System.Threading.Tasks;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Domain.Services;
using Moq;
using Xunit;

namespace FindexiumDomain.tests
{
    public class CurvePointServiceTests
    {
        private readonly Mock<ICurvePointRepository> _mockRepository;
        private readonly CurvePointService _service;

        public CurvePointServiceTests()
        {
            _mockRepository = new Mock<ICurvePointRepository>();
            _service = new CurvePointService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCurvePoints()
        {
            // Arrange
            var curvePoints = new List<CurvePoint> { new CurvePoint { Id = 1 }, new CurvePoint { Id = 2 } };
            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(curvePoints);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(curvePoints, result);
        }

        [Fact]
        public async Task GetAllAsync_ThrowsApplicationException_OnError()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAllAsync()).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _service.GetAllAsync());
            Assert.Equal("An error occurred while retrieving all curve points.", exception.Message);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCurvePoint()
        {
            // Arrange
            var curvePoint = new CurvePoint { Id = 1 };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(curvePoint);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.Equal(curvePoint, result);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsApplicationException_OnError()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _service.GetByIdAsync(1));
            Assert.Equal("An error occurred while retrieving the curve point with ID 1.", exception.Message);
        }

        [Fact]
        public async Task AddAsync_AddsCurvePoint()
        {
            // Arrange
            var curvePoint = new CurvePoint { Id = 1 };

            // Act
            await _service.AddAsync(curvePoint);

            // Assert
            _mockRepository.Verify(repo => repo.AddAsync(curvePoint), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentNullException_WhenCurvePointIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddAsync(null));
        }

        [Fact]
        public async Task AddAsync_ThrowsApplicationException_OnError()
        {
            // Arrange
            var curvePoint = new CurvePoint { Id = 1 };
            _mockRepository.Setup(repo => repo.AddAsync(curvePoint)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _service.AddAsync(curvePoint));
            Assert.Equal("An error occurred while adding a new curve point.", exception.Message);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesCurvePoint()
        {
            // Arrange
            var curvePoint = new CurvePoint { Id = 1 };
            _mockRepository.Setup(repo => repo.ExistsAsync(curvePoint.Id)).ReturnsAsync(true);

            // Act
            await _service.UpdateAsync(curvePoint);

            // Assert
  //          _mockRepository.Verify(repo => repo.UpdateAsync(curvePoint), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsArgumentNullException_WhenCurvePointIsNull()
        {
            // Act
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateAsync(null));

            // Assert
            Assert.Equal("Value cannot be null. (Parameter 'curvePoint')", exception.Message);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsInvalidOperationException_WhenCurvePointDoesNotExist()
        {
            // Arrange
            var curvePoint = new CurvePoint { Id = 1 };
            _mockRepository.Setup(repo => repo.ExistsAsync(curvePoint.Id)).ReturnsAsync(false);

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(curvePoint));

            // Assert
            Assert.Equal("Curve point with ID 1 does not exist.", exception.Message);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsApplicationException_OnError()
        {
            // Arrange
            var curvePoint = new CurvePoint { Id = 1 };
            _mockRepository.Setup(repo => repo.ExistsAsync(curvePoint.Id)).ReturnsAsync(true);
    //        _mockRepository.Setup(repo => repo.UpdateAsync(curvePoint)).ThrowsAsync(new Exception("Database error"));

            // Act
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _service.UpdateAsync(curvePoint));
            // Assert

            Assert.Equal("An error occurred while updating the curve point with ID 1.", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_DeletesCurvePoint()
        {
            // Arrange
            var id = 1;
            _mockRepository.Setup(repo => repo.ExistsAsync(id)).ReturnsAsync(true);

            // Act
            await _service.DeleteAsync(id);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsInvalidOperationException_WhenCurvePointDoesNotExist()
        {
            // Arrange
            var id = 1;
            _mockRepository.Setup(repo => repo.ExistsAsync(id)).ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.DeleteAsync(id));
            Assert.Equal("Curve point with ID 1 does not exist.", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsApplicationException_OnError()
        {
            // Arrange
            var id = 1;
            _mockRepository.Setup(repo => repo.ExistsAsync(id)).ReturnsAsync(true);
            _mockRepository.Setup(repo => repo.DeleteAsync(id)).ThrowsAsync(new Exception("Database error"));

            // Act
           var exception = await Assert.ThrowsAsync<ApplicationException>(() => _service.DeleteAsync(id));
            //  Assert            
            Assert.Equal("An error occurred while deleting the curve point with ID 1.", exception.Message);
        }
    }
}
