using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FindexiumDomain.tests
{
    public class UserServicesTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly UserService _userService;

        public UserServicesTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _loggerMock = new Mock<ILogger<UserService>>();
            _userService = new UserService(_userRepositoryMock.Object, _userManagerMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetUsersAsync_ReturnsUsers()
        {
            // Arrange
            var users = new List<User> { new User { UserName = "user1" }, new User { UserName = "user2" } };
            _userRepositoryMock.Setup(repo => repo.GetUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetUsersAsync();

            // Assert
            Assert.Equal(users, result);
        }

        [Fact]
        public async Task GetUsersAsync_ThrowsException()
        {
            // Arrange
            var exception = new Exception("Test exception");
            _userRepositoryMock.Setup(repo => repo.GetUsersAsync()).ThrowsAsync(exception);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _userService.GetUsersAsync());
            Assert.Equal(exception, ex);
           
        }
        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser()
        {
            // Arrange
            var userId = "1";
            var user = new User { Id = userId, UserName = "user1" };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task GetUserByIdAsync_ThrowsException()
        {
            // Arrange
            var userId = "1";
            var exception = new Exception("Test exception");
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ThrowsAsync(exception);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _userService.GetUserByIdAsync(userId));
            Assert.Equal(exception, ex);
           
        }
        [Fact]
        public async Task AddUserAsync_ReturnsSuccess_WhenUserIsAdded()
        {
            // Arrange
            var user = new User { UserName = "newuser" };
            var password = "password";
            _userManagerMock.Setup(um => um.FindByNameAsync(user.UserName)).ReturnsAsync((User)null);
            _userManagerMock.Setup(um => um.CreateAsync(user, password)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.AddUserAsync(user, password);

            // Assert
            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task AddUserAsync_ReturnsFailure_WhenUserAlreadyExists()
        {
            // Arrange
            var user = new User { UserName = "existinguser" };
            var password = "password";
            _userManagerMock.Setup(um => um.FindByNameAsync(user.UserName)).ReturnsAsync(user);

            // Act
            var result = await _userService.AddUserAsync(user, password);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Username already exists.", result.Errors.First().Description);
        }
        [Fact]
        public async Task UpdateUserAsync_ReturnsSuccess_WhenUserIsUpdated()
        {
            // Arrange
            var user = new User { UserName = "updateduser" };
            _userManagerMock.Setup(um => um.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.UpdateUserAsync(user);

            // Assert
            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task UpdateUserAsync_ThrowsException()
        {
            // Arrange
            var user = new User { UserName = "updateduser" };
            var exception = new Exception("Test exception");
            _userManagerMock.Setup(um => um.UpdateAsync(user)).ThrowsAsync(exception);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _userService.UpdateUserAsync(user));
            Assert.Equal(exception, ex);
          
        }
        [Fact]
        public async Task DeleteUserAsync_ReturnsSuccess_WhenUserIsDeleted()
        {
            // Arrange
            var userId = "1";
            var user = new User { Id = userId, UserName = "user1" };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.DeleteUserAsync(userId);

            // Assert
            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task DeleteUserAsync_ReturnsFailure_WhenUserNotFound()
        {
            // Arrange
            var userId = "1";
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.DeleteUserAsync(userId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("User not found", result.Errors.First().Description);
        }

        [Fact]
        public async Task DeleteUserAsync_ThrowsException()
        {
            // Arrange
            var userId = "1";
            var exception = new Exception("Test exception");
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ThrowsAsync(exception);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _userService.DeleteUserAsync(userId));
            Assert.Equal(exception, ex);
          
        }

        [Fact]
        public async Task UserExistsAsync_ReturnsTrue_WhenUserExists()
        {
            // Arrange
            var userId = "1";
            var user = new User { Id = userId };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.UserExistsAsync(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UserExistsAsync_ReturnsFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "1";
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.UserExistsAsync(userId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UserExistsAsync_ThrowsException()
        {
            // Arrange
            var userId = "1";
            var exception = new Exception("Test exception");
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ThrowsAsync(exception);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _userService.UserExistsAsync(userId));
            Assert.Equal(exception, ex);
         
        }

        [Fact]
        public async Task ValidateCredentialsAsync_ReturnsUser_WhenCredentialsAreValid()
        {
            // Arrange
            var login = "user1";
            var password = "password";
            var user = new User { UserName = login };
            _userManagerMock.Setup(um => um.FindByNameAsync(login)).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.CheckPasswordAsync(user, password)).ReturnsAsync(true);

            // Act
            var result = await _userService.ValidateCredentialsAsync(login, password);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task ValidateCredentialsAsync_ReturnsNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            var login = "user1";
            var password = "password";
            var user = new User { UserName = login };
            _userManagerMock.Setup(um => um.FindByNameAsync(login)).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.CheckPasswordAsync(user, password)).ReturnsAsync(false);

            // Act
            var result = await _userService.ValidateCredentialsAsync(login, password);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ValidateCredentialsAsync_ThrowsException()
        {
            // Arrange
            var login = "user1";
            var password = "password";
            var exception = new Exception("Test exception");
            _userManagerMock.Setup(um => um.FindByNameAsync(login)).ThrowsAsync(exception);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _userService.ValidateCredentialsAsync(login, password));
            Assert.Equal(exception, ex);
           
        }
        [Fact]
        public async Task GetUserRolesAsync_ReturnsRoles()
        {
            // Arrange
            var user = new User { UserName = "user1" };
            var roles = new List<string> { "Admin", "User" };
            _userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(roles);

            // Act
            var result = await _userService.GetUserRolesAsync(user);

            // Assert
            Assert.Equal(roles, result);
        }

        [Fact]
        public async Task GetUserRolesAsync_ThrowsException()
        {
            // Arrange
            var user = new User { UserName = "user1" };
            var exception = new Exception("Test exception");
            _userManagerMock.Setup(um => um.GetRolesAsync(user)).ThrowsAsync(exception);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _userService.GetUserRolesAsync(user));
            Assert.Equal(exception, ex);
        }
    }
    
}
