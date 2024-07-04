namespace mmDailyPlanner.Server.Models
{
    public class SessionToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int SessionId { get; set; } // Foreign key
        public User User { get; set; }
        public Session Session { get; set; }
    }

}
