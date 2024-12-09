using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Findexium.Infrastructure.Models;
using Findexium.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Moq;

namespace FindexiumInfrastructure.tests
{
    public class TradeRepositoryTests
    {
        private readonly DbContextOptions<LocalDbContext> _dbContextoptions;
        private readonly Mock<ILogger<TradeRepository>> _mockLogger;

        public TradeRepositoryTests()
        {
            _dbContextoptions = new DbContextOptionsBuilder<LocalDbContext>()
                .UseInMemoryDatabase(databaseName: "TradeRepository")
                .Options;
            _mockLogger = new Mock<ILogger<TradeRepository>>();
        }

        private TradeRepository CreateRepository(LocalDbContext context)
        {
            return new TradeRepository(context, _mockLogger.Object);
        }

        private LocalDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new LocalDbContext(options);
        }

        private async Task CleanDatabase(LocalDbContext context)
        {
            context.Trades.RemoveRange(context.Trades);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsTrades()
        {
            //Arrange
            using (var context = CreateContext("TestDatabase_GetAllAsync"))
            {
                await CleanDatabase(context);
                var tradeRepository = CreateRepository(context);

                var trades = new List<TradeDto>
                {
                    new TradeDto("Account1", "Type1", 100, 200, 50.5, 51.5, DateTime.Now, "Security1", "Status1", "Trader1", "Benchmark1", "Book1", "CreationName1", DateTime.Now, "RevisionName1", DateTime.Now, "DealName1"),
                    new TradeDto("Account2", "Type2", 150, 250, 55.5, 56.5, DateTime.Now, "Security2", "Status2", "Trader2", "Benchmark2", "Book2", "CreationName2", DateTime.Now, "RevisionName2", DateTime.Now, "DealName2"),
                    new TradeDto("Account3", "Type3", 200, 300, 60.5, 61.5, DateTime.Now, "Security3", "Status3", "Trader3", "Benchmark3", "Book3", "CreationName3", DateTime.Now, "RevisionName3", DateTime.Now, "DealName3")
                };

                context.Trades.AddRange(trades);
                await context.SaveChangesAsync();

                //Act
                var result = await tradeRepository.GetAllTradesAsync();

                //Assert
                Assert.Equal(3, result.Count());
                Assert.Contains(result, t => t.Account == "Account1");
                Assert.Contains(result, t => t.Account == "Account2");
                Assert.Contains(result, t => t.Account == "Account3");
            }
        }

        [Fact]
        public async Task GetAllAsync_WhenExceptionIsThrown()
        {
            //Arrange
            using (var context = CreateContext("TestDatabase_GetAllAsync_WhenExceptionIsThrow"))
            {
                var mockLogger = new Mock<ILogger<TradeRepository>>();
                var repository = new TradeRepository(context, mockLogger.Object);

                var mockSet = new Mock<DbSet<TradeDto>>();
                mockSet.As<IQueryable<TradeDto>>().Setup(m => m.Provider)
                    .Throws(new Exception("An error occurred while retrieving all trades."));
                context.Trades = mockSet.Object;

                //Act
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.GetAllTradesAsync());

                //Assert
                Assert.Equal("An error occurred while retrieving all trades.", exception.Message);

            }

        }
        [Fact]
        public async Task GetTradeByIdAsync_ReturnsTrade()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_GetTradeByIdAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var trade = new TradeDto("Account1", "Type1", 100, 200, 50.5, 51.5, DateTime.Now, "Security1", "Status1", "Trader1", "Benchmark1", "Book1", "CreationName1", DateTime.Now, "RevisionName1", DateTime.Now, "DealName1");
                context.Trades.Add(trade);
                await context.SaveChangesAsync();

