using Azure.Core;
using Microsoft.EntityFrameworkCore;
using mmDailyPlanner.Server.Data;
using mmDailyPlanner.Server.Models;
using mmDailyPlanner.Server.Repositories;
using Microsoft.Extensions.Logging;
using mmDailyPlanner.Server.Constants;
using System;

namespace mmDailyPlanner.Server.Services
{
    public class SessionService : ISessionService
    {
        private readonly DailyPlannerContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<SessionService> _logger;

        public SessionService(DailyPlannerContext context,
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            ILogger<SessionService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task CreateSessionTokenAsync(string sessionToken, string sessionId, int userId)
        {
            try
            {
                var session = await _context.Sessions.SingleOrDefaultAsync(s => s.SessionId == sessionId);
                if (session == null)
                {
                    _logger.LogError(ErrorMessages.SessionTokenMissing);
                    throw new ArgumentException(ErrorMessages.SessionTokenMissing);
                }
                var session_id = int.Parse(sessionId);
                var newSessionToken = new SessionToken
                {
                    Token = sessionToken,
                    SessionId = session.Id,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(7)
                };

                session.SessionToken = newSessionToken;

                _context.SessionTokens.Add(newSessionToken);
                await _context.SaveChangesAsync();

                _logger.LogInformation(AuthMessages.UserRegistered);
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, AuthMessages.Registration);
                throw new ArgumentException(AuthMessages.Registration, ex);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorOccuredWhileActionDescription, AuthMessages.Registration);
                throw new InvalidOperationException(ErrorMessages.ErrorOccuredWhileActionDescription, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, AuthMessages.UserLoginFailed, ex.Message);
                throw;
            }
        }

        public async Task InvalidateSessionsAsync(int userId)
        {
            try
            {
                var sessions = await _context.Sessions.Where(s => s.UserId == userId).ToListAsync();
                foreach (var session in sessions)
                {
                    session.IsActive = false;
                }
                await _context.SaveChangesAsync();
                _logger.LogInformation(AuthMessages.UserLoggedOut);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorOccuredWhileActionDescription, AuthMessages.Logout);
                throw;
            }
        }

        public async Task CreateSessionAsync(string sessionId, int userId, string sessionToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var session = new Session
                {
                    SessionId = sessionId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };
                _context.Sessions.Add(session);

                var sessionTokenEntity = new SessionToken
                {
                    Token = sessionToken,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddHours(1),
                    Session = session
                };
                _context.SessionTokens.Add(sessionTokenEntity);

                await _context.SaveChangesAsync();

                session.SessionTokenId = sessionTokenEntity.Id;
                _context.Sessions.Update(session);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                _logger.LogInformation(AuthMessages.UserLoggedIn);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, AuthMessages.UserLoginFailed, ex.Message);
                throw;
            }
        }

        public async Task InvalidateSessionAsync(string sessionToken)
        {
            var token = await _context.SessionTokens.SingleOrDefaultAsync(st => st.Token == sessionToken);
            if (token != null)
            {
                _context.SessionTokens.Remove(token);

                var session = await _context.Sessions.SingleOrDefaultAsync(s => s.SessionId == token.Token);
                if (session != null)
                {
                    session.IsActive = false;
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation(AuthMessages.LogoutSuccess);
            }
        }

        public async Task<bool> ValidateSessionTokenAsync(string sessionToken)
        {
            var token = await _context.SessionTokens.SingleOrDefaultAsync(st => st.Token == sessionToken && st.ExpiresAt > DateTime.UtcNow);
            return token != null;
        }

        public async Task<int> GetCurrentUserId()
        {
            var sessionTokenValue = _httpContextAccessor.HttpContext.Request.Cookies["sessionToken"];
            var userId = await _userRepository.GetUserId(sessionTokenValue);
            _logger.LogInformation(AuthMessages.UserIdRetrieved);
            return userId;
        }
    }
}
