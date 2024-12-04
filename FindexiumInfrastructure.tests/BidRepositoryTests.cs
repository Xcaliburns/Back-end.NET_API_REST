using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Findexium.Infrastructure.Models;
using Findexium.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;// ici il a fallu ajouter Linq
using System.Threading.Tasks;
using Xunit;


namespace FindexiumInfrastructure.tests
{
    public class BidRepositoryTests
    {
        private readonly DbContextOptions<LocalDbContext> _dbContextOptions;
        private readonly Mock<ILogger<BidRepository>> _mockLogger;

        public BidRepositoryTests()
        {
            // Initialize DbContextOptions
            _dbContextOptions = new DbContextOptionsBuilder<LocalDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Initialize logger mock
            _mockLogger = new Mock<ILogger<BidRepository>>();
        }

        private BidRepository CreateRepository(LocalDbContext context)
        {
            return new BidRepository(context, _mockLogger.Object);
        }

        private LocalDbContext CreateContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;
            return new LocalDbContext(options);
        }

        private async Task CleanDatabase(LocalDbContext context)
        {
            context.Bids.RemoveRange(context.Bids);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllBids()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_GetAllAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var bids = new List<BidDto>
                        {
                            new BidDto("Account1", "Type1", 100, 200, 300, 400, "Benchmark1", DateTime.Now, "Commentary1", "Security1", "Status1", "Trader1", "Book1", "Creator1", DateTime.Now, "Revisor1", DateTime.Now, "Deal1", "DealType1", "Source1", "Side1"),
                            new BidDto("Account2", "Type2", 150, 250, 350, 450, "Benchmark2", DateTime.Now, "Commentary2", "Security2", "Status2", "Trader2", "Book2", "Creator2", DateTime.Now, "Revisor2", DateTime.Now, "Deal2", "DealType2", "Source2", "Side2")
                        };

                context.Bids.AddRange(bids);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                Assert.Equal(2, result.Count());
            }
        }
        [Fact]
        public async Task GetAllAsync_ThrowsApplicationException_WhenExceptionOccurs()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_GetAllAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<BidRepository>>();
                var repository = new BidRepository(context, mockLogger.Object);

                // Create a mock DbSet and configure it to throw an exception when ToListAsync is called
                var mockSet = new Mock<DbSet<BidDto>>();
                mockSet.As<IQueryable<BidDto>>().Setup(m => m.Provider).Throws(new Exception("Simulated exception"));
                context.Bids = mockSet.Object;

                // Act & Assert
                var exception = await Assert.ThrowsAsync<ApplicationException>(() => repository.GetAllAsync());

                Assert.Equal("An error occurred while retrieving all bids.", exception.Message);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsBid_WhenBidExists()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_GetByIdAsync_Exists"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var bid = new BidDto("Account1", "Type1", 100, 200, 300, 400, "Benchmark1", DateTime.Now, "Commentary1", "Security1", "Status1", "Trader1", "Book1", "Creator1", DateTime.Now, "Revisor1", DateTime.Now, "Deal1", "DealType1", "Source1", "Side1");
                context.Bids.Add(bid);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetByIdAsync(bid.BidListId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Account1", result.Account);
            }
        }

       
        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenBidDoesNotExist()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_GetByIdAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                // Act
                var result = await repository.GetByIdAsync(1);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetByIdAsync_LogsErrorAndThrowsApplicationException_WhenExceptionOccurs()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_GetByIdAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<BidRepository>>();
                var repository = new BidRepository(context, mockLogger.Object);

                // Simulate an exception when FindAsync is called
                context.Bids = new Mock<DbSet<BidDto>>().Object;
                var mockSet = Mock.Get(context.Bids);
                mockSet.Setup(m => m.FindAsync(It.IsAny<int>())).ThrowsAsync(new Exception("Simulated exception"));

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.GetByIdAsync(1));

                Assert.Equal("An error occurred while retrieving the bid with ID 1.", exception.Message);
               
            }
        }

        [Fact]
        public async Task AddAsync_AddsBid()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_AddAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var bidList = new BidList { Account = "Account1", BidType = "bidType" };

                // Act
                await repository.AddAsync(bidList);

                // Assert
                var addedBid = await context.Bids.FirstOrDefaultAsync(b => b.Account == "Account1");
                Assert.NotNull(addedBid);
            }
        }

        [Fact]
        public async Task AddAsync_ThrowsApplicationException_WhenExceptionOccurs()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_AddAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<BidRepository>>();
                var repository = new BidRepository(context, mockLogger.Object);

                var bidList = new BidList { Account = "Account1", BidType = "bidType" };

                // Simulate an exception when SaveChangesAsync is called
                context.Bids = new Mock<DbSet<BidDto>>().Object;
                var mockSet = Mock.Get(context.Bids);
                mockSet.Setup(m => m.Add(It.IsAny<BidDto>())).Throws(new Exception("Simulated exception"));

                // Act & Assert
                var exception = await Assert.ThrowsAsync<ApplicationException>(() => repository.AddAsync(bidList));

                Assert.Equal("An error occurred while adding the bid.", exception.Message);
            }
        }




        [Fact]
        public async Task UpdateAsync_UpdatesBid()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_UpdateAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var bid = new BidDto("Account1", "Type1", 100, 200, 300, 400, "Benchmark1", DateTime.Now, "Commentary1", "Security1", "Status1", "Trader1", "Book1", "Creator1", DateTime.Now, "Revisor1", DateTime.Now, "Deal1", "DealType1", "Source1", "Side1");
                context.Bids.Add(bid);
                await context.SaveChangesAsync();

                var bidList = new BidList { Account = "UpdatedAccount" };

                // Act
                await repository.UpdateAsync(bid.BidListId, bidList);

                // Assert
                var updatedBid = await context.Bids.FindAsync(bid.BidListId);
                Assert.NotNull(updatedBid);
                Assert.Equal("UpdatedAccount", updatedBid.Account);
            }
        }

        [Fact]
        public async Task UpdateAsync_ThrowsArgumentNullException_WhenBidListIsNull()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_UpdateAsync_NullBidList"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                // Act & Assert
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => repository.UpdateAsync(1, null));
                Assert.Equal("bidList", exception.ParamName);
            }
        }

        [Fact]
        public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenBidDoesNotExist()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_UpdateAsync_BidNotFound"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var bidList = new BidList { Account = "UpdatedAccount" };

                // Act & Assert
                var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.UpdateAsync(1, bidList));
                Assert.Equal("Bid not found", exception.Message);
            }
        }

       

      


        [Fact]
        public async Task DeleteAsync_DeletesBid()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_DeleteAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var bid = new BidDto("Account1", "Type1", 100, 200, 300, 400, "Benchmark1", DateTime.Now, "Commentary1", "Security1", "Status1", "Trader1", "Book1", "Creator1", DateTime.Now, "Revisor1", DateTime.Now, "Deal1", "DealType1", "Source1", "Side1");
                context.Bids.Add(bid);
                await context.SaveChangesAsync();

                // Act
                await repository.DeleteAsync(bid.BidListId);

                // Assert
                var deletedBid = await context.Bids.FindAsync(bid.BidListId);
                Assert.Null(deletedBid);
            }
        }

        [Fact]
        public async Task ExistsAsync_ReturnsTrue_WhenBidExists()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_ExistsAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var bid = new BidDto("Account1", "Type1", 100, 200, 300, 400, "Benchmark1", DateTime.Now, "Commentary1", "Security1", "Status1", "Trader1", "Book1", "Creator1", DateTime.Now, "Revisor1", DateTime.Now, "Deal1", "DealType1", "Source1", "Side1");
                context.Bids.Add(bid);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.ExistsAsync(bid.BidListId);

                // Assert
                Assert.True(result);
            }
        }

        [Fact]
        public async Task ExistsAsync_ReturnsFalse_WhenBidDoesNotExist()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_ExistsAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                // Act
                var result = await repository.ExistsAsync(1);

                // Assert
                Assert.False(result);
            }
        }

        [Fact]
        public async Task ExistsAsync_ThrowsApplicationException_WhenExceptionOccurs()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_ExistsAsync_Exception"))
            {
                var repository = CreateRepository(context);

                // Simulate an exception when AnyAsync is called
                var mockSet = new Mock<DbSet<BidDto>>();
                mockSet.As<IQueryable<BidDto>>().Setup(m => m.Provider).Throws(new Exception("Simulated exception"));
                context.Bids = mockSet.Object;

                // Act & Assert
                var exception = await Assert.ThrowsAsync<ApplicationException>(() => repository.ExistsAsync(1));
                Assert.Equal("An error occurred while checking the existence of the bid with ID 1.", exception.Message);

             
            }
        }


    }
}
