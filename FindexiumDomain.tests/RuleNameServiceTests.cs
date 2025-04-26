using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Domain.Services;
using Moq;

namespace FindexiumDomain.tests
{
    public class RuleNameServiceTests
    {
        private readonly Mock<IRuleNameRepository> _mockRuleNameRepository;
        private readonly IRuleNameServices _ruleNameService;

        public RuleNameServiceTests()
        {
            _mockRuleNameRepository = new Mock<IRuleNameRepository>();
            _ruleNameService = new RuleNameService(_mockRuleNameRepository.Object);
        }

        #region GetAllRatingsAsync Tests

        [Fact]
        public async Task GetAllRatingsAsync_ReturnsListOfRuleNames()
        {
            // Arrange
            var ruleNames = new List<RuleName>
                {
                    new RuleName { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SELECT 1", SqlPart = "Part1" },
                    new RuleName { Id = 2, Name = "Rule2", Description = "Description2", Json = "{}", Template = "Template2", SqlStr = "SELECT 2", SqlPart = "Part2" }
                };
            _mockRuleNameRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(ruleNames);

            // Act
            var result = (await _ruleNameService.GetAllRulesAsync()).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Collection(result,
                item => Assert.Equal(1, item.Id),
                item => Assert.Equal(2, item.Id));
        }

        [Fact]
        public async Task GetAllRatingsAsync_ThrowsApplicationException()
        {
            // Arrange
            _mockRuleNameRepository.Setup(repo => repo.GetAllAsync()).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _ruleNameService.GetAllRulesAsync());
            Assert.Equal("An error occurred while retrieving all rule names.", exception.Message);
        }

        #endregion

        #region GetRuleByIdAsync Tests

        [Fact]
        public async Task GetRuleByIdAsync_ReturnsRuleName()
        {
            // Arrange
            var ruleName = new RuleName { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SELECT 1", SqlPart = "Part1" };
            _mockRuleNameRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ruleName);

            // Act
            var result = await _ruleNameService.GetRuleByIdAsync(1);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("Rule1", result.Name);
        }

        [Fact]
        public async Task GetRuleByIdAsync_ThrowsApplicationException()
        {
            // Arrange
            _mockRuleNameRepository.Setup(repo => repo.GetByIdAsync(1)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _ruleNameService.GetRuleByIdAsync(1));
            Assert.Equal("An error occurred while retrieving the rule name with ID 1.", exception.Message);
        }

        [Fact]
        public async Task GetRuleByIdAsync_ReturnsTrueIfExists()
        {
            // Arrange
            var ruleName = new RuleName { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SELECT 1", SqlPart = "Part1" };
            _mockRuleNameRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ruleName);

            // Act
            var result = await _ruleNameService.GetRuleByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        #endregion

        #region AddRuleAsync Tests

        [Fact]
        public async Task AddRuleAsync_AddsRuleName()
        {
            // Arrange
            var ruleName = new RuleName { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SELECT 1", SqlPart = "Part1" };
            _mockRuleNameRepository.Setup(repo => repo.AddAsync(ruleName)).Returns(Task.CompletedTask);

            // Act
            await _ruleNameService.AddRuleAsync(ruleName);

            // Assert
            _mockRuleNameRepository.Verify(repo => repo.AddAsync(ruleName), Times.Once);
        }

        [Fact]
        public async Task AddRuleAsync_ThrowsApplicationException()
        {
            // Arrange
            var ruleName = new RuleName { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SELECT 1", SqlPart = "Part1" };
            _mockRuleNameRepository.Setup(repo => repo.AddAsync(ruleName)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _ruleNameService.AddRuleAsync(ruleName));
            Assert.Equal("An error occurred while adding a new rule name.", exception.Message);
        }

        [Fact]
        public async Task AddRuleAsync_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ruleNameService.AddRuleAsync(null));
        }

        #endregion

        #region UpdateRuleAsync Tests

        [Fact]
        public async Task UpdateRuleAsync_UpdatesRuleName()
        {
            // Arrange
            var ruleName = new RuleName { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SELECT 1", SqlPart = "Part1" };
            _mockRuleNameRepository.Setup(repo => repo.ExistsAsync(1)).ReturnsAsync(true);
            _mockRuleNameRepository.Setup(repo => repo.UpdateAsync(ruleName)).Returns(Task.CompletedTask);

            // Act
            await _ruleNameService.UpdateRuleAsync(ruleName);

            // Assert
            _mockRuleNameRepository.Verify(repo => repo.UpdateAsync(ruleName), Times.Once);
        }

        [Fact]
        public async Task UpdateRuleAsync_ThrowsApplicationException()
        {
            // Arrange
            var ruleName = new RuleName { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SELECT 1", SqlPart = "Part1" };
            _mockRuleNameRepository.Setup(repo => repo.ExistsAsync(1)).ReturnsAsync(true);
            _mockRuleNameRepository.Setup(repo => repo.UpdateAsync(ruleName)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _ruleNameService.UpdateRuleAsync(ruleName));
            Assert.Equal("An error occurred while updating the rule name with ID 1.", exception.Message);
        }

        [Fact]
        public async Task UpdateRuleAsync_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ruleNameService.UpdateRuleAsync(null));
        }

        [Fact]
        public async Task UpdateRuleAsync_ThrowsInvalidOperationException()
        {
            // Arrange
            var ruleName = new RuleName { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SELECT 1", SqlPart = "Part1" };
            _mockRuleNameRepository.Setup(repo => repo.ExistsAsync(1)).ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _ruleNameService.UpdateRuleAsync(ruleName));
            Assert.Equal("Rule name with ID 1 does not exist.", exception.Message);
        }

        #endregion

        #region DeleteRuleAsync Tests

        [Fact]
        public async Task DeleteRuleAsync_DeletesRuleName()
        {
            // Arrange
            _mockRuleNameRepository.Setup(repo => repo.ExistsAsync(1)).ReturnsAsync(true);
            _mockRuleNameRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _ruleNameService.DeleteRuleAsync(1);

            // Assert
            _mockRuleNameRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteRuleAsync_ThrowsApplicationException()
        {
            // Arrange
            _mockRuleNameRepository.Setup(repo => repo.ExistsAsync(1)).ReturnsAsync(true);
            _mockRuleNameRepository.Setup(repo => repo.DeleteAsync(1)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _ruleNameService.DeleteRuleAsync(1));
            Assert.Equal("An error occurred while deleting the rule name with ID 1.", exception.Message);
        }

        [Fact]
        public async Task DeleteRuleAsync_ThrowsInvalidOperationException()
        {
            // Arrange
            _mockRuleNameRepository.Setup(repo => repo.ExistsAsync(1)).ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _ruleNameService.DeleteRuleAsync(1));
            Assert.Equal("Rule name with ID 1 does not exist.", exception.Message);
        }

        #endregion

       
    }
}
