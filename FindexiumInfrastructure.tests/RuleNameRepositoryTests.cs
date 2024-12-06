using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Findexium.Infrastructure.Models;
using Findexium.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace FindexiumInfrastructure.tests
{
    public class RuleNameRepositoryTests
    {
        private readonly DbContextOptions<LocalDbContext> _dbContextOptions;
        private readonly Mock<ILogger<RuleNameRepository>> _mockLogger;

        public RuleNameRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<LocalDbContext>()
                .UseInMemoryDatabase(databaseName: "FindexiumTestDb")
                .Options;

            _mockLogger = new Mock<ILogger<RuleNameRepository>>();
        }

        private RuleNameRepository CreateRepository(LocalDbContext context)
        {
            return new RuleNameRepository(context, _mockLogger.Object);
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
            context.RuleNames.RemoveRange(context.RuleNames);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllRuleNames()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_GetAllAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var ruleNames = new List<RuleNameDto>
                    {
                        new RuleNameDto(1, "Rule1", "Description1", "{}", "Template1", "SQL1", "SQLPart1"),
                        new RuleNameDto(2, "Rule2", "Description2", "{}", "Template2", "SQL2", "SQLPart2"),
                        new RuleNameDto(3, "Rule3", "Description3", "{}", "Template3", "SQL3", "SQLPart3")
                    };

                context.RuleNames.AddRange(ruleNames);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                Assert.Equal(3, result.Count());
                Assert.Equal(1, result.First().Id);
                Assert.Equal("Rule1", result.First().Name);
            }
        }

        [Fact]
        public async Task GetAllAsync_WhenExceptionIsThrown()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_GetAllAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<RuleNameRepository>>();
                var repository = new RuleNameRepository(context, mockLogger.Object);

                var mockSet = new Mock<DbSet<RuleNameDto>>();
                mockSet.As<IQueryable<RuleNameDto>>().Setup(m => m.Provider)
                    .Throws(new Exception("An error occurred while retrieving all rule names."));
                context.RuleNames = mockSet.Object;

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.GetAllAsync());
                Assert.Equal("An error occurred while retrieving all rule names.", exception.Message);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsRuleName()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_GetByIdAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var ruleName = new RuleNameDto(1, "Rule1", "Description1", "{}", "Template1", "SQL1", "SQLPart1");
                context.RuleNames.Add(ruleName);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetByIdAsync(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal("Rule1", result.Name);
            }
        }

        [Fact]
        public async Task GetByIdAsync_WhenExceptionIsThrown()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_GetByIdAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<RuleNameRepository>>();
                var repository = new RuleNameRepository(context, mockLogger.Object);

                var mockSet = new Mock<DbSet<RuleNameDto>>();
                mockSet.Setup(m => m.FindAsync(It.IsAny<int>()))
                    .Throws(new Exception("An error occurred while retrieving the rule name."));
                context.RuleNames = mockSet.Object;

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.GetByIdAsync(1));
                Assert.Equal("An error occurred while retrieving the rule name.", exception.Message);
            }
        }

        [Fact]
        public async Task AddAsync_AddsRuleName()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_AddAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var ruleName = new RuleName
                {
                    Id = 1,
                    Name = "Rule1",
                    Description = "Description1",
                    Json = "{}",
                    Template = "Template1",
                    SqlStr = "SQL1",
                    SqlPart = "SQLPart1"
                };

                // Act
                await repository.AddAsync(ruleName);

                // Assert
                var addedRuleName = await context.RuleNames.FindAsync(1);
                Assert.NotNull(addedRuleName);
                Assert.Equal("Rule1", addedRuleName.Name);
            }
        }

        [Fact]
        public async Task AddAsync_WhenExceptionIsThrown()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_AddAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<RuleNameRepository>>();
                var repository = new RuleNameRepository(context, mockLogger.Object);

                var mockSet = new Mock<DbSet<RuleNameDto>>();
                mockSet.Setup(m => m.Add(It.IsAny<RuleNameDto>()))
                    .Throws(new Exception("An error occurred while adding the rule name."));
                context.RuleNames = mockSet.Object;

                var ruleName = new RuleName
                {
                    Id = 1,
                    Name = "Rule1",
                    Description = "Description1",
                    Json = "{}",
                    Template = "Template1",
                    SqlStr = "SQL1",
                    SqlPart = "SQLPart1"
                };

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.AddAsync(ruleName));
                Assert.Equal("An error occurred while adding the rule name.", exception.Message);
            }
        }

        [Fact]
        public async Task UpdateAsync_UpdatesRuleName()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_UpdateAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var ruleName = new RuleNameDto(1, "Rule1", "Description1", "{}", "Template1", "SQL1", "SQLPart1");
                context.RuleNames.Add(ruleName);
                await context.SaveChangesAsync();

                var updatedRuleName = new RuleName
                {
                    Id = 1,
                    Name = "UpdatedRule1",
                    Description = "UpdatedDescription1",
                    Json = "{}",
                    Template = "UpdatedTemplate1",
                    SqlStr = "UpdatedSQL1",
                    SqlPart = "UpdatedSQLPart1"
                };

                // Act
                await repository.UpdateAsync(updatedRuleName);

                // Assert
                var result = await context.RuleNames.FindAsync(1);
                Assert.NotNull(result);
                Assert.Equal("UpdatedRule1", result.Name);
                Assert.Equal("UpdatedDescription1", result.Description);
            }
        }

        [Fact]
        public async Task UpdateAsync_WhenExceptionIsThrown()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_UpdateAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<RuleNameRepository>>();
                var repository = new RuleNameRepository(context, mockLogger.Object);

                var mockSet = new Mock<DbSet<RuleNameDto>>();
                mockSet.Setup(m => m.FindAsync(It.IsAny<int>()))
                    .Throws(new Exception("An error occurred while updating the rule name."));
                context.RuleNames = mockSet.Object;

                var ruleName = new RuleName
                {
                    Id = 1,
                    Name = "UpdatedRule1",
                    Description = "UpdatedDescription1",
                    Json = "{}",
                    Template = "UpdatedTemplate1",
                    SqlStr = "UpdatedSQL1",
                    SqlPart = "UpdatedSQLPart1"
                };

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.UpdateAsync(ruleName));
                Assert.Equal("An error occurred while updating the rule name.", exception.Message);
            }
        }

        [Fact]
        public async Task DeleteAsync_DeletesRuleName()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_DeleteAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var ruleName = new RuleNameDto(1, "Rule1", "Description1", "{}", "Template1", "SQL1", "SQLPart1");
                context.RuleNames.Add(ruleName);
                await context.SaveChangesAsync();

                // Act
                await repository.DeleteAsync(1);

                // Assert
                var deletedRuleName = await context.RuleNames.FindAsync(1);
                Assert.Null(deletedRuleName);
            }
        }

        [Fact]
        public async Task DeleteAsync_WhenExceptionIsThrown()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_DeleteAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<RuleNameRepository>>();
                var repository = new RuleNameRepository(context, mockLogger.Object);

                var mockSet = new Mock<DbSet<RuleNameDto>>();
                mockSet.Setup(m => m.FindAsync(It.IsAny<int>()))
                    .Throws(new Exception("An error occurred while deleting the rule name."));
                context.RuleNames = mockSet.Object;

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.DeleteAsync(1));
                Assert.Equal("An error occurred while deleting the rule name.", exception.Message);
            }
        }

        [Fact]
        public async Task ExistsAsync_WhenExceptionIsThrown()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_ExistsAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<RuleNameRepository>>();
                var repository = new RuleNameRepository(context, mockLogger.Object);

                var mockSet = new Mock<DbSet<RuleNameDto>>();
                mockSet.As<IQueryable<RuleNameDto>>().Setup(m => m.Provider)
                    .Throws(new Exception("An error occurred while checking if the rule name exists."));
                context.RuleNames = mockSet.Object;

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.ExistsAsync(1));
                Assert.Equal("An error occurred while checking if the rule name exists.", exception.Message);
            }
        }
    }
}
