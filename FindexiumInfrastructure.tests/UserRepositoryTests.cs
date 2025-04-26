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
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;


        public UserRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<LocalDbContext>()
                .UseInMemoryDatabase(databaseName: "UserRepository")
                .Options;

            _mockLogger = new Mock<ILogger<UserRepository>>();
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);

            userRepository = new UserRepository(new LocalDbContext(_dbContextOptions), _userManagerMock.Object, _roleManagerMock.Object, _mockLogger.Object);
        }

       

        private LocalDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new LocalDbContext(options);
        }

        private UserRepository CreateRepository(LocalDbContext context)
        {
            return new UserRepository(context, _userManagerMock.Object, _roleManagerMock.Object, _mockLogger.Object);
        }

        private async Task CleanDatabase(LocalDbContext context)
        {
            context.Users.RemoveRange(context.Users);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnAllUsers()
        {
            using (var context = CreateContext("GetUsersAsync_ShouldReturnAllUsers"))
            {
                var repository = CreateRepository(context);
                context.Users.Add(new User { UserName = "user1", Fullname = "User One" });
                context.Users.Add(new User { UserName = "user2", Fullname = "User Two" });
                await context.SaveChangesAsync();

                var users = await repository.GetUsersAsync();

                Assert.Equal(2, users.Count());
            }
        }

        [Fact]
        public async Task GetUsersAsync_ShouldthrowException()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_GetUsersAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<UserRepository>>();
                var mockUserStore = new Mock<IUserStore<User>>();
                var mockUserManager = new Mock<UserManager<User>>(
                    mockUserStore.Object,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                );
                var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
                var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                    mockRoleStore.Object,
                    null,
                    null,
                    null,
                    null
                );
                var repository = new UserRepository(context, mockUserManager.Object, mockRoleManager.Object, mockLogger.Object);

                var mockSet = new Mock<DbSet<User>>();
                mockSet.As<IQueryable<User>>().Setup(m => m.Provider)
                    .Throws(new Exception("An error occurred while fetching users."));
                context.Users = mockSet.Object;

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.GetUsersAsync());
                Assert.Equal("An error occurred while fetching users.", exception.Message);
            }
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser()
        {
            using (var context = CreateContext("GetUserByIdAsync_ShouldReturnUser"))
            {
                var repository = CreateRepository(context);
                var user = new User { UserName = "user1", Fullname = "User One" };
                context.Users.Add(user);
                await context.SaveChangesAsync();

                var fetchedUser = await repository.GetUserByIdAsync(user.Id);

                Assert.NotNull(fetchedUser);
                Assert.Equal(user.UserName, fetchedUser.UserName);
            }
        }

        [Fact]
        public async Task GetUserByIdAsync_ThrowException()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_GetUserByIdAsync_Exception"))
            {
                var mockLogger = new Mock<ILogger<UserRepository>>();
                var mockUserStore = new Mock<IUserStore<User>>();
                var mockUserManager = new Mock<UserManager<User>>(
                    mockUserStore.Object,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                );
                var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
                var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                    mockRoleStore.Object,
                    null,
                    null,
                    null,
                    null
                );
                var repository = new UserRepository(context, mockUserManager.Object, mockRoleManager.Object, mockLogger.Object);

                var mockSet = new Mock<DbSet<User>>();
                mockSet.Setup(m => m.FindAsync(It.IsAny<string>()))
                    .ThrowsAsync(new Exception("An error occurred while fetching user by id."));
                context.Users = mockSet.Object;

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => repository.GetUserByIdAsync("test-id"));
                Assert.Equal("An error occurred while fetching user by id.", exception.Message);
            }
        }
        [Fact]
        public async Task AddUserAsync_ShouldAddUserAndAssignRole()
        {
            // Arrange
            var user = new User
            {
                UserName = "testuser",
                PasswordHash = "passwordhash",
                Fullname = "Test User"
            };

            var mockLogger = new Mock<ILogger<UserRepository>>();
            var mockUserStore = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(
                mockUserStore.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            );
            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                mockRoleStore.Object,
                null,
                null,
                null,
                null
            );

            mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            mockRoleManager.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), "User"))
                .ReturnsAsync(IdentityResult.Success);

            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_AddUserAsync")
                .Options;

            using (var context = new LocalDbContext(options))
            {
                var repository = new UserRepository(context, mockUserManager.Object, mockRoleManager.Object, mockLogger.Object);

                // Act
                await repository.AddUserAsync(user);

                // Assert
                var addedUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "testuser");
                Assert.NotNull(addedUser);
                Assert.Equal("testuser", addedUser.UserName);
                Assert.Equal("passwordhash", addedUser.PasswordHash);
                Assert.Equal("Test User", addedUser.Fullname);
            }

            mockUserManager.Verify(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(um => um.AddToRoleAsync(It.IsAny<User>(), "User"), Times.Once);
        }

        [Fact]
        public async Task AddUserAsync_ShouldLogError()
        {
            // Arrange
            var user = new User
            {
                UserName = "testuser",
                PasswordHash = "passwordhash",
                Fullname = "Test User"
            };

            _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), "User"))
                .ThrowsAsync(new Exception("Role assignment failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => userRepository.AddUserAsync(user));
            Assert.Equal("An error occurred while adding the user.", exception.Message);


        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUser()
        {
            // Arrange
            using (var context = CreateContext("UpdateUserAsync_ShouldUpdateUser"))
            {
                var mockUserStore = new Mock<IUserStore<User>>();
                var mockPasswordHasher = new Mock<IPasswordHasher<User>>();
                var mockUserManager = new Mock<UserManager<User>>(
                    mockUserStore.Object, null, mockPasswordHasher.Object, null, null, null, null, null, null);

                var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
                var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                    mockRoleStore.Object, null, null, null, null);

                var mockLogger = new Mock<ILogger<UserRepository>>();
                var repository = new UserRepository(context, mockUserManager.Object, mockRoleManager.Object, mockLogger.Object);

                var user = new User { Id = "1", UserName = "user1", Fullname = "User One", PasswordHash = "password" };
                context.Users.Add(user);
                await context.SaveChangesAsync();

                var updatedUser = new User { Id = user.Id, UserName = "updateduser", Fullname = "Updated User One", PasswordHash = "newpasswordhash" };

                mockUserManager.Setup(um => um.FindByIdAsync(user.Id))
                    .ReturnsAsync(user);

                mockUserManager.Setup(um => um.UpdateAsync(It.IsAny<User>()))
                    .ReturnsAsync(IdentityResult.Success);

                mockPasswordHasher.Setup(ph => ph.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
                    .Returns("hashedpassword");

                // Act
                await repository.UpdateUserAsync(updatedUser);

                // Assert
                var fetchedUser = await context.Users.FindAsync(user.Id);
                Assert.NotNull(fetchedUser);
                Assert.Equal("updateduser", fetchedUser.UserName);
                Assert.Equal("Updated User One", fetchedUser.Fullname);
                Assert.Equal("hashedpassword", fetchedUser.PasswordHash);

                mockUserManager.Verify(um => um.FindByIdAsync(user.Id), Times.Once);
                mockUserManager.Verify(um => um.UpdateAsync(It.IsAny<User>()), Times.Once);
                mockPasswordHasher.Verify(ph => ph.HashPassword(It.IsAny<User>(), "newpasswordhash"), Times.Once);
            }
        }


        [Fact]
        public async Task DeleteUserAsync_ShouldDeleteUser()
        {
            using (var context = CreateContext("DeleteUserAsync_ShouldDeleteUser"))
            {
                // Arrange
                var user = new User { Id = "1", UserName = "user1", Fullname = "User One" };
                context.Users.Add(user);
                await context.SaveChangesAsync();

                var mockUserManager = new Mock<UserManager<User>>(
                    Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
                mockUserManager.Setup(um => um.FindByIdAsync(user.Id))
                    .ReturnsAsync(user);
                mockUserManager.Setup(um => um.DeleteAsync(user))
                    .ReturnsAsync(IdentityResult.Success);

                var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
                var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                    mockRoleStore.Object, null, null, null, null, null, null, null, null);

                var repository = new UserRepository(context, mockUserManager.Object, mockRoleManager.Object, Mock.Of<ILogger<UserRepository>>());

                // Act
                await repository.DeleteUserAsync(user.Id);

                // Assert
                var deletedUser = await context.Users.FindAsync(user.Id);
                Assert.Null(deletedUser);
            }
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldThrowException_WhenErrorOccurs()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_DeleteUserAsync_Exception"))
            {
                var repository = CreateRepository(context);

                var mockSet = new Mock<DbSet<User>>();
                mockSet.Setup(m => m.FindAsync(It.IsAny<string>()))
                    .ThrowsAsync(new Exception("An error occurred while deleting user with id"));
                context.Users = mockSet.Object;

                // Act & Assert
                await Assert.ThrowsAsync<Exception>(() => repository.DeleteUserAsync("test-id"));
            }
        }

        [Fact]
        public async Task UserExistsAsync_ShouldReturnTrueIfExists()
        {
            using (var context = CreateContext("UserExistsAsync_ShouldReturnTrueIfExists"))
            {
                var user = new User { Id = "1", UserName = "user1", Fullname = "User One" };
                context.Users.Add(user);
                await context.SaveChangesAsync();

                var mockUserManager = new Mock<UserManager<User>>(
                    Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
                mockUserManager.Setup(um => um.FindByIdAsync(user.Id))
                    .ReturnsAsync(user);

                var repository = new UserRepository(context, mockUserManager.Object, _roleManagerMock.Object, _mockLogger.Object);

                var exists = await repository.UserExistsAsync(user.Id);

                Assert.True(exists);
            }
        }

        [Fact]
        public async Task UserExistsAsync_ShouldReturnFalseIfNotExists()
        {
            using (var context = CreateContext("UserExistsAsync_ShouldReturnFalseIfNotExists"))
            {
                var repository = CreateRepository(context);

                var exists = await repository.UserExistsAsync("nonexistent-id");

                Assert.False(exists);
            }
        }

     
     

        [Fact]
        public async Task UpdateUserAsync_ShouldThrowException()
        {
            // Arrange
            using (var context = CreateContext("TestDatabase_UpdateUserAsync_Exception"))
            {
                var repository = CreateRepository(context);

                var mockSet = new Mock<DbSet<User>>();
                mockSet.Setup(m => m.FindAsync(It.IsAny<string>()))
                    .ThrowsAsync(new Exception("An error occurred while updating user"));
                context.Users = mockSet.Object;

                var user = new User { Id = "test-id", UserName = "testuser", Fullname = "Test User" };

                // Act & Assert
                await Assert.ThrowsAsync<Exception>(() => repository.UpdateUserAsync(user));
            }
        }
    }
}
    