                //Act
                var result = await repository.GetTradeByIdAsync(1);



                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.TradeId);
                Assert.Equal("Account1", result.Account);
            }
        }
        [Fact]
        public async Task GetTradeByIdAsync_WhenExceptionIsThrown()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_GetTradeByIdAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<TradeRepository>>();
                var repository = new TradeRepository(context, mockLogger.Object);

                var mockSet = new Mock<DbSet<TradeDto>>();
                mockSet.Setup(m => m.FindAsync(It.IsAny<int>()))
                    .Throws(new Exception("An error occurred while retrieving the trade."));
                context.Trades = mockSet.Object;

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.GetTradeByIdAsync(1));
                Assert.Equal("An error occurred while retrieving the trade.", exception.Message);
            }
        }
        [Fact]
        public async Task AddTradeAsync_AddsTradeSuccessfully()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_AddTradeAsync"))
            {
                await CleanDatabase(context);
                var mockLogger = new Mock<ILogger<TradeRepository>>();
                var repository = new TradeRepository(context, mockLogger.Object);

                var trade = new Trade
                {
                    Account = "Account1",
                    AccountType = "Type1",
                    BuyQuantity = 100,
                    SellQuantity = 50,
                    BuyPrice = 10.5,
                    SellPrice = 11.0,
                    TradeDate = DateTime.Now,
                    TradeSecurity = "Security1",
                    TradeStatus = "Status1",
                    Trader = "Trader1",
                    Benchmark = "Benchmark1",
                    Book = "Book1",
                    CreationName = "Creator1",
                    CreationDate = DateTime.Now,
                    RevisionName = "Revisor1",
                    RevisionDate = DateTime.Now,
                    DealName = "Deal1"
                };

                // Act
                await repository.AddTradeAsync(trade);

                // Assert
                var addedTrade = await context.Trades.FirstOrDefaultAsync(t => t.Account == "Account1");
                Assert.NotNull(addedTrade);
                Assert.Equal("Account1", addedTrade.Account);
            }
        }
        [Fact]
        public async Task AddTradeAsync_WhenExceptionIsThrown()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_AddTradeAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<TradeRepository>>();
                var repository = new TradeRepository(context, mockLogger.Object);

                var mockSet = new Mock<DbSet<TradeDto>>();
                mockSet.Setup(m => m.Add(It.IsAny<TradeDto>()))
                    .Throws(new Exception("An error occurred while adding the trade."));
                context.Trades = mockSet.Object;

                var trade = new Trade
                {
                    Account = "Account1",
                    AccountType = "Type1",
                    BuyQuantity = 100,
                    SellQuantity = 50,
                    BuyPrice = 10.5,
                    SellPrice = 11.0,
                    TradeDate = DateTime.Now,
                    TradeSecurity = "Security1",
                    TradeStatus = "Status1",
                    Trader = "Trader1",
                    Benchmark = "Benchmark1",
                    Book = "Book1",
                    CreationName = "Creator1",
                    CreationDate = DateTime.Now,
                    RevisionName = "Revisor1",
                    RevisionDate = DateTime.Now,
                    DealName = "Deal1"
                };



                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.AddTradeAsync(trade));
                Assert.Equal("An error occurred while adding the trade.", exception.Message);

            }
        }
        [Fact]
        public async Task UpdateTradeAsync_UpdatesTrade()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_UpdateTradeAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var initialTrade = new TradeDto(

                    "Account1",
                    "Type1",
                    100,
                    50,
                    10.5,
                    11.0,
                    DateTime.Now,
                    "Security1",
                    "Status1",
                    "Trader1",
                    "Benchmark1",
                    "Book1",
                    "Creator1",
                    DateTime.Now,
                    "Revisor1",
                    DateTime.Now,
                    "Deal1"
                );

                context.Trades.Add(initialTrade);
                await context.SaveChangesAsync();

                var updatedTrade = new Trade
                {
                    TradeId = 1,
                    Account = "UpdatedAccount",
                    AccountType = "UpdatedType",
                    BuyQuantity = 200,
                    SellQuantity = 100,
                    BuyPrice = 20.5,
                    SellPrice = 21.0,
                    TradeDate = DateTime.Now.AddDays(1),
                    TradeSecurity = "UpdatedSecurity",
                    TradeStatus = "UpdatedStatus",
                    Trader = "UpdatedTrader",
                    Benchmark = "UpdatedBenchmark",
                    Book = "UpdatedBook",
                    CreationName = "UpdatedCreator",
                    CreationDate = DateTime.Now.AddDays(1),
                    RevisionName = "UpdatedRevisor",
                    RevisionDate = DateTime.Now.AddDays(1),
                    DealName = "UpdatedDeal"
                };

                // Act
                await repository.UpdateTradeAsync(updatedTrade);

                // Assert
                var result = await context.Trades.FindAsync(1);
                Assert.NotNull(result);
                Assert.Equal("UpdatedAccount", result.Account);
                Assert.Equal("UpdatedType", result.AccountType);
                Assert.Equal(200, result.BuyQuantity);
                Assert.Equal(100, result.SellQuantity);
                Assert.Equal(20.5, result.BuyPrice);
                Assert.Equal(21.0, result.SellPrice);
                Assert.Equal(updatedTrade.TradeDate, result.TradeDate);
                Assert.Equal("UpdatedSecurity", result.TradeSecurity);
                Assert.Equal("UpdatedStatus", result.TradeStatus);
                Assert.Equal("UpdatedTrader", result.Trader);
                Assert.Equal("UpdatedBenchmark", result.Benchmark);
                Assert.Equal("UpdatedBook", result.Book);
                Assert.Equal("UpdatedCreator", result.CreationName);
                Assert.Equal(updatedTrade.CreationDate, result.CreationDate);
                Assert.Equal("UpdatedRevisor", result.RevisionName);
                Assert.Equal(updatedTrade.RevisionDate, result.RevisionDate);
                Assert.Equal("UpdatedDeal", result.DealName);
            }
        }

        [Fact]
        public async Task UpdateAsync_WhenExceptionIsThrown()
        {
            //Arrange
            using (var context = CreateContext("TestDatabase_UpdateTradeAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<TradeRepository>>();
                var repository = new TradeRepository(context, mockLogger.Object);

                var mockSet = new Mock<DbSet<TradeDto>>();
                mockSet.Setup(m => m.FindAsync(It.IsAny<int>()))
                    .Throws(new Exception("An error occurred while updating the trade."));
                context.Trades = mockSet.Object;

                var trade = new Trade
                {
                    TradeId = 1,
                    Account = "Account1",
                    AccountType = "Type1",
                    BuyQuantity = 100,
                    SellQuantity = 50,
                    BuyPrice = 10.5,
                    SellPrice = 11.0,
                    TradeDate = DateTime.Now,
                    TradeSecurity = "Security1",
                    TradeStatus = "Status1",
                    Trader = "Trader1",
                    Benchmark = "Benchmark1",
                    Book = "Book1",
                    CreationName = "Creator1",
                    CreationDate = DateTime.Now,
                    RevisionName = "Revisor1",
                    RevisionDate = DateTime.Now,
                    DealName = "Deal1"
                };

                //Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.UpdateTradeAsync(trade));
                Assert.Equal("An error occurred while updating the trade.", exception.Message);
            }


        }

        [Fact]
        public async Task DeleteTradeAsync_DeletesTrade()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_DeleteTradeAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var trade = new TradeDto(
                    "Account1",
                    "Type1",
                    100,
                    50,
                    10.5,
                    11.0,
                    DateTime.Now,
                    "Security1",
                    "Status1",
                    "Trader1",
                    "Benchmark1",
                    "Book1",
                    "Creator1",
                    DateTime.Now,
                    "Revisor1",
                    DateTime.Now,
                    "Deal1"
                );

                context.Trades.Add(trade);
                await context.SaveChangesAsync();

                // Act
                await repository.DeleteTradeAsync(1);

                // Assert
                var result = await context.Trades.FindAsync(1);
                Assert.Null(result);
            }
        }
        [Fact]
        public async Task DeleteTradeAsync_WhenExceptionIsThrown()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_DeleteTradeAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<TradeRepository>>();
                var repository = new TradeRepository(context, mockLogger.Object);

                var mockSet = new Mock<DbSet<TradeDto>>();
                mockSet.Setup(m => m.FindAsync(It.IsAny<int>()))
                    .Throws(new Exception("An error occurred while deleting the trade."));
                context.Trades = mockSet.Object;

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.DeleteTradeAsync(1));
                Assert.Equal("An error occurred while deleting the trade.", exception.Message);
            }
        }

        //[Fact]
        //public async Task DeleteTradeAsync_WhenTradeDoesNotExist()
        //{
        //    // Arrange
        //    using (var context = CreateContext("TestDatabase_DeleteTradeAsync_TradeDoesNotExist"))
        //    {
        //        await CleanDatabase(context);
        //        var repository = CreateRepository(context);

        //        // Act & Assert
        //        var exception = await Assert.ThrowsAsync<Exception>(() => repository.DeleteTradeAsync(1));
        //        Assert.Equal("An error occurred while deleting the trade.", exception.Message);
        //    }
        //}

        [Fact]
        public async Task TradeExistsAsync_ReturnsTrue()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_TradeExistsAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var trade = new TradeDto(
                    "Account1",
                    "Type1",
                    100,
                    50,
                    10.5,
                    11.0,
                    DateTime.Now,
                    "Security1",
                    "Status1",
                    "Trader1",
                    "Benchmark1",
                    "Book1",
                    "Creator1",
                    DateTime.Now,
                    "Revisor1",
                    DateTime.Now,
                    "Deal1"
                );

                context.Trades.Add(trade);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.TradeExistsAsync(1);

                // Assert
                Assert.True(result);
            }
        }

        [Fact]
        public async Task ExistsAsync_WhenExceptionIsThrown()
        {
            using (var context = CreateContext("TestDatabase_TradeExistsAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<TradeRepository>>();
                var repository = new TradeRepository(context, mockLogger.Object);

                var mockSet = new Mock<DbSet<TradeDto>>();
                mockSet.As<IQueryable<TradeDto>>().Setup(m => m.Provider)
                    .Throws(new Exception("An error occurred while checking if trade exists"));
                context.Trades = mockSet.Object;

                //Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.TradeExistsAsync(1));
                Assert.Equal("An error occurred while checking if trade exists", exception.Message);
            }
        }

        }
}
