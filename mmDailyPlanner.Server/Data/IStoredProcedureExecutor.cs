using mmDailyPlanner.Server.DTO;
using mmDailyPlanner.Server.Models;

namespace mmDailyPlanner.Server.Data
{
    public interface IStoredProcedureExecutor
    {
        Task<User> ExecuteAddUserStoredProcedureAsync(User user, byte[] salt);
        Task<UserTaskAnalyticsDto> GetUserTaskAnalyticsAsync(int userId);
    }
}
