using System.Collections.Generic;
using System.Net.NetworkInformation;
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
    public class BidControllerTests
    {
        private readonly Mock<IBidListServices> _mockBidService;
        private readonly Mock<ILogger<BidListController>> _mockLogger;
        private readonly BidListController _controller;

        public BidControllerTests()
        {
            _mockBidService = new Mock<IBidListServices>();
            _mockLogger = new Mock<ILogger<BidListController>>();
            _controller = new BidListController(_mockBidService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetBidList_ReturnsOkResult_WithListOfThreeBids()
        {
            // Arrange
            var bids = new List<BidList>
            {
                new() { Account = "Account1", BidType = "Type1", BidQuantity = 1, AskQuantity = 1, Bid = 1.0, Ask = 1.0, Benchmark = "Benchmark1", BidListDate = new DateTime(2021, 1, 1),Commentary = "comment1",BidSecurity="Secure1",BidStatus ="status1",Trader="Trader1",Book="Book1",CreationName="name1",CreationDate= new DateTime(2021,1,1), RevisionName="name1",RevisionDate=new DateTime(2022,1,1), DealName="name1", DealType="type1",SourceListId="Id1",Side="Side1" },
                new() { Account = "Account2", BidType = "Type2", BidQuantity = 2, AskQuantity = 2, Bid = 2.0, Ask = 2.0, Benchmark = "Benchmark2",  BidListDate = new DateTime(2021, 1, 2),Commentary = "comment2",BidSecurity="Secure2",BidStatus ="status2",Trader="Trader2",Book="Book2",CreationName="name2",CreationDate= new DateTime(2021,1,1), RevisionName="name2",RevisionDate=new DateTime(2022,1,2), DealName="name2", DealType="type2",SourceListId="Id2",Side="Side2"  },
                new() { Account = "Account3", BidType = "Type3", BidQuantity = 3, AskQuantity = 3, Bid = 3.0, Ask = 3.0, Benchmark = "Benchmark3",  BidListDate = new DateTime(2021, 1, 3),Commentary = "comment3",BidSecurity="Secure3",BidStatus ="status3",Trader="Trader3",Book="Book3",CreationName="name3",CreationDate= new DateTime(2021,1,1), RevisionName="name3",RevisionDate=new DateTime(2022,1,3), DealName="name3", DealType="type3",SourceListId="Id3",Side="Side3"  }
            };
            _mockBidService.Setup(service => service.GetAllAsync()).ReturnsAsync(bids);

            // Act
            var result = await _controller.GetBids();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnBids = Assert.IsType<List<BidResponse>>(okResult.Value);
            Assert.Equal(3, returnBids.Count);
            Assert.Collection(returnBids,
                item => Assert.Equal("Account1", item.Account),
                item => Assert.Equal("Account2", item.Account),
                item => Assert.Equal("Account3", item.Account));
        }



        [Fact]
        public async Task GetBids_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _mockBidService.Setup(service => service.GetAllAsync()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetBids();

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }

        [Fact]
        public async Task GetBid_ById_ReturnsOkResult_WithBidResponse()
        {
            // Arrange
            var bid = new BidList
            {
                BidListId = -1,
                Account = "Account1",
                BidType = "Type1",
                BidQuantity = 1
            };

            _mockBidService.Setup(service => service.GetByIdAsync(-1)).ReturnsAsync(bid);

            // Act
            var result = await _controller.GetBid(-1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnBid = Assert.IsType<BidResponse>(okResult.Value);
            Assert.Equal("Account1", returnBid.Account);
        }


        [Fact]
        public async Task GetBid_ReturnsNotFound_WhenBidIsNull()
        {
            // Arrange
            int bidId = 1;


            _mockBidService.Setup(service => service.GetByIdAsync(bidId))
                .ReturnsAsync((BidList)null);

            // Act
            var result = await _controller.GetBid(bidId);

            // Assert

            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetBid_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            int testId = 1;
            _mockBidService.Setup(service => service.GetByIdAsync(It.IsAny<int>()))
                               .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetBid(testId);

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }



        [Fact]
        public async Task PostBid_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var bidRequest = new BidRequest
            {
                Account = "TestAccount",
                BidType = "TestType",
                BidQuantity = 100,
              
            };



            _mockBidService.Setup(service => service.AddAsync(It.IsAny<BidList>())).Returns(Task.CompletedTask);
            // _mockBidService.Setup(service => service.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await _controller.PostBidList(bidRequest);

            // Assert

            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task PostBidList_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Account", "Required");
            var request = new BidRequest();

            // Act
            var result = await _controller.PostBidList(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task PostBidList_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var request = new BidRequest
            {
                Account = "TestAccount",
                BidType = "TestType",
                BidQuantity = 100,
             
            };



            _mockBidService.Setup(service => service.AddAsync(It.IsAny<BidList>())).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.PostBidList(request);

            //Assert
            // Vérifie que le résultat de l'action est de type ObjectResult
            var actionResult = Assert.IsType<ObjectResult>(result);
            // Vérifie que le code de statut de la réponse est 500 (Internal Server Error)
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode);
            // Vérifie que le message de la réponse est "Internal server error"
            Assert.Equal("Internal server error", actionResult.Value);
        }


        [Fact]
        public async Task PutBid_ReturnsNoContent_WhenBidIsUpdated()
        {
            // Arrange
            var Id = 1;
            var bidRequest = new BidRequest
            {
                Account = "TestAccount",
                BidType = "TestType",
                BidQuantity = 100,
            };

            var existingBid = new BidList
            {
                BidListId = Id,
                Account = "ExistingAccount",
                BidType = "ExistingType",
                BidQuantity = 50
            };

            _mockBidService.Setup(service => service.GetByIdAsync(Id)).ReturnsAsync(existingBid);
            _mockBidService.Setup(service => service.UpdateAsync(Id, It.IsAny<BidList>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PutBid(Id, bidRequest);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

      

        [Fact]
        public async Task PutBid_ReturnsNotFound_WhenBidDoesNotExist()
        {
            // Arrange
            var Id = 1;
            var bidRequest = new BidRequest
            {
                Account = "TestAccount",
                BidType = "TestType",
                BidQuantity = 100,
            };

            _mockBidService.Setup(service => service.GetByIdAsync(Id))
                .ReturnsAsync((BidList)null);

            // Act
            var result = await _controller.PutBid(Id, bidRequest);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutBid_ModelInvalid()
        {
            //Arrange
            var id = 1;
            var bidRequest = new BidRequest
            {
               
                BidType = "TestType",
                BidQuantity = 100
            };

            _controller.ModelState.AddModelError("Account", "Required");

            // Act
            var result = await _controller.PutBid(id, bidRequest);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            
        }

       

     

        [Fact]
        public async Task PutBid_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var Id = 1;
            var bidRequest = new BidRequest
            {
                Account = "TestAccount",
                BidType = "TestType",
                BidQuantity = 100
            };

            _mockBidService.Setup(service => service.GetByIdAsync(Id)).ReturnsAsync(new BidList { BidListId = Id });
            _mockBidService.Setup(service => service.UpdateAsync(Id, It.IsAny<BidList>())).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.PutBid(Id, bidRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task DeleteBidList_ReturnsNoContent_WhenBidIsDeleted()
        {
            // Arrange
            int bidId = 1;
            _mockBidService.Setup(s => s.GetByIdAsync(bidId)).ReturnsAsync(new BidList { BidListId = bidId });
            _mockBidService.Setup(s => s.DeleteAsync(bidId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteBidList(bidId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteBidList_ReturnsNotFound_WhenBidDoesNotExist()
        {
            // Arrange
            int bidId = 1;
            _mockBidService.Setup(s => s.GetByIdAsync(bidId)).ReturnsAsync((BidList)null);

            // Act
            var result = await _controller.DeleteBidList(bidId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteBidList_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            int bidId = 1;
            _mockBidService.Setup(s => s.GetByIdAsync(bidId)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.DeleteBidList(bidId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }


    }
}
