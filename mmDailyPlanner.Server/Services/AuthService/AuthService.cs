using mmDailyPlanner.Server.Data;
using mmDailyPlanner.Server.DTO;
using mmDailyPlanner.Server.Models;
using mmDailyPlanner.Server.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace mmDailyPlanner.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ISessionService _sessionService;
        private readonly DailyPlannerContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepository,
            IPasswordService passwordService,
            ISessionService sessionService,
            DailyPlannerContext context,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _sessionService = sessionService;
            _context = context;
            _logger = logger;
        }

        public async Task<bool> IsAuthenticatedAsync(string sessionTokenValue)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionTokenValue))
                    return false;

                var sessionToken = await _context.SessionTokens
                    .FirstOrDefaultAsync(st => st.Token == sessionTokenValue);

                if (sessionToken == null)
                    return false;

                var session = await _context.Sessions
                    .FirstOrDefaultAsync(s => s.SessionTokenId == sessionToken.SessionId && s.IsActive);

                return session != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking authentication.");
                return false;
            }
        }

        public async Task<(bool success, string message)> RegisterAsync(UserDTO userDto, string ipAddress)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByUsernameAsync(userDto.Username);
                if (existingUser != null)
                    return (false, "Username is already taken.");

                var user = new User
                {
                    Username = userDto.Username,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    PhoneNumber = userDto.PhoneNumber,
                    Email = userDto.Email,
                    Password = userDto.Password,
                    AddressIP = ipAddress,
                    IsAuthenticated = false
                };

                if (userDto.ProfileImage != null)
                {
                    using var memoryStream = new MemoryStream();
                    await userDto.ProfileImage.CopyToAsync(memoryStream);
                    user.ProfileImage = memoryStream.ToArray();
                }

                await _userRepository.AddUserAsync(user);
                return (true, "User registered successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user.");
                return (false, "An error occurred while registering the user.");
            }
        }

        public async Task<(bool success, string message)> LoginAsync(LoginModel loginModel, string ipAddress, string sessionToken, HttpResponse response)
        {
            try
            {
                var user = await _userRepository.GetUserByUsernameAsync(loginModel.Username);
                if (user == null || !_passwordService.VerifyPassword(loginModel.Password, user.Password, user.Salt))
                    return (false, "Invalid username or password.");

                var rnd = new Random();
                var newSessionId = rnd.Next(100000, 999999).ToString();

                await _sessionService.InvalidateSessionsAsync(user.Id);

                var newSessionToken = GenerateRandomSessionToken();

                await _sessionService.CreateSessionAsync(newSessionId, user.Id, newSessionToken);
                await _sessionService.CreateSessionTokenAsync(newSessionToken, newSessionId, user.Id);

                response.Cookies.Append("sessionToken", newSessionToken, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict
                });

                return (true, "Login successful.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging in user.");
                return (false, "An error occurred while logging in the user.");
            }
        }

        public async Task<UserProfileDTO> GetUserAsync(string sessionToken)
        {
            try
            {
                var id = await _userRepository.GetUserId(sessionToken);
                return id > 0 ? await _userRepository.GetUserByIdAsync(id) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user.");
                return null;
            }
        }

        public async Task LogoutAsync(string sessionToken, HttpResponse response)
        {
            try
            {
                if (sessionToken != null)
                {
                    var session = await _context.SessionTokens
                        .Include(st => st.Session)
                        .FirstOrDefaultAsync(st => st.Token == sessionToken);

                    if (session != null)
                    {
                        session.Session.IsActive = false;
                        _context.Sessions.Update(session.Session);
                        await _context.SaveChangesAsync();

                        _context.SessionTokens.Remove(session);
                        await _context.SaveChangesAsync();
                    }
                }

                response.Cookies.Delete("sessionToken");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging out user.");
            }
        }

        public async Task<int> GetCurrentUserIdAsync(string sessionToken)
        {
            try
            {
                var sessionTokenEntity = await _context.SessionTokens
                    .FirstOrDefaultAsync(st => st.Token == sessionToken);

                if (sessionTokenEntity == null)
                    return 0;

                var session = await _context.Sessions
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.SessionTokenId == sessionTokenEntity.SessionId && s.IsActive);

                return session?.UserId ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving current user ID.");
                return 0;
            }
        }

        private string GenerateRandomSessionToken()
        {
            try
            {
                var randomBytes = new byte[32];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomBytes);
                }
                return Convert.ToBase64String(randomBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating random session token.");
                throw;
            }
        }

    }
}
