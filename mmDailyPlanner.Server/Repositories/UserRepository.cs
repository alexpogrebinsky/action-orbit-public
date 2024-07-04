using mmDailyPlanner.Server.Data;
using mmDailyPlanner.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using mmDailyPlanner.Server.Services;
using mmDailyPlanner.Server.DTO;
using System;
using System.Threading.Tasks;
using AutoMapper;
using mmDailyPlanner.Server.Constants;

namespace mmDailyPlanner.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DailyPlannerContext _context;
        private readonly IMemoryCache _cache;
        private readonly IPasswordService _passwordService;
        private readonly IStoredProcedureExecutorFactory _storedProcedureExecutorFactory;
        private readonly ILogger<UserRepository> _logger;
        private readonly IMapper _mapper;
        public UserRepository(
            DailyPlannerContext context,
            IMemoryCache cache,
            IPasswordService passwordService,
            IStoredProcedureExecutorFactory storedProcedureExecutorFactory,
            ILogger<UserRepository> logger,
            IMapper mapper)
        {
            _context = context;
            _cache = cache;
            _passwordService = passwordService;
            _storedProcedureExecutorFactory = storedProcedureExecutorFactory;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserProfileDTO> GetUserByIdAsync(int id)
        {
            try
            {
                if (!_cache.TryGetValue(id, out User? user))
                {
                    user = await _context.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Id == id);

                    if (user != null)
                    {
                        _cache.Set(id, user, TimeSpan.FromMinutes(5));
                    }
                }

                UserProfileDTO userProfileDTO = _mapper.Map<UserProfileDTO>(user);
                return userProfileDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting user by ID: {UserId}", id);
                throw;
            }
        }


        public async Task<User> GetUserByUsernameAsync(string username)
        {
            try
            {
                return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting user by username: {Username}", username);
                throw;
            }
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "User object cannot be null.");
                }

                (user.Password, byte[] salt) = _passwordService.HashPassword(user.Password);

                user.Role = Roles.SetNewUserRole();

                var storedProcedureExecutor = _storedProcedureExecutorFactory.Create();
                var newUser = await storedProcedureExecutor.ExecuteAddUserStoredProcedureAsync(user, salt);

                if (newUser == null)
                {
                    throw new InvalidOperationException("Stored procedure execution failed: no user was returned.");
                }

                _logger.LogInformation("User added successfully: {UserId}", newUser.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding user: {Username}", user?.Username);
                throw;
            }
        }

        public async Task<UserTaskAnalyticsDto> GetUserTaskAnalyticsAsync(int userId)
        {
            try
            {
                var storedProcedureExecutor = _storedProcedureExecutorFactory.Create();
                return await storedProcedureExecutor.GetUserTaskAnalyticsAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting task analytics for user: {UserId}", userId);
                throw;
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            try
            {
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _logger.LogInformation("User updated successfully: {UserId}", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user: {UserId}", user.Id);
                throw;
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("User deleted successfully: {UserId}", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user: {UserId}", id);
                throw;
            }
        }

        public async Task<int> GetUserId(string sessionTokenValue)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionTokenValue))
                {
                    return 0; 
                }

                var sessionToken = await _context.SessionTokens
                    .FirstOrDefaultAsync(st => st.Token == sessionTokenValue);

                if (sessionToken == null)
                {
                    return 0; 
                }

                var session = await _context.Sessions
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.SessionTokenId == sessionToken.SessionId && s.IsActive);

                if (session == null)
                {
                    return 0; 
                }

                return session.UserId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting user ID for session token: {SessionToken}", sessionTokenValue);
                throw;
            }
        }

    }
}
