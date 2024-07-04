using mmDailyPlanner.Server.Models;

public class Session
{
    public int Id { get; set; }
    public string SessionId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public User User { get; set; }
    public int? SessionTokenId { get; set; }
    public SessionToken SessionToken { get; set; } // This line is crucial
}