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
using Microsoft.Identity.Client;
using Moq;
using Xunit;


namespace FindexiumApi.tests
{
    public class TradeControllerTests
    {
        private readonly Mock<ITradeService> _tradeService;
        private readonly Mock<ILogger<TradesController>> _logger;
        private readonly TradesController _controller;


        public TradeControllerTests()
        {
            _tradeService = new Mock<ITradeService>();
            _logger = new Mock<ILogger<TradesController>>();
            _controller = new TradesController(_tradeService.Object, _logger.Object);
        }

        [Fact]
        public async Task GetTrades_ReturnsOkResult()
        {
            // Arrange
            var trades = new List<Trade>
            {
                new Trade
                {
                    TradeId = 1,
                    Account = "Account1",
                    AccountType = "AccountType1",
                    BuyQuantity = 1,
                    SellQuantity = 1,
                    BuyPrice = 1,
                    SellPrice = 1,
                    Benchmark = "Benchmark1",
                    TradeDate = DateTime.Now,
                    TradeSecurity = "TradeSecurity1",
                    TradeStatus = "TradeStatus1",
                    Trader = "Trader1",
                    Book = "Book1",
                    CreationName = "CreationName1",
                    CreationDate = DateTime.Now,
                    RevisionName = "RevisionName1",
                    RevisionDate = DateTime.Now,
                    BuyCurrency = "USD",
                    SellCurrency = "EUR"
                },
                new Trade
                {
                    TradeId = 2,
                    Account = "Account2",
                    AccountType = "AccountType2",
                    BuyQuantity = 2,
                    SellQuantity = 2,
                    BuyPrice = 2,
                    SellPrice = 2,
                    Benchmark = "Benchmark2",
                    TradeDate = DateTime.Now,
                    TradeSecurity = "TradeSecurity2",
                    TradeStatus = "TradeStatus2",
                    Trader = "Trader2",
                    Book = "Book2",
                    CreationName = "CreationName2",
                    CreationDate = DateTime.Now,
                    RevisionName = "RevisionName2",
                    RevisionDate = DateTime.Now,
                    BuyCurrency = "GBP",
                    SellCurrency = "USD"
                },
                new Trade
                {
                    TradeId = 3,
                    Account = "Account3",
                    AccountType = "AccountType3",
                    BuyQuantity = 3,
                    SellQuantity = 3,
                    BuyPrice = 3,
                    SellPrice = 3,
                    Benchmark = "Benchmark3",
                    TradeDate = DateTime.Now,
                    TradeSecurity = "TradeSecurity3",
                    TradeStatus = "TradeStatus3",
                    Trader = "Trader3",
                    Book = "Book3",
                    CreationName = "CreationName3",
                    CreationDate = DateTime.Now,
                    RevisionName = "RevisionName3",
                    RevisionDate = DateTime.Now,
                    BuyCurrency = "EUR",
                    SellCurrency = "JPY"
                }
            };

            _tradeService.Setup(x => x.GetAllTradesAsync()).ReturnsAsync(trades);

            // Act
            var result = await _controller.GetTrades();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnTrade = Assert.IsType<List<TradeResponse>>(okResult.Value);
            Assert.Equal(3, returnTrade.Count());
            Assert.Collection(returnTrade,
                item => { Assert.Equal(1, item.TradeId); },
                item => { Assert.Equal(2, item.TradeId); },
                item => { Assert.Equal(3, item.TradeId); }

            );
        }

        [Fact]
        public async Task GetTrades_ReturnsInternalServerError()
        {
            // Arrange
            _tradeService.Setup(service => service.GetAllTradesAsync())
                .ThrowsAsync(new Exception("Test exception"));


            // Act
            var result = await _controller.GetTrades();

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }

