using Microsoft.AspNetCore.Mvc;
using mmDailyPlanner.Server.DTO;
using mmDailyPlanner.Server.Services.AuthService;
using mmDailyPlanner.Server.Models;
using mmDailyPlanner.Server.Constants;

namespace mmDailyPlanner.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private const string SessionTokenCookieName = "sessionToken";

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("isAuthenticated")]
        public async Task<IActionResult> IsAuthenticated()
        {
            return await HandleRequestAsync(async () =>
            {
                var sessionToken = GetSessionToken();
                if (string.IsNullOrEmpty(sessionToken))
                    return Unauthorized(new { message = ErrorMessages.SessionTokenMissing });

                var isAuthenticated = await _authService.IsAuthenticatedAsync(sessionToken);
                return Ok(new { isAuthenticated });
            }, AuthMessages.CheckingAuthentication);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromForm] UserDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await HandleRequestAsync(async () =>
            {
                var ipAddress = GetIpAddress();
                var result = await _authService.RegisterAsync(userDto, ipAddress);

                if (result.success)
                {
                    _logger.LogInformation(AuthMessages.UserRegistered);
                    return Ok(new { message = result.message });
                }

                _logger.LogWarning(ErrorMessages.UserRegistrationFailed, result.message);
                return BadRequest(new { message = result.message });
            }, AuthMessages.Registration);
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await HandleRequestAsync(async () =>
            {
                var ipAddress = GetIpAddress();
                var sessionToken = GetSessionToken();
                var result = await _authService.LoginAsync(loginModel, ipAddress, sessionToken, Response);

                if (result.success)
                {
                    _logger.LogInformation(AuthMessages.UserLoggedIn);
                    return Ok(new { message = result.message });
                }

                _logger.LogWarning(ErrorMessages.UserLoginFailed, result.message);
                return Unauthorized(new { message = result.message });
            }, AuthMessages.Login);
        }

        [HttpGet("get-user")]
        public async Task<IActionResult> GetUserAsync()
        {
            return await HandleRequestAsync(async () =>
            {
                var sessionToken = GetSessionToken();
                if (string.IsNullOrEmpty(sessionToken))
                    return Unauthorized(new { message = ErrorMessages.SessionTokenMissing });

                var user = await _authService.GetUserAsync(sessionToken);
                if (user != null)
                {
                    _logger.LogInformation(AuthMessages.UserRetrieved);
                    return Ok(user);
                }

                _logger.LogWarning(ErrorMessages.UserNotFound);
                return NotFound(new { message = ErrorMessages.UserNotFound });
            }, AuthMessages.RetrievingUser);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            return await HandleRequestAsync(async () =>
            {
                var sessionToken = GetSessionToken();
                if (string.IsNullOrEmpty(sessionToken))
                    return Unauthorized(new { message = ErrorMessages.SessionTokenMissing });

                await _authService.LogoutAsync(sessionToken, Response);
                _logger.LogInformation(AuthMessages.UserLoggedOut);
                return Ok(new { message = AuthMessages.LogoutSuccess });
            }, AuthMessages.Logout);
        }

        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUserId()
        {
            return await HandleRequestAsync(async () =>
            {
                var sessionToken = GetSessionToken();
                if (string.IsNullOrEmpty(sessionToken))
                    return Unauthorized(new { message = ErrorMessages.SessionTokenMissing });

                var userId = await _authService.GetCurrentUserIdAsync(sessionToken);
                if (userId > 0)
                {
                    _logger.LogInformation(AuthMessages.UserIdRetrieved);
                    return Ok(new { userId });
                }

                _logger.LogWarning(ErrorMessages.NoValidSession);
                return Unauthorized(new { message = ErrorMessages.NoValidSession });
            }, AuthMessages.RetrievingUserId);
        }

        private string GetSessionToken() => _httpContextAccessor.HttpContext?.Request.Cookies[SessionTokenCookieName];

        private string GetIpAddress() => _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        private async Task<IActionResult> HandleRequestAsync(Func<Task<IActionResult>> func, string actionDescription)
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(ErrorMessages.ErrorOccuredWhileActionDescription, actionDescription));
                return StatusCode(500, new { message = string.Format(ErrorMessages.ErrorOccured, actionDescription) });
            }
        }
    }
}
