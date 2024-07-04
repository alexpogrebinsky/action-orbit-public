using mmDailyPlanner.Server.DTO;
using mmDailyPlanner.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mmDailyPlanner.Server.Repositories
{
    public interface IUserRepository
    {
        Task<UserProfileDTO> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<UserTaskAnalyticsDto> GetUserTaskAnalyticsAsync(int userId);
        Task<int> GetUserId(string sessionTokenValue);
    }
}