        [Fact]
        public async Task GetCurvePointById()
        {
            //Arrange

            var trade = new Trade
            {
                TradeId = 1,
                Account = "Account1",
                AccountType = "AccountType1",
                BuyQuantity = 1,
                SellQuantity = 1,
                BuyPrice = 1,
                SellPrice = 1,
                Benchmark = "Benchmark1",
                TradeDate = DateTime.Now,
                TradeSecurity = "TradeSecurity1",
                TradeStatus = "TradeStatus1",
                Trader = "Trader1",
                Book = "Book1",
                CreationName = "CreationName1",
                CreationDate = DateTime.Now,
                RevisionName = "RevisionName1",
                RevisionDate = DateTime.Now,
                BuyCurrency = "USD",
                SellCurrency = "EUR"
            };

            _tradeService.Setup(service => service.GetTradeByIdAsync(1)).ReturnsAsync(trade);

            // Act
            var result = await _controller.GetTrade(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Trade>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnTrade = Assert.IsType<Trade>(okResult.Value);
            Assert.Equal(1, returnTrade.TradeId);

        }
        [Fact]
        public async Task GetTrade_ReturnsNotFound()
        {
            // Arrange
            _tradeService.Setup(service => service.GetTradeByIdAsync(1))
            .ReturnsAsync((Trade)null);

            // Act
            var result = await _controller.GetTrade(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }


        [Fact]
        public async Task GetTrade_ReturnsInternalServerError()
        {
            // Arrange
            _tradeService.Setup(service => service.GetTradeByIdAsync(1))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetTrade(1);

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }

        [Fact]
        public async Task PostTrade_ValidRequest()
        {
            // Arrange
            var request = new TradeRequest
            {

                Account = "Account1",
                AccountType = "AccountType1",
                BuyQuantity = 1,
                SellQuantity = 1,
                BuyPrice = 1,
                SellPrice = 1,
                Benchmark = "Benchmark1",
                TradeDate = DateTime.Now,
                TradeSecurity = "TradeSecurity1",
                TradeStatus = "TradeStatus1",
                Trader = "Trader1",
                Book = "Book1",
                CreationName = "CreationName1",
                CreationDate = DateTime.Now,
                RevisionName = "RevisionName1",
                RevisionDate = DateTime.Now,
                BuyCurrency = "USD",
                SellCurrency = "EUR"
            };

            _tradeService.Setup(service => service.AddTradeAsync(It.IsAny<Trade>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateTrade(request);

            //Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task PostTrade_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange

            _controller.ModelState.AddModelError("Account", "Required");
            var request = new TradeRequest
            {
                AccountType = "AccountType1",
                BuyQuantity = 1,
                SellQuantity = 1,
                BuyPrice = 1,
                SellPrice = 1,
                Benchmark = "Benchmark1",
                TradeDate = DateTime.Now,
                TradeSecurity = "TradeSecurity1",
                TradeStatus = "TradeStatus1",
                Trader = "Trader1",
                Book = "Book1",
                CreationName = "CreationName1",
                CreationDate = DateTime.Now,
                RevisionName = "RevisionName1",
                RevisionDate = DateTime.Now,
                BuyCurrency = "USD",
                SellCurrency = "EUR"
            };


            // Act
            var result = await _controller.CreateTrade(request);

            // Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, actionResult.StatusCode);
        }

        [Fact]
        public async Task PostTrade_ThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var request = new TradeRequest
            {
                Account = "Account1",
                AccountType = "AccountType1",
                BuyQuantity = 1,
                SellQuantity = 1,
                BuyPrice = 1,
                SellPrice = 1,
                Benchmark = "Benchmark1",
                TradeDate = DateTime.Now,
                TradeSecurity = "TradeSecurity1",
                TradeStatus = "TradeStatus1",
                Trader = "Trader1",
                Book = "Book1",
                CreationName = "CreationName1",
                CreationDate = DateTime.Now,
                RevisionName = "RevisionName1",
                RevisionDate = DateTime.Now,
                BuyCurrency = "USD",
                SellCurrency = "EUR"
            };

            _tradeService.Setup(service => service.AddTradeAsync(It.IsAny<Trade>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.CreateTrade(request);

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }

        [Fact]
        public async Task PutTrade_ValidRequest()
        {
            // Arrange
            var trade = new Trade
            {
                TradeId = 1,
                Account = "Account1",
                AccountType = "AccountType1",
                BuyQuantity = 1,
                SellQuantity = 1,
                BuyPrice = 1,
                SellPrice = 1,
                Benchmark = "Benchmark1",
                TradeDate = DateTime.Now,
                TradeSecurity = "TradeSecurity1",
                TradeStatus = "TradeStatus1",
                Trader = "Trader1",
                Book = "Book1",
                CreationName = "CreationName1",
                CreationDate = DateTime.Now,
                RevisionName = "RevisionName1",
                RevisionDate = DateTime.Now,
                BuyCurrency = "USD",
                SellCurrency = "EUR"
            };

            _tradeService.Setup(service => service.UpdateTradeAsync(It.IsAny<Trade>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateTrade(1, trade);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateTrade_ReturnsBadRequest_WhenIdDoesNotMatchTradeId()
        {
            // Arrange
            int id = 1;
            var trade = new Trade
            {
                TradeId = 2, // Different from the id parameter
                Account = "Account1",
                AccountType = "AccountType1",
                BuyQuantity = 1,
                SellQuantity = 1,
                BuyPrice = 1,
                SellPrice = 1,
                Benchmark = "Benchmark1",
                TradeDate = DateTime.Now,
                TradeSecurity = "TradeSecurity1",
                TradeStatus = "TradeStatus1",
                Trader = "Trader1",
                Book = "Book1",
                CreationName = "CreationName1",
                CreationDate = DateTime.Now,
                RevisionName = "RevisionName1",
                RevisionDate = DateTime.Now,
                DealName = "DealName1",
                DealType = "DealType1",
                SourceListId = "SourceListId1",
                Side = "Side1",
                BuyCurrency = "USD",
                SellCurrency = "EUR"
            };

            // Act
            var result = await _controller.UpdateTrade(id, trade);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task UpdateTrade_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            int id = 1;
            var trade = new Trade
            {
                TradeId = id,
                Account = "Account1",
                AccountType = "AccountType1",
                BuyQuantity = 1,
                SellQuantity = 1,
                BuyPrice = 1,
                SellPrice = 1,
                Benchmark = "Benchmark1",
                TradeDate = DateTime.Now,
                TradeSecurity = "TradeSecurity1",
                TradeStatus = "TradeStatus1",
                Trader = "Trader1",
                Book = "Book1",
                CreationName = "CreationName1",
                CreationDate = DateTime.Now,
                RevisionName = "RevisionName1",
                RevisionDate = DateTime.Now,
                DealName = "DealName1",
                DealType = "DealType1",
                SourceListId = "SourceListId1",
                Side = "Side1",
                BuyCurrency = "USD",
                SellCurrency = "EUR"
            };

            // Simulate invalid model state
            _controller.ModelState.AddModelError("Account", "The Account field is required.");

            // Act
            var result = await _controller.UpdateTrade(id, trade);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var modelState = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(modelState.ContainsKey("Account"));
        }

        [Fact]
        public async Task UpdateTrade_DbUpdateConcurrencyException_TradeNotFound_ReturnsNotFound()
        {
            // Arrange
            int id = 1;
            var trade = new Trade
            {
                TradeId = id,
                Account = "Account1",
                AccountType = "AccountType1",
                BuyQuantity = 1,
                SellQuantity = 1,
                BuyPrice = 1,
                SellPrice = 1,
                Benchmark = "Benchmark1",
                TradeDate = DateTime.Now,
                TradeSecurity = "TradeSecurity1",
                TradeStatus = "TradeStatus1",
                Trader = "Trader1",
                Book = "Book1",
                CreationName = "CreationName1",
                CreationDate = DateTime.Now,
                RevisionName = "RevisionName1",
                RevisionDate = DateTime.Now,
                DealName = "DealName1",
                DealType = "DealType1",
                SourceListId = "SourceListId1",
                Side = "Side1",
                BuyCurrency = "USD",
                SellCurrency = "EUR"
            };

            _tradeService.Setup(service => service.UpdateTradeAsync(It.IsAny<Trade>()))
       .ThrowsAsync(new DbUpdateConcurrencyException());


            // Act
            var result = await _controller.UpdateTrade(id, trade);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateTrade_ConcurrencyException_TradeExists_ThrowsException()
        {
            // Arrange
            var tradeServiceMock = new Mock<ITradeService>();
            var loggerMock = new Mock<ILogger<TradesController>>();
            var controller = new TradesController(tradeServiceMock.Object, loggerMock.Object);

            var trade = new Trade { TradeId = 1, Account = "Account1", TradeSecurity = "Security1", TradeDate = DateTime.Now };
            tradeServiceMock.Setup(s => s.TradeExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            tradeServiceMock.Setup(s => s.UpdateTradeAsync(It.IsAny<Trade>())).ThrowsAsync(new DbUpdateConcurrencyException());

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => controller.UpdateTrade(trade.TradeId, trade));
        }
        [Fact]
        public async Task UpdateTrade_ThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            int id = 1;
            var trade = new Trade
            {
                TradeId = id,
                Account = "Account1",
                AccountType = "AccountType1",
                BuyQuantity = 1,
                SellQuantity = 1,
                BuyPrice = 1,
                SellPrice = 1,
                Benchmark = "Benchmark1",
                TradeDate = DateTime.Now,
                TradeSecurity = "TradeSecurity1",
                TradeStatus = "TradeStatus1",
                Trader = "Trader1",
                Book = "Book1",
                CreationName = "CreationName1",
                CreationDate = DateTime.Now,
                RevisionName = "RevisionName1",
                RevisionDate = DateTime.Now,
                DealName = "DealName1",
                DealType = "DealType1",
                SourceListId = "SourceListId1",
                Side = "Side1",
                BuyCurrency = "USD",
                SellCurrency = "EUR"
            };

            _tradeService.Setup(service => service.UpdateTradeAsync(It.IsAny<Trade>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.UpdateTrade(id, trade);

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }

        [Fact]
        public async Task DeleteTrade_ReturnsNoContent_WhenTradeExists()
        {
            // Arrange
            var tradeId = 1;
            var trade = new Trade { TradeId = tradeId };
            _tradeService.Setup(x => x.GetTradeByIdAsync(tradeId)).ReturnsAsync(trade);
            _tradeService.Setup(x => x.DeleteTradeAsync(tradeId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteTrade(tradeId);

            // Assert
            Assert.IsType<NoContentResult>(result);

        }

        [Fact]
        public async Task DeleteTrade_ReturnsNotFound_WhenTradeDoesNotExist()
        {
            // Arrange
            var tradeId = 1;
            _tradeService.Setup(x => x.GetTradeByIdAsync(tradeId)).ReturnsAsync((Trade)null);

            // Act
            var result = await _controller.DeleteTrade(tradeId);

            // Assert
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        public async Task DeleteTrade_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var tradeId = 1;
            _tradeService.Setup(x => x.GetTradeByIdAsync(tradeId)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.DeleteTrade(tradeId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);

        }


    }
}
