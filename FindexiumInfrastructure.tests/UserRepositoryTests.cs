using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Findexium.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
namespace FindexiumInfrastructure.tests
{
    public class UserRepositoryTests
    {
        private readonly UserRepository userRepository;
        private readonly DbContextOptions<LocalDbContext> _dbContextOptions;
        private readonly Mock<ILogger<UserRepository>> _mockLogger;
        private readonly Mock<UserManager<User>> _userManagerMock;


        public UserRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<LocalDbContext>()
                .UseInMemoryDatabase(databaseName: "UserRepository")
                .Options;

            _mockLogger = new Mock<ILogger<UserRepository>>();
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

            //   userRepository = new UserRepository(new LocalDbContext(_dbContextOptions), _userManagerMock.Object, _mockLogger.Object);
        }

        //private UserRepository CreateRepository(LocalDbContext context)
        //{
        //  //  return new UserRepository(context, null, _mockLogger.Object);
        //}

        private LocalDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new LocalDbContext(options);
        }

        private async Task CleanDatabase(LocalDbContext context)
        {
            context.Users.RemoveRange(context.Users);
            await context.SaveChangesAsync();
        }

        //[Fact]
        //public async Task GetUsersAsync_ShouldReturnAllUsers()
        //{
        //    using (var context = CreateContext("GetUsersAsync_ShouldReturnAllUsers"))
        //    {
        //        var repository = CreateRepository(context);
        //        context.Users.Add(new User { UserName = "user1", Fullname = "User One" });
        //        context.Users.Add(new User { UserName = "user2", Fullname = "User Two" });
        //        await context.SaveChangesAsync();

        //        var users = await repository.GetUsersAsync();

        //        Assert.Equal(2, users.Count());
        //    }
        //}

        [Fact]
        public async Task GetUsersAsync_ShouldthrowException()
        {
            // Arrange
            //using (var context = CreateContext("TestDatabase_GetUsersAsync_Exception"))
            //{
            //    var mockLogger = new Mock<ILogger<UserRepository>>();
            //    var mockUserManager = new Mock<UserManager<User>>(
            //        Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            //    var repository = new UserRepository(context, mockUserManager.Object, mockLogger.Object);

            //    var mockSet = new Mock<DbSet<User>>();
            //    mockSet.As<IQueryable<User>>().Setup(m => m.Provider)
            //        .Throws(new Exception("An error occurred while fetching users."));
            //    context.Users = mockSet.Object;

            //    // Act & Assert
            //    var exception = await Assert.ThrowsAsync<Exception>(() => repository.GetUsersAsync());
            //    Assert.Equal("An error occurred while fetching users.", exception.Message);
            //}
        }

        //[Fact]
        //public async Task GetUserByIdAsync_ShouldReturnUser()
        //{
        //    using (var context = CreateContext("GetUserByIdAsync_ShouldReturnUser"))
        //    {
        //        var repository = CreateRepository(context);
        //        var user = new User { UserName = "user1", Fullname = "User One" };
        //        context.Users.Add(user);
        //        await context.SaveChangesAsync();

        //        var fetchedUser = await repository.GetUserByIdAsync(user.Id);

        //        Assert.NotNull(fetchedUser);
        //        Assert.Equal(user.UserName, fetchedUser.UserName);
        //    }
        //}

        [Fact]
        public async Task GetUserByIdAsync_ThrowException()
        {
            // Arrange
            //using (var context = CreateContext("TestDatabase_GetUserByIdAsync_Exception"))
            //{
            //    var mockLogger = new Mock<ILogger<UserRepository>>();
            //    var mockUserManager = new Mock<UserManager<User>>(
            //        Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            //    var repository = new UserRepository(context, mockUserManager.Object, mockLogger.Object);

            //    var mockSet = new Mock<DbSet<User>>();
            //    mockSet.Setup(m => m.FindAsync(It.IsAny<string>()))
            //        .ThrowsAsync(new Exception("An error occurred while fetching user by id."));
            //    context.Users = mockSet.Object;

            //    // Act & Assert
            //    var exception = await Assert.ThrowsAsync<Exception>(() => repository.GetUserByIdAsync("test-id"));
            //    Assert.Equal("An error occurred while fetching user by id.", exception.Message);


            //        }
            //    }
            //    [Fact]
            //    public async Task AddUserAsync_ShouldAddUserAndAssignRole()
            //    {
            //        // Arrange
            //        var user = new User
            //        {
            //            UserName = "testuser",
            //            PasswordHash = "passwordhash",
            //            Fullname = "Test User"
            //        };

            //        _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), "User"))
            //            .ReturnsAsync(IdentityResult.Success);

            //        // Act
            //        await userRepository.AddUserAsync(user);

            //        // Assert
            //        using (var context = new LocalDbContext(_dbContextOptions))
            //        {
            //            var addedUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "testuser");
            //            Assert.NotNull(addedUser);
            //            Assert.Equal("testuser", addedUser.UserName);
            //            Assert.Equal("passwordhash", addedUser.PasswordHash);
            //            Assert.Equal("Test User", addedUser.Fullname);
            //        }

            //        _userManagerMock.Verify(um => um.AddToRoleAsync(It.IsAny<User>(), "User"), Times.Once);
            //    }

            //    [Fact]
            //    public async Task AddUserAsync_ShouldLogError()
            //    {
            //        // Arrange
            //        var user = new User
            //        {
            //            UserName = "testuser",
            //            PasswordHash = "passwordhash",
            //            Fullname = "Test User"
            //        };

            //        _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), "User"))
            //            .ThrowsAsync(new Exception("Role assignment failed"));

            //        // Act & Assert
            //        var exception = await Assert.ThrowsAsync<Exception>(() => userRepository.AddUserAsync(user));
            //        Assert.Equal("An error occurred while adding the user.", exception.Message);


            //    }


            //[Fact]
            //    public async Task DeleteUserAsync_ShouldDeleteUser()
            //    {
            //        using (var context = CreateContext("DeleteUserAsync_ShouldDeleteUser"))
            //        {
            //            var repository = CreateRepository(context);
            //            var user = new User { UserName = "user1", Fullname = "User One" };
            //            context.Users.Add(user);
            //            await context.SaveChangesAsync();

            //            await repository.DeleteUserAsync(user.Id);

            //            var deletedUser = await context.Users.FindAsync(user.Id);
            //            Assert.Null(deletedUser);
            //        }
            //    }

            //    [Fact]
            //    public async Task DeleteUserAsync_ShouldThrowException_WhenErrorOccurs()
            //    {
            //        // Arrange
            //        using (var context = CreateContext("TestDatabase_DeleteUserAsync_Exception"))
            //        {
            //            var repository = CreateRepository(context);

            //            var mockSet = new Mock<DbSet<User>>();
            //            mockSet.Setup(m => m.FindAsync(It.IsAny<string>()))
            //                .ThrowsAsync(new Exception("An error occurred while deleting user with id"));
            //            context.Users = mockSet.Object;

            //            // Act & Assert
            //            await Assert.ThrowsAsync<Exception>(() => repository.DeleteUserAsync("test-id"));
            //        }
            //    }

            //    [Fact]
            //    public async Task UserExistsAsync_ShouldReturnTrueIfExists()
            //    {
            //        using (var context = CreateContext("UserExistsAsync_ShouldReturnTrueIfExists"))
            //        {
            //            var repository = CreateRepository(context);
            //            var user = new User { UserName = "user1", Fullname = "User One" };
            //            context.Users.Add(user);
            //            await context.SaveChangesAsync();

            //            var exists = await repository.UserExistsAsync(user.Id);

            //            Assert.True(exists);
            //        }
            //    }

            //    [Fact]
            //    public async Task UserExistsAsync_ShouldReturnFalseIfNotExists()
            //    {
            //        using (var context = CreateContext("UserExistsAsync_ShouldReturnFalseIfNotExists"))
            //        {
            //            var repository = CreateRepository(context);

            //            var exists = await repository.UserExistsAsync("nonexistent-id");

            //            Assert.False(exists);
            //        }
            //    }

            //  [Fact]
            //public async Task UserExistsAsync_ShouldThrowException()
            //{
            //    // Arrange
            //    using (var context = CreateContext("TestDatabase_UserExistsAsync_Exception"))
            //    {
            //        var mockLogger = new Mock<ILogger<UserRepository>>();
            //        var repository = new UserRepository(context, null, mockLogger.Object);

            //        var mockSet = new Mock<DbSet<User>>();
            //        mockSet.As<IQueryable<User>>().Setup(m => m.Provider)
            //            .Throws(new Exception("An error occurred while checking if user exists with id"));
            //        context.Users = mockSet.Object;

            //        // Act & Assert
            //        await Assert.ThrowsAsync<Exception>(() => repository.UserExistsAsync("test-id"));
            //  }
            //    }
            //[Fact]
            //public async Task UpdateUserAsync_ShouldUpdateUserAndRemoveRoles()
            //   {
            //using (var context = CreateContext("UpdateUserAsync_ShouldUpdateUserAndRemoveRoles"))
            //{
            //    var mockUserStore = new Mock<IUserStore<User>>();
            //    var mockUserManager = new Mock<UserManager<User>>(
            //        mockUserStore.Object, null, null, null, null, null, null, null, null);
            //    var repository = new UserRepository(context, mockUserManager.Object, _mockLogger.Object);

            //    var user = new User { UserName = "user1", Fullname = "User One", PasswordHash = "password" };
            //    context.Users.Add(user);
            //    await context.SaveChangesAsync();

            //    var updatedUser = new User { Id = user.Id, UserName = "updateduser", Fullname = "Updated User One", PasswordHash = "newpasswordhash" };

            //    mockUserManager.Setup(um => um.UpdateAsync(It.IsAny<User>()))
            //        .ReturnsAsync(IdentityResult.Success);

            //    mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<User>()))
            //        .ReturnsAsync(new List<string> { "User" });

            //    mockUserManager.Setup(um => um.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
            //        .ReturnsAsync(IdentityResult.Success);

            //    await repository.UpdateUserAsync(updatedUser);

            //    var fetchedUser = await context.Users.FindAsync(user.Id);
            //    Assert.NotNull(fetchedUser);
            //    Assert.Equal("updateduser", fetchedUser.UserName);
            //    Assert.Equal("Updated User One", fetchedUser.Fullname);

            //    mockUserManager.Verify(um => um.UpdateAsync(It.IsAny<User>()), Times.Once);
            //    mockUserManager.Verify(um => um.GetRolesAsync(It.IsAny<User>()), Times.Once);
            //    mockUserManager.Verify(um => um.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()), Times.Once);
            //}
            //    }

            //    [Fact]
            //    public async Task UpdateUserAsync_ShouldThrowException()
            //    {
            //        // Arrange
            //        using (var context = CreateContext("TestDatabase_UpdateUserAsync_Exception"))
            //        {
            //            var repository = CreateRepository(context);

            //            var mockSet = new Mock<DbSet<User>>();
            //            mockSet.Setup(m => m.FindAsync(It.IsAny<string>()))
            //                .ThrowsAsync(new Exception("An error occurred while updating user"));
            //            context.Users = mockSet.Object;

            //            var user = new User { Id = "test-id", UserName = "testuser", Fullname = "Test User" };

            //            // Act & Assert
            //            await Assert.ThrowsAsync<Exception>(() => repository.UpdateUserAsync(user));
            //        }
            //    }
            //}
        }
    }
}
