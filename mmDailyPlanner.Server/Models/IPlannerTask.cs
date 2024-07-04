namespace mmDailyPlanner.Server.Models
{
    public interface IPlannerTask
    {
        int Id { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        int Priority { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }
        DateTime DueDate { get; set; }
        string Category { get; set; }
        bool IsCompleted { get; set; }
        int? UserId { get; set; }
    }
}
