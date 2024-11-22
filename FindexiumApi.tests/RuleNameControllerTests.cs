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
    public class RuleNameControllerTests
    {
        private readonly Mock<IRuleNameServices> _mockRuleNameService;
        private readonly Mock<ILogger<RuleNameController>> _mockLogger;
        private readonly RuleNameController _mockRuleNameController;


        public RuleNameControllerTests()
        {
            _mockRuleNameService = new Mock<IRuleNameServices>();
            _mockLogger = new Mock<ILogger<RuleNameController>>();
            _mockRuleNameController = new RuleNameController(_mockRuleNameService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetRuleNames_ReturnsOkResult()
        {
            // Arrange
            var ruleNames = new List<RuleName>
            {
                new RuleName { Id = 1, Name = "Rule1", Description = "Rule1 Description", Json = "Rule1 Json" },
                new RuleName { Id = 2, Name = "Rule2", Description = "Rule2 Description", Json = "Rule2 Json" },
                new RuleName { Id = 3, Name = "Rule3", Description = "Rule3 Description", Json = "Rule3 Json" }
            };
            _mockRuleNameService.Setup(service => service.GetAllRatingsAsync()).ReturnsAsync(ruleNames);

            // Act
            var result = await _mockRuleNameController.GetRuleName();

            // Assert

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnRuleNames = Assert.IsType<List<RuleName>>(okResult.Value);
            Assert.Equal(3, returnRuleNames.Count);
            Assert.Collection(returnRuleNames,
                item => Assert.Equal("Rule1", item.Name),
                item => Assert.Equal("Rule2", item.Name),
                item => Assert.Equal("Rule3", item.Name));

        }

        [Fact]
        public async Task GetRuleNameList_ReturnsInternalServerError_OnException()
        {
            //Arrange
            _mockRuleNameService.Setup(service => service.GetAllRatingsAsync())
                .ThrowsAsync(new System.Exception("An error occurred"));

            //Act
            var result = await _mockRuleNameController.GetRuleName();

            //Assert

            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Internal server error", statusCodeResult.Value);
        }

        [Fact]
        public async Task GetRuleName_ById_ReturnsOkResult_WithRuleName()
        {
            //Arrange
            var ruleName = new RuleName
            {
                Id = 1,
                Name = "Rule1",
                Description = "Rule1 Description",
                Json = "Rule1 Json"
            };
            _mockRuleNameService.Setup(service => service.GetRuleByIdAsync(1))
                .ReturnsAsync(ruleName);

            //Act
            var result = await _mockRuleNameController.GetRuleName(1);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnRuleName = Assert.IsType<RuleName>(okResult.Value);
            Assert.Equal("Rule1", returnRuleName.Name);
            Assert.True(returnRuleName.Id == 1);
            Assert.Equal("Rule1 Description", returnRuleName.Description);
        }

        [Fact]
        public async Task GetRuleName_ById_ReturnsNotFound_WhenRuleNameIsNull()
        {
            //Arrange
            int testId = 1;
            _mockRuleNameService.Setup(service => service.GetRuleByIdAsync(testId))
                .ReturnsAsync((RuleName)null);

            //Act
            var result = await _mockRuleNameController.GetRuleName(testId);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetRuleName_ById_ReturnInternalServorError_OnException()
        {
            //Arrange
            int testId = 1;
            _mockRuleNameService.Setup(service => service.GetRuleByIdAsync(testId))
                .ThrowsAsync(new System.Exception("An error occurred"));

            //Act
            var result = await _mockRuleNameController.GetRuleName(testId);

            //Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Internal server error", statusCodeResult.Value);
        }

        [Fact]
        public async Task PostRuleName_ValiRequest()
        {
            //Arrange
            var request = new RuleNameRequest
            {
                Name = "Rule1",
                Description = "Rule1 Description",
                Json = "Rule1 Json"
            };

            //Act
            var result = await _mockRuleNameController.PostRuleName(request);

            //Assert
            Assert.IsType<CreatedResult>(result);

        }

        [Fact]
        public async Task PostRuleName_InvalidModelState_WhenRatingIsNull()
        {
            _mockRuleNameController.ModelState.AddModelError("Name", "Name is required");
            var request = new RuleNameRequest
            {
                Name = null,
                Description = "Rule1 Description",
                Json = "Rule1 Json"
            };

            //Act
            var result = await _mockRuleNameController.PostRuleName(request);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PostRuleName_ReturnsInternalServerError()
        {
            //Arrange
            var request = new RuleNameRequest
            {
                Name = "Rule1",
                Description = "Rule1 Description",
                Json = "Rule1 Json"
            };

            _mockRuleNameService.Setup(service => service.AddRuleAsync(It.IsAny<RuleName>()))
                .ThrowsAsync(new System.Exception("An error occurred"));

            //Act
            var result = await _mockRuleNameController.PostRuleName(request);


            //Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)actionResult).StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }

        [Fact]
        public async Task PutRuleName_ReturnsNoContent()
        {
            //Arrange
            var ruleName = new RuleName
            {
                Id = 1,
                Name = "Rule1",
                Description = "Rule1 Description",
                Json = "Rule1 Json"
            };

            //Act
            var result = await _mockRuleNameController.PutRuleName(1, ruleName);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutRuleName_ReturnsBadRequest_WhenIdMismatch()
        {
            //Arrange
            var ruleNameId = 2;
            var ruleName = new RuleName
            {
                Id = 1,
                Name = "Rule1",
                Description = "Rule1 Description",
                Json = "Rule1 Json"
            };

            //Act
            var result = await _mockRuleNameController.PutRuleName(ruleNameId, ruleName);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutRuleName_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
        {
            //Arrange
            var ruleName = new RuleName
            {
                Id = 1,
                Name = "Rule1",
                Description = "Rule1 Description",
                Json = "Rule1 Json"
            };

            _mockRuleNameService.Setup(service => service.UpdateRuleAsync(It.IsAny<RuleName>()))
                .ThrowsAsync(new ArgumentException("Invalid argument"));

            //Act
            var result = await _mockRuleNameController.PutRuleName(1, ruleName);

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid argument", actionResult.Value);
        }
        [Fact]
       public async Task PutRuleName_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            //Arrange
            var ruleName = new RuleName
            {
                Id = 1,
                Name = "Rule1",
                Description = "Rule1 Description",
                Json = "Rule1 Json"
            };

            _mockRuleNameService.Setup(service => service.UpdateRuleAsync(It.IsAny<RuleName>()))
                .ThrowsAsync(new System.Exception("An error occurred"));

            //Act
            var result = await _mockRuleNameController.PutRuleName(1, ruleName);

            //Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)actionResult).StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }

        [Fact]
        public async Task DeleteRuleName_ReturnsNoContent()
        {
            // Arrange
            int ruleNameId = 1;
            var ruleName = new RuleName
            {
                Id = ruleNameId,
                Name = "Rule1",
                Description = "Rule1 Description",
                Json = "Rule1 Json"
            };

            _mockRuleNameService.Setup(service => service.GetRuleByIdAsync(ruleNameId))
                .ReturnsAsync(ruleName);

            _mockRuleNameService.Setup(service => service.DeleteRuleAsync(ruleNameId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _mockRuleNameController.DeleteRuleName(ruleNameId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteRuleName_ReturnsNotFound_WhenRuleNameIsNull()
        {
            // Arrange
            int ruleNameId = 1;
            _mockRuleNameService.Setup(service => service.GetRuleByIdAsync(ruleNameId))
                .ReturnsAsync((RuleName)null);

            // Act
            var result = await _mockRuleNameController.DeleteRuleName(ruleNameId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteRuleName_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            int ruleNameId = 1;
            _mockRuleNameService.Setup(service => service.GetRuleByIdAsync(ruleNameId))
                .ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _mockRuleNameController.DeleteRuleName(ruleNameId);

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }
    }
}