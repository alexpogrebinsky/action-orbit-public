namespace mmDailyPlanner.Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AddressIP { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set;}
        public string? Role { get; set; }
        public bool? IsAuthenticated { get; set; }
        public byte[] ProfileImage { get; set; }
        public byte[] Salt { get; set; }
        public ICollection<Session> Sessions { get; set; }
        public ICollection<SessionToken> SessionTokens { get; set; }

    }
}
