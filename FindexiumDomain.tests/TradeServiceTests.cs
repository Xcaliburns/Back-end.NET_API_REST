using System.Collections.Generic;
using System.Threading.Tasks;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Domain.Services;
using Moq;
using Xunit;


namespace FindexiumDomain.tests
{
    public class TradeServiceTests
    {
        private readonly Mock<ITradeRepository> _mockTradeRepository;
        private readonly ITradeService _tradeService;

        public TradeServiceTests()
        {
            _mockTradeRepository = new Mock<ITradeRepository>();
            _tradeService = new TradeService(_mockTradeRepository.Object);
        }

        #region GetAllTradesAsync Tests

        [Fact]
        public async Task GetAllTradesAsync_ReturnsListOfTrades()
        {
            // Arrange
            var trades = new List<Trade>
                    {
                        new Trade { TradeId = 1, Account = "Account1", TradeSecurity = "Security1" },
                        new Trade { TradeId = 2, Account = "Account2", TradeSecurity = "Security2" }
                    };
            _mockTradeRepository.Setup(repo => repo.GetAllTradesAsync()).ReturnsAsync(trades);

            // Act
            var result = (await _tradeService.GetAllTradesAsync()).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Collection(result,
                item => Assert.Equal(1, item.TradeId),
                item => Assert.Equal(2, item.TradeId));
        }

        [Fact]
        public async Task GetAllTradesAsync_ThrowsApplicationException()
        {
            // Arrange
            _mockTradeRepository.Setup(repo => repo.GetAllTradesAsync()).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _tradeService.GetAllTradesAsync());
            Assert.Equal("An error occurred while retrieving all trades.", exception.Message);
        }

        #endregion

        #region GetTradeByIdAsync Tests

        [Fact]
        public async Task GetTradeByIdAsync_ReturnsTrade()
        {
            // Arrange
            var trade = new Trade { TradeId = 1, Account = "Account1", TradeSecurity = "Security1" };
            _mockTradeRepository.Setup(repo => repo.GetTradeByIdAsync(1)).ReturnsAsync(trade);

            // Act
            var result = await _tradeService.GetTradeByIdAsync(1);

            // Assert
            Assert.Equal(1, result.TradeId);
            Assert.Equal("Account1", result.Account);
        }

        [Fact]
        public async Task GetTradeByIdAsync_ThrowsApplicationException()
        {
            // Arrange
            _mockTradeRepository.Setup(repo => repo.GetTradeByIdAsync(1)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _tradeService.GetTradeByIdAsync(1));
            Assert.Equal("An error occurred while retrieving the trade with ID 1.", exception.Message);
        }

        #endregion

        #region AddTradeAsync Tests

        [Fact]
        public async Task AddTradeAsync_AddsTrade()
        {
            // Arrange
            var trade = new Trade { TradeId = 1, Account = "Account1", TradeSecurity = "Security1" };
            _mockTradeRepository.Setup(repo => repo.AddTradeAsync(trade)).Returns(Task.CompletedTask);

            // Act
            await _tradeService.AddTradeAsync(trade);

            // Assert
            _mockTradeRepository.Verify(repo => repo.AddTradeAsync(trade), Times.Once);
        }

        [Fact]
        public async Task AddTradeAsync_ThrowsApplicationException()
        {
            // Arrange
            var trade = new Trade { TradeId = 1, Account = "Account1", TradeSecurity = "Security1" };
            _mockTradeRepository.Setup(repo => repo.AddTradeAsync(trade)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _tradeService.AddTradeAsync(trade));
            Assert.Equal("An error occurred while adding a new trade.", exception.Message);
        }

        [Fact]
        public async Task AddTradeAsync_ThrowsArgumentNullException()
        {
            // Act
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _tradeService.AddTradeAsync(null));

