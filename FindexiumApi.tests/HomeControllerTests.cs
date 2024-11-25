using Findexium.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FindexiumApi.tests
{
    public class HomeControllerTests
    {
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _controller = new HomeController();
        }

        [Fact]
        public void Get_ReturnsOkResult()
        {
            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Admin_ReturnsOkResult()
        {
            // Act
            var result = _controller.Admin();

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}