using System.ComponentModel.DataAnnotations.Schema;

namespace mmDailyPlanner.Server.DTO
{
    public class UserTaskAnalyticsDto
    {
        public int TotalTasks { get; set; }
        public int TotalCompletedTasks { get; set; }
        public int OutstandingTasks { get; set; }
        public int ApproachingDueTasks { get; set; }
        public int PastDueTasks { get; set; }
        public decimal AveragePriority { get; set; }
        public int AverageCompletionTime { get; set; }
        public string TasksPerCategoryJson { get; set; }
        public string CompletedTasksPerCategoryJson { get; set; }
        public string TasksByPriorityLevelJson { get; set; }
        public string TasksByCompletionStatusJson { get; set; }
        public string TaskCompletionTimelinessJson { get; set; }

        [NotMapped]
        public Dictionary<string, int> TasksPerCategory { get; set; }

        [NotMapped]
        public Dictionary<string, int> CompletedTasksPerCategory { get; set; }

        [NotMapped]
        public Dictionary<string, int> TasksByPriorityLevel { get; set; }

        [NotMapped]
        public Dictionary<string, int> TasksByCompletionStatus { get; set; }

        [NotMapped]
        public Dictionary<string, double> TaskCompletionTimeliness { get; set; }
    }
}
