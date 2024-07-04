namespace mmDailyPlanner.Server.Services
{
    public interface ISessionService
    {
        Task InvalidateSessionsAsync(int userId);
        Task CreateSessionAsync(string sessionId, int userId, string sessionToken);
        Task InvalidateSessionAsync(string sessionToken);
        Task<bool> ValidateSessionTokenAsync(string sessionToken);
        Task CreateSessionTokenAsync(string sessionToken, string sessionId, int userId);
        Task<int> GetCurrentUserId();
    }
}
