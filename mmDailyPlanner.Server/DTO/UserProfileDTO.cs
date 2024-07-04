using mmDailyPlanner.Server.Models;

namespace mmDailyPlanner.Server.DTO
{
    public class UserProfileDTO
    {
        public string Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsAuthenticated { get; set; }
        public byte[] ProfileImage { get; set; }
    }
}
