using mmDailyPlanner.Server.Models;

namespace mmDailyPlanner.Server.Data
{
    public interface IStoredProcedure
    {
        Task<User> ExecuteAsync(User user, byte[] salt);

    }
}