            // Assert
            Assert.NotNull(exception);
        }

        #endregion

        #region UpdateTradeAsync Tests

        [Fact]
        public async Task UpdateTradeAsync_UpdatesTrade()
        {
            // Arrange
            var trade = new Trade { TradeId = 1, Account = "Account1", TradeSecurity = "Security1" };
            _mockTradeRepository.Setup(repo => repo.TradeExistsAsync(1)).ReturnsAsync(true);
            _mockTradeRepository.Setup(repo => repo.UpdateTradeAsync(trade)).Returns(Task.CompletedTask);

            // Act
            await _tradeService.UpdateTradeAsync(trade);

            // Assert
            _mockTradeRepository.Verify(repo => repo.UpdateTradeAsync(trade), Times.Once);
        }

        [Fact]
        public async Task UpdateTradeAsync_ThrowsApplicationException()
        {
            // Arrange
            var trade = new Trade { TradeId = 1, Account = "Account1", TradeSecurity = "Security1" };
            _mockTradeRepository.Setup(repo => repo.TradeExistsAsync(1)).ReturnsAsync(true);
            _mockTradeRepository.Setup(repo => repo.UpdateTradeAsync(trade)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _tradeService.UpdateTradeAsync(trade));
            Assert.Equal("An error occurred while updating the trade with ID 1.", exception.Message);
        }

        [Fact]
        public async Task UpdateTradeAsync_ThrowsArgumentNullException()
        {
            // Act
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _tradeService.UpdateTradeAsync(null));

            // Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task UpdateTradeAsync_ThrowsInvalidOperationException()
        {
            // Arrange
            var trade = new Trade { TradeId = 1, Account = "Account1", TradeSecurity = "Security1" };
            _mockTradeRepository.Setup(repo => repo.TradeExistsAsync(1)).ReturnsAsync(false);

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _tradeService.UpdateTradeAsync(trade));

            // Assert
            Assert.Equal("Trade with ID 1 does not exist.", exception.Message);
        }

        #endregion

        #region DeleteTradeAsync Tests

        [Fact]
        public async Task DeleteTradeAsync_DeletesTrade()
        {
            // Arrange
            _mockTradeRepository.Setup(repo => repo.TradeExistsAsync(1)).ReturnsAsync(true);
            _mockTradeRepository.Setup(repo => repo.DeleteTradeAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _tradeService.DeleteTradeAsync(1);

            // Assert
            _mockTradeRepository.Verify(repo => repo.DeleteTradeAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteTradeAsync_ThrowsApplicationException()
        {
            // Arrange
            _mockTradeRepository.Setup(repo => repo.TradeExistsAsync(1)).ReturnsAsync(true);
            _mockTradeRepository.Setup(repo => repo.DeleteTradeAsync(1)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _tradeService.DeleteTradeAsync(1));
            Assert.Equal("An error occurred while deleting the trade with ID 1.", exception.Message);
        }

        [Fact]
        public async Task DeleteTradeAsync_ThrowsInvalidOperationException()
        {
            // Arrange
            _mockTradeRepository.Setup(repo => repo.TradeExistsAsync(1)).ReturnsAsync(false);

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _tradeService.DeleteTradeAsync(1));

            // Assert
            Assert.Equal("Trade with ID 1 does not exist.", exception.Message);
        }

        #endregion

        #region TradeExistsAsync Tests

        [Fact]
        public async Task TradeExistsAsync_ReturnsTrue_WhenTradeExists()
        {
            // Arrange
            _mockTradeRepository.Setup(repo => repo.TradeExistsAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _tradeService.TradeExistsAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task TradeExistsAsync_ReturnsFalse_WhenTradeDoesNotExist()
        {
            // Arrange
            _mockTradeRepository.Setup(repo => repo.TradeExistsAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _tradeService.TradeExistsAsync(1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task TradeExistsAsync_ThrowsApplicationException()
        {
            // Arrange
            _mockTradeRepository.Setup(repo => repo.TradeExistsAsync(1)).ThrowsAsync(new Exception("Database error"));

            // Act
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _tradeService.TradeExistsAsync(1));

            // Assert
            Assert.Equal("An error occurred while checking if the trade with ID 1 exists.", exception.Message);
        }



        #endregion
    }
}
