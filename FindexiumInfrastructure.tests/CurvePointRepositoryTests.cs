using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Findexium.Infrastructure.Models;
using Findexium.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Moq;


namespace FindexiumInfrastructure.tests
{
    public class CurvePointRepositoryTests
    {
        private readonly DbContextOptions<LocalDbContext> _dbContextOptions;
        private readonly Mock<ILogger<CurvePointRepository>> _mockLogger;


        public CurvePointRepositoryTests()
        {
            //Initialize DbContext and Logger
            _dbContextOptions = new DbContextOptionsBuilder<LocalDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            //Initialize logger mock
            _mockLogger = new Mock<ILogger<CurvePointRepository>>();
        }

        private CurvePointRepository CreateRepository(LocalDbContext context)
        {
            return new CurvePointRepository(context, _mockLogger.Object);
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
            context.CurvePoints.RemoveRange(context.CurvePoints);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCurvePoints()
        {
            //Arrange
            using (var context = CreateContext("TestDatabase_GetallAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var curvePoints = new List<CurvePointsDto>
                {
                    new CurvePointsDto(1, DateTime.Now, 1.0, 1.0, DateTime.Now),
                    new CurvePointsDto(1, DateTime.Now, 2.0, 2.0, DateTime.Now),
                    new CurvePointsDto(1, DateTime.Now, 3.0, 3.0, DateTime.Now)
                };

                context.CurvePoints.AddRange(curvePoints);
                await context.SaveChangesAsync();

                //Act
                var result = await repository.GetAllAsync();

                //Assert
                Assert.Equal(3, result.Count());
                Assert.Equal(1, result.First().CurveId);
                Assert.Equal(1.0, result.First().Term);
            }
        }

        [Fact]
        public async Task GetAllAsync_ThrowsApplicationException_WhenExceptionOccurs()
        {
            //Arrange
            using (var context = CreateContext("TestDatabase_GetAllAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<CurvePointRepository>>();
                var repository = new CurvePointRepository(context, mockLogger.Object);

                // Create a mock DbSet and configure it to throw an exception when ToListAsync is called
                var mockSet = new Mock<DbSet<CurvePointsDto>>();
                mockSet.As<IQueryable<CurvePointsDto>>().Setup(m => m.Provider).Throws(new Exception("Mock exception"));
                context.CurvePoints = mockSet.Object;

                //Act and Assert
                var exception = await Assert.ThrowsAsync<ApplicationException>(() => repository.GetAllAsync());
                Assert.Equal("An error occurred while retrieving all curve points.", exception.Message);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCurvePoint_WhenIdExists()
        {
            //Arrange
            using (var context = CreateContext("TestDatabase_GetByIdAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var curvePoint = new CurvePointsDto(1, DateTime.Now, 1.0, 1.0, DateTime.Now);
                context.CurvePoints.Add(curvePoint);
                await context.SaveChangesAsync();

                //Act
                var result = await repository.GetByIdAsync(curvePoint.Id);

                //Assert
                Assert.NotNull(result);
                Assert.Equal(curvePoint.CurveId, result.CurveId);
                Assert.Equal(curvePoint.Term, result.Term);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenIdDoesNotExist()
        {
            //Arrange
            using (var context = CreateContext("TestDatabase_GetByIdAsync_NotFound"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                //Act
                var result = await repository.GetByIdAsync(1);

                //Assert
                Assert.Null(result);
            }
        }
        [Fact]
        public async Task GetByIdAsync_ThrowsException_WhenExceptionOccurs()
        {
            //Arrange
            using (var context = CreateContext("TestDatabase_GetByIdAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<CurvePointRepository>>();
                var repository = new CurvePointRepository(context, mockLogger.Object);

                // Create a mock DbSet and configure it to throw an exception when FindAsync is called
                var mockSet = new Mock<DbSet<CurvePointsDto>>();
                mockSet.Setup(m => m.FindAsync(It.IsAny<int>())).Throws(new Exception("Mock exception"));
                context.CurvePoints = mockSet.Object;

                //Act and Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.GetByIdAsync(1));
                Assert.Equal("Error occurred while fetching curve point with id: 1", exception.Message);
            }
        }

        [Fact]
        public async Task AddAsync_AddsCurvePoint()
        {
            //Arrange
            using (var context = CreateContext("TestDatabase_AddAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var curvePoint = new CurvePoint
                {
                    CurveId = 1,
                    AsOfDate = DateTime.Now,
                    Term = 1.0,
                    CurvePointValue = 1.0,
                    CreationDate = DateTime.Now
                };

                //Act
                await repository.AddAsync(curvePoint);

                //Assert
                var result = await context.CurvePoints.FirstOrDefaultAsync();
                Assert.NotNull(result);
                Assert.Equal(curvePoint.CurveId, result.CurveId);
                Assert.Equal(curvePoint.Term, result.Term);
            }
        }

        [Fact]
        public async Task AddAsync_ThrowsApplicationException_WhenExceptionOccurs()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_AddAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<CurvePointRepository>>();
                var repository = new CurvePointRepository(context, mockLogger.Object);

                // Create a mock DbSet and configure it to throw an exception when Add is called
                var mockSet = new Mock<DbSet<CurvePointsDto>>();
                mockSet.Setup(m => m.Add(It.IsAny<CurvePointsDto>())).Throws(new Exception("Mock exception"));
                context.CurvePoints = mockSet.Object;

                var curvePoint = new CurvePoint
                {
                    CurveId = 1,
                    AsOfDate = DateTime.Now,
                    Term = 1.0,
                    CurvePointValue = 1.0,
                    CreationDate = DateTime.Now
                };

                // Act and Assert
                var exception = await Assert.ThrowsAsync<ApplicationException>(() => repository.AddAsync(curvePoint));
                Assert.Equal("An error occurred while adding curve point.", exception.Message);
            }
        }

        [Fact]
        public async Task UpdateAsync_WhenCurvePointIsNull()
        {
            //Arrange
            using (var context = CreateContext("TestDatabase_UpdateAsync_Null"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                //Act and Assert
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => repository.UpdateAsync(1, null));
                Assert.Equal("curvePoint", exception.ParamName);
            }
        }

        [Fact]
        public async Task UpdateAsync_WhenCurvePointDoesNotExist()
        {
            //Arrange
            using (var context = CreateContext("TestDatabase_UpdateAsync_NotFound"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var curvePoint = new CurvePoint
                {
                    Id = 1,
                    CurveId = 1,
                    AsOfDate = DateTime.Now,
                    Term = 1.0,
                    CurvePointValue = 1.0,
                    CreationDate = DateTime.Now
                };

                //Act and Assert
                var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.UpdateAsync(curvePoint.Id, curvePoint));
                Assert.Equal("CurvePoint not found", exception.Message);
            }


        }

        [Fact]
        public async Task UpdateAsync_UpdatesCurvePoint()
        {
            //Arrange
            using (var context = CreateContext("TestDatabase_UpdateAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var curvePoint = new CurvePointsDto(1, DateTime.Now, 1.0, 1.0, DateTime.Now);
                context.CurvePoints.Add(curvePoint);
                await context.SaveChangesAsync();

                var updatedCurvePoint = new CurvePoint
                {
                    Id = curvePoint.Id,
                    CurveId = 2,
                    AsOfDate = DateTime.Now,
                    Term = 2.0,
                    CurvePointValue = 2.0,
                    CreationDate = DateTime.Now
                };

                //Act
                await repository.UpdateAsync(updatedCurvePoint.Id, updatedCurvePoint);

                //Assert
                var result = await context.CurvePoints.FirstOrDefaultAsync();
                Assert.NotNull(result);
                Assert.Equal(updatedCurvePoint.CurveId, result.CurveId);
                Assert.Equal(updatedCurvePoint.Term, result.Term);
            }
        }

        [Fact]
        public async Task DeleteAsync_WhenCurvePointExists()
        {
            //Arrange
            using (var context = CreateContext("TestDatabase_DeleteAsync"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var curvePoint = new CurvePointsDto(1, DateTime.Now, 1.0, 1.0, DateTime.Now);
                context.CurvePoints.Add(curvePoint);
                await context.SaveChangesAsync();

                //Act
                await repository.DeleteAsync(curvePoint.Id);

                //Assert
                var result = await context.CurvePoints.FirstOrDefaultAsync();
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task DeleteAsync_WhenCurvePointDoesNotExist()
        {
            //Arrange
            using (var context = CreateContext("TestDatabase_DeleteAsync_NotFound"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                //Act
                await repository.DeleteAsync(1);

                //Assert
                var result = await context.CurvePoints.FirstOrDefaultAsync();
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task ExistsAsync_IsTrue()
        {
            //Arrange
            using (var context = CreateContext("TestDatabase_ExistsAsync_True"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                var curvePoint = new CurvePointsDto(1, DateTime.Now, 1.0, 1.0, DateTime.Now);
                context.CurvePoints.Add(curvePoint);
                await context.SaveChangesAsync();

                //Act
                var result = await repository.ExistsAsync(curvePoint.Id);

                //Assert
                Assert.True(result);
            }
        }

        [Fact]
        public async Task ExistsAsync_IsFalse()
        {
            //Arrange
            using (var context = CreateContext("TestDatabase_ExistsAsync_False"))
            {
                await CleanDatabase(context);
                var repository = CreateRepository(context);

                //Act
                var result = await repository.ExistsAsync(1);

                //Assert
                Assert.False(result);
            }
        }

        [Fact]
        public async Task ExistsAsync_ThrowsException_WhenExceptionOccurs()
        {
            //Arrange
            using (var context = CreateContext("TestDatabase_ExistsAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<CurvePointRepository>>();
                var repository = new CurvePointRepository(context, mockLogger.Object);

                // Create a mock DbSet and configure it to throw an exception when AnyAsync is called
                var mockSet = new Mock<DbSet<CurvePointsDto>>();
                mockSet.As<IQueryable<CurvePointsDto>>().Setup(m => m.Provider)
                    .Throws(new Exception("Mock exception"));
                context.CurvePoints = mockSet.Object;

                //Act and Assert
                var exception = await Assert.ThrowsAsync<ApplicationException>(() => repository.ExistsAsync(1));
                Assert.Equal("An error occurred while checking the existence of the CurvePoint with ID 1", exception.Message);
            }
        }


    }
}



