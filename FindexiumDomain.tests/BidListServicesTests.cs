using System.Collections.Generic;
using System.Threading.Tasks;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Domain.Services;
using Moq;
using Xunit;

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
    }
}
