using mmDailyPlanner.Server.DTO;
using mmDailyPlanner.Server.Models;
using mmDailyPlanner.Server.Services;

namespace mmDailyPlanner.Server.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<IPlannerTask>> GetTasksAsync(int userId);
        Task<IPlannerTask> GetTaskByIdAsync(int id);
        Task AddTaskAsync(AddTaskDTO task, int userId);
        Task UpdateTaskAsync(TaskDetailDTO task);
        Task DeleteTaskAsync(int id);
        Task MarkTaskAsCompletedAsync(int taskId);
        Task<IEnumerable<CompletedTaskDTO>> GetCompletedTasksAsync(int userId);
        Task<UserTaskAnalyticsDto> GetUserTaskAnalyticsAsync(int userId);
        Task<ValidationResult> TaskAndUserAreValid(int taskId, int userId);
    }
}
