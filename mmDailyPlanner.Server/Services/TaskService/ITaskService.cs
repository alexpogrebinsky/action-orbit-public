using mmDailyPlanner.Server.DTO;
using mmDailyPlanner.Server.Models;

namespace mmDailyPlanner.Server.Services.TaskService
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskListDTO>> GetTasksAsync();
        Task<TaskDetailDTO> GetTaskAsync(int taskId);
        Task AddTaskAsync(AddTaskDTO task);
        Task UpdateTaskAsync(int taskId, TaskDetailDTO task);
        Task DeleteTaskAsync(int taskId);
        Task<IEnumerable<CompletedTaskDTO>> GetCompleteTasks();
        Task MarkTaskAsCompleted(int taskId);
        Task<UserTaskAnalyticsDto> GetUserTaskAnalyticsAsync();
    }
}
