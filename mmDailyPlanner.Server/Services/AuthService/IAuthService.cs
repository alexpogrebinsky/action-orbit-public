using mmDailyPlanner.Server.DTO;
using mmDailyPlanner.Server.Models;

namespace mmDailyPlanner.Server.Services.AuthService
{
    public interface IAuthService
    {
        Task<bool> IsAuthenticatedAsync(string sessionToken);
        Task<(bool success, string message)> RegisterAsync(UserDTO userDto, string ipAddress);
        Task<(bool success, string message)> LoginAsync(LoginModel loginModel, string ipAddress, string sessionToken, HttpResponse response);
        Task<UserProfileDTO> GetUserAsync(string sessionToken);
        Task LogoutAsync(string sessionToken, HttpResponse response);
        Task<int> GetCurrentUserIdAsync(string sessionToken);
    }
}
