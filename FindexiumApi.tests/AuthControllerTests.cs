using Findexium.Api.Controllers;
using Findexium.Api.Models;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace FindexiumApi.tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<ILogger<AuthController>> _mockLogger;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockAuthService = new Mock<IAuthService>();
            _mockLogger = new Mock<ILogger<AuthController>>();
            _authController = new AuthController(_mockConfiguration.Object, _mockAuthService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Login_ValidUser_ReturnsOkResultWithToken()
        {
            // Arrange
            var loginRequest = new LoginRequest { Login = "testuser", Password = "password" };
            var user = new User { UserName = "testuser" };
            var roles = new List<string> { "User" };

            _mockAuthService.Setup(s => s.ValidateUserAsync(loginRequest.Login, loginRequest.Password)).ReturnsAsync(user);
            _mockAuthService.Setup(s => s.GetUserRolesAsync(user)).ReturnsAsync(roles);
            _mockConfiguration.Setup(c => c["Jwt:SecretKey"]).Returns("LeSecretDeTotoEtTitiQuiFontDuSki");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("Findexium");
            _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("Findexium");

            // Act
            var result = await _authController.Login(loginRequest) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            Assert.NotNull(result.Value);
            var tokenProperty = result.Value.GetType().GetProperty("Token");
            Assert.NotNull(tokenProperty);

            var tokenValue = tokenProperty.GetValue(result.Value) as string;
            Assert.NotNull(tokenValue);
            Assert.StartsWith("Bearer ", tokenValue);
        }

        [Fact]
        public async Task Login_InvalidUser_ReturnsUnauthorized()
        {
            // Arrange
            var loginRequest = new LoginRequest { Login = "invaliduser", Password = "password" };

            _mockAuthService.Setup(s => s.ValidateUserAsync(loginRequest.Login, loginRequest.Password)).ReturnsAsync((User?)null);

            // Act
            var result = await _authController.Login(loginRequest);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Login_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var loginRequest = new LoginRequest { Login = "testuser", Password = "password" };

            _mockAuthService.Setup(s => s.ValidateUserAsync(loginRequest.Login, loginRequest.Password)).ThrowsAsync(new System.Exception("Test exception"));

            // Act
            var result = await _authController.Login(loginRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal("Internal server error", result.Value);
        }
    }
}
