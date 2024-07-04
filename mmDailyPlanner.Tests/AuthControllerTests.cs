using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using mmDailyPlanner.Server.Services.AuthService;
using mmDailyPlanner.Server.Controllers;
using mmDailyPlanner.Server.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using mmDailyPlanner.Server.Models;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly Mock<ILogger<AuthController>> _mockLogger;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _mockLogger = new Mock<ILogger<AuthController>>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _controller = new AuthController(_mockAuthService.Object, _mockLogger.Object, _mockHttpContextAccessor.Object);
    }

    [Fact]
    public async Task RegisterAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("error", "some error");

        // Act
        var result = await _controller.RegisterAsync(new UserDTO());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task LoginAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("error", "some error");

        // Act
        var result = await _controller.LoginAsync(new LoginModel());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task LoginAsync_ReturnsUnauthorized_WhenLoginFails()
    {
        // Arrange
        _mockAuthService.Setup(x => x.LoginAsync(It.IsAny<LoginModel>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<HttpResponse>()))
            .ReturnsAsync((false, "Invalid username or password."));

        // Act
        var result = await _controller.LoginAsync(new LoginModel());

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);
    }


}
