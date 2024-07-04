namespace mmDailyPlanner.Server.Models
{
    public class PlannerTask: IPlannerTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DueDate { get; set; }
        public string Category { get; set; }
        public bool IsCompleted { get; set; }
        public int? UserId { get; set; }
    }
}
