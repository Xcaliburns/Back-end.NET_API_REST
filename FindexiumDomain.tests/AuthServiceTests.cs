using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Domain.Services;
using Moq;


namespace FindexiumDomain.tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IAuthRepository> _mockAuthRepository;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockAuthRepository = new Mock<IAuthRepository>();
            _authService = new AuthService(_mockAuthRepository.Object);
        }

        [Fact]
        public async Task ValidateUserAsync_ValidUser_ReturnsUser()
        {
            // Arrange
            var userName = "testuser";
            var password = "password";
            var user = new User { UserName = userName };

            _mockAuthRepository.Setup(r => r.FindByNameAsync(userName)).ReturnsAsync(user);
            _mockAuthRepository.Setup(r => r.CheckPasswordAsync(user, password)).ReturnsAsync(true);

            // Act
            var result = await _authService.ValidateUserAsync(userName, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userName, result.UserName);
        }

        [Fact]
        public async Task ValidateUserAsync_InvalidUser_ReturnsNull()
        {
            // Arrange
            var userName = "invaliduser";
            var password = "password";

            _mockAuthRepository.Setup(r => r.FindByNameAsync(userName)).ReturnsAsync((User)null);

            // Act
            var result = await _authService.ValidateUserAsync(userName, password);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserRolesAsync_ValidUser_ReturnsRoles()
        {
            // Arrange
            var user = new User { UserName = "testuser" };
            var roles = new List<string> { "User" };

            _mockAuthRepository.Setup(r => r.GetRolesAsync(user)).ReturnsAsync(roles);

            // Act
            var result = await _authService.GetUserRolesAsync(user);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("User", result[0]);
        }
    }
}
