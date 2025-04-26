using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Domain.Services;
using Moq;

namespace FindexiumDomain.tests
{
    public class BidListServicesTests
    {
        private readonly Mock<IBidListRepository> _mockBidListRepository;
        private readonly IBidListServices _bidListServices;

        public BidListServicesTests()
        {
            _mockBidListRepository = new Mock<IBidListRepository>();
            _bidListServices = new BidListService(_mockBidListRepository.Object);
        }

        [Fact]
        public async Task GetAllBidListsAsync_ReturnsListOfThreeBidLists()
        {
            // Arrange
            var bidLists = new List<BidList>
                        {
                            new BidList { BidListId = 1, Account = "Account1", BidType = "Type1", Bid = 100 },
                            new BidList { BidListId = 2, Account = "Account2", BidType = "Type2", Bid = 200 },
                            new BidList { BidListId = 3, Account = "Account3", BidType = "Type3", Bid = 300 }
                        };
            _mockBidListRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(bidLists);

            // Act
            var result = (await _bidListServices.GetAllAsync()).ToList();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Collection(result,
                item => Assert.Equal(1, item.BidListId),
                item => Assert.Equal(2, item.BidListId),
                item => Assert.Equal(3, item.BidListId));
        }

        [Fact]
        public async Task GetAllAsync_ThrowsApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockBidListRepository.Setup(repo => repo.GetAllAsync()).ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _bidListServices.GetAllAsync());
            Assert.Equal("An error occurred while retrieving all bid lists.", exception.Message);
            Assert.Equal("Test exception", exception.InnerException.Message);
        }

        [Fact]
        public async Task GetBidListByIdAsync_ReturnsBidList()
        {
            // Arrange
            var bidList = new BidList { BidListId = 1, Account = "Account1", BidType = "Type1", Bid = 100 };
            _mockBidListRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(bidList);

            // Act
            var result = await _bidListServices.GetByIdAsync(1);

            // Assert
            Assert.Equal(1, result.BidListId);
            Assert.Equal("Account1", result.Account);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            var id = 1;
            _mockBidListRepository.Setup(repo => repo.GetByIdAsync(id)).ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _bidListServices.GetByIdAsync(id));
            Assert.Equal($"An error occurred while retrieving the bid list with ID {id}.", exception.Message);
            Assert.Equal("Test exception", exception.InnerException.Message);
        }

        [Fact]
        public async Task AddBidListAsync_AddsBidList()
        {
            // Arrange
            var bidList = new BidList { BidListId = 1, Account = "Account1", BidType = "Type1", Bid = 100 };
            _mockBidListRepository.Setup(repo => repo.AddAsync(bidList)).Returns(Task.CompletedTask);

            // Act
            await _bidListServices.AddAsync(bidList);

            // Assert
            _mockBidListRepository.Verify(repo => repo.AddAsync(bidList), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentNullException_WhenBidListIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _bidListServices.AddAsync(null));
        }

        [Fact]
        public async Task AddAsync_ThrowsApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            var bidList = new BidList { BidListId = 1, Account = "TestAccount" };
            _mockBidListRepository.Setup(repo => repo.AddAsync(bidList)).ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _bidListServices.AddAsync(bidList));
            Assert.Equal("An error occurred while adding a new bid list.", exception.Message);
            Assert.Equal("Test exception", exception.InnerException.Message);
        }


        [Fact]
        public async Task UpdateBidListAsync_UpdatesBidList()
        {
            // Arrange
            var Id = 1;
            var bidList = new BidList {  Account = "Account1", BidType = "Type1", Bid = 100 };
            _mockBidListRepository.Setup(repo => repo.ExistsAsync(1)).ReturnsAsync(true);
            _mockBidListRepository.Setup(repo => repo.UpdateAsync(Id,bidList)).Returns(Task.CompletedTask);

            // Act
            await _bidListServices.UpdateAsync(Id, bidList);

            // Assert
            _mockBidListRepository.Verify(repo => repo.UpdateAsync(Id, bidList), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsArgumentNullException_WhenBidListIsNull()
        {
            // Arrange
            int id = 1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _bidListServices.UpdateAsync(id, null));
        }

        [Fact]
        public async Task UpdateAsync_ThrowsInvalidOperationException_WhenBidListDoesNotExist()
        {
            // Arrange
            int id = 1;
            var bidList = new BidList { BidListId = id, Account = "TestAccount" };
            _mockBidListRepository.Setup(repo => repo.ExistsAsync(id)).ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _bidListServices.UpdateAsync(id, bidList));
            Assert.Equal($"Bid list with ID {id} does not exist.", exception.Message);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            int id = 1;
            var bidList = new BidList { BidListId = id, Account = "TestAccount" };
            _mockBidListRepository.Setup(repo => repo.ExistsAsync(id)).ReturnsAsync(true);
            _mockBidListRepository.Setup(repo => repo.UpdateAsync(id, bidList)).ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _bidListServices.UpdateAsync(id, bidList));
            Assert.Equal($"An error occurred while updating the bid list with ID {bidList.BidListId}.", exception.Message);
            Assert.Equal("Test exception", exception.InnerException.Message);
        }

        [Fact]
        public async Task DeleteBidListAsync_DeletesBidList()
        {
            // Arrange
            _mockBidListRepository.Setup(repo => repo.ExistsAsync(1)).ReturnsAsync(true);
            _mockBidListRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _bidListServices.DeleteAsync(1);

            // Assert
            _mockBidListRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsInvalidOperationException_WhenBidListDoesNotExist()
        {
            // Arrange
            int id = 1;
            _mockBidListRepository.Setup(repo => repo.ExistsAsync(id)).ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _bidListServices.DeleteAsync(id));
            Assert.Equal($"Bid list with ID {id} does not exist.", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            int id = 1;
            _mockBidListRepository.Setup(repo => repo.ExistsAsync(id)).ReturnsAsync(true);
            _mockBidListRepository.Setup(repo => repo.DeleteAsync(id)).ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _bidListServices.DeleteAsync(id));
            Assert.Equal($"An error occurred while deleting the bid list with ID {id}.", exception.Message);
            Assert.Equal("Test exception", exception.InnerException.Message);
        }



        [Fact]
        public async Task ExistsAsync_ReturnsTrue_WhenBidListExists()
        {
            // Arrange
            int id = 1;
            _mockBidListRepository.Setup(repo => repo.ExistsAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _bidListServices.ExistsAsync(id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsAsync_ReturnsFalse_WhenBidListDoesNotExist()
        {
            // Arrange
            int id = 1;
            _mockBidListRepository.Setup(repo => repo.ExistsAsync(id)).ReturnsAsync(false);

            // Act
            var result = await _bidListServices.ExistsAsync(id);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExistsAsync_ThrowsApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            int id = 1;
            _mockBidListRepository.Setup(repo => repo.ExistsAsync(id)).ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _bidListServices.ExistsAsync(id));
            Assert.Equal($"An error occurred while checking the existence of the bid list with ID {id}.", exception.Message);
            Assert.Equal("Test exception", exception.InnerException.Message);
        }
    }
}
