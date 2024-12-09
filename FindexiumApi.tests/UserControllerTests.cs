using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Findexium.Api.Controllers;
using Findexium.Api.Models;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FindexiumApi.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userService;
        private readonly Mock<ILogger<UsersController>> _logger;
        private readonly UsersController _controller;

        public UserControllerTests()
        {
            _userService = new Mock<IUserService>();
            _logger = new Mock<ILogger<UsersController>>();
            _controller = new UsersController(_userService.Object, _logger.Object);
        }

        [Fact]
        public async Task GetUsers_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", UserName = "User1", Fullname = "Full Name 1",  },
                new User { Id = "2", UserName = "User2", Fullname = "Full Name 2",  }
            };
            _userService.Setup(x => x.GetUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnUsers = Assert.IsType<List<UserResponse>>(okResult.Value);
            Assert.Equal(2, returnUsers.Count);
        }


        [Fact]
        public async Task GetUsers_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _userService.Setup(x => x.GetUsersAsync()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Internal server error", statusCodeResult.Value);
          
        }

        [Fact]
        public async Task GetUser_ReturnsOkResult_WhenUserExists()
        {
            // Arrange
            var userId = "1";
            var user = new User { Id = userId, UserName = "User1", Fullname = "Full Name 1" };
            _userService.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnUser = Assert.IsType<User>(okResult.Value);
            Assert.Equal(userId, returnUser.Id);
        }

        [Fact]
        public async Task GetUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "1";
            _userService.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.GetUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
public async Task GetUser_ReturnsInternalServerError_WhenExceptionIsThrown()
{
    // Arrange
    var userId = "1";
    _userService.Setup(x => x.GetUserByIdAsync(userId)).ThrowsAsync(new Exception("Test exception"));

    // Act
    var result = await _controller.GetUser(userId);

    // Assert
    var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
    Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
    Assert.Equal("Internal server error", statusCodeResult.Value);
}


        [Fact]
        public async Task PutUser_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var userId = "1";
            var user = new User { Id = userId, UserName = "UpdatedUser", Email = "updated@example.com" };
            _userService.Setup(x => x.UpdateUserAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.PutUser(userId, user);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _userService.Verify(x => x.UpdateUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task PutUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "1";
            var user = new User { Id = userId, UserName = "UpdatedUser", Email = "updated@example.com" };
            _userService.Setup(x => x.UpdateUserAsync(user)).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User not found" }));

            // Act
            var result = await _controller.PutUser(userId, user);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutUser_ReturnsBadRequest_WhenIdDoesNotMatchUserId()
        {
            // Arrange
            var userId = "1";
            var user = new User { Id = "2", UserName = "UpdatedUser", Email = "updated@example.com" };

            // Act
            var result = await _controller.PutUser(userId, user);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutUser_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var userId = "1";
            var user = new User { Id = userId, UserName = "UpdatedUser", Email = "updated@example.com" };
            _userService.Setup(x => x.UpdateUserAsync(user)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.PutUser(userId, user);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Internal server error", statusCodeResult.Value);
        }


        [Fact]
        public async Task PostUser_ReturnsCreatedAtAction_WhenUserIsCreated()
        {
            // Arrange
            var userRequest = new UserRequest { UserName = "NewUser", Password = "Password123", FullName = "New User" };
            var user = new User { Id = "1", UserName = "NewUser", Fullname = "New User" };
            _userService.Setup(x => x.AddUserAsync(It.IsAny<User>(), userRequest.Password)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.PostUser(userRequest);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnUser = Assert.IsType<UserResponse>(createdAtActionResult.Value);
            Assert.Equal(userRequest.UserName, returnUser.UserName);
        }

        [Fact]
        public async Task PostUser_ReturnsBadRequest_WhenRequestIsNull()
        {
            // Arrange
            UserRequest request = null;

            // Act
            var result = await _controller.PostUser(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid user data.", badRequestResult.Value);
        }

        [Fact]
        public async Task PostUser_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var userRequest = new UserRequest { UserName = "NewUser", Password = "Password123", FullName = "New User" };
            _userService.Setup(x => x.AddUserAsync(It.IsAny<User>(), userRequest.Password)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.PostUser(userRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Internal server error", statusCodeResult.Value);
        }


        [Fact]
        public async Task DeleteUser_ReturnsNoContent_WhenUserIsDeleted()
        {
            // Arrange
            var userId = "1";
            var user = new User { Id = userId, UserName = "User1", Fullname = "Full Name 1" };
            _userService.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _userService.Setup(x => x.DeleteUserAsync(userId)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _userService.Verify(x => x.DeleteUserAsync(userId), Times.Once);
        }

        [Fact]
        public async Task PostUser_ReturnsBadRequest_WhenAddUserFails()
        {
            // Arrange
            var userRequest = new UserRequest { UserName = "NewUser", Password = "Password123", FullName = "New User" };
            var identityResult = IdentityResult.Failed(new IdentityError { Description = "User creation failed" });
            _userService.Setup(x => x.AddUserAsync(It.IsAny<User>(), userRequest.Password)).ReturnsAsync(identityResult);

            // Act
            var result = await _controller.PostUser(userRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(identityResult.Errors, badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "1";
            _userService.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task DeleteUser_ReturnsNotFound_WhenDeleteFails()
        {
            // Arrange
            var userId = "1";
            var user = new User { Id = userId, UserName = "User1", Fullname = "Full Name 1" };
            _userService.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _userService.Setup(x => x.DeleteUserAsync(userId)).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Deletion failed" }));

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var userId = "1";
            _userService.Setup(x => x.GetUserByIdAsync(userId)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Internal server error", statusCodeResult.Value);
        }


    }
}
