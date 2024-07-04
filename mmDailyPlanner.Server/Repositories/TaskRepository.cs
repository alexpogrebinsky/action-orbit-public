using Microsoft.EntityFrameworkCore;
using mmDailyPlanner.Server.Data;
using mmDailyPlanner.Server.Models;
using mmDailyPlanner.Server.Repositories;
using mmDailyPlanner.Server.DTO;
using mmDailyPlanner.Server.Services;
using AutoMapper;
using mmDailyPlanner.Server.Constants;

public class TaskRepository : ITaskRepository
{
    private readonly DailyPlannerContext _context;
    private readonly IStoredProcedureExecutorFactory _storedProcedureExecutorFactory;
    private readonly ISessionService _sessionService;
    private readonly IMapper _mapper;
    private readonly ILogger<TaskRepository> _logger;

    public TaskRepository(
        DailyPlannerContext context,
        IStoredProcedureExecutorFactory storedProcedureExecutorFactory,
        ISessionService sessionService,
        IMapper mapper,
        ILogger<TaskRepository> logger)
    {
        _context = context;
        _storedProcedureExecutorFactory = storedProcedureExecutorFactory;
        _sessionService = sessionService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<IPlannerTask>> GetTasksAsync(int userId)
    {
        try
        {
            var tasks = await _context.PlannerTasks
                .Where(task => task.UserId == userId)
                .Where(task => task.IsCompleted == false)
                .ToListAsync();

            return tasks;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ErrorMessages.TasksGetFailed);
            throw new Exception(ErrorMessages.TasksGetFailed, ex);
        }
    }

    public async Task<IPlannerTask> GetTaskByIdAsync(int id)
    {
        try
        {
            return await _context.PlannerTasks.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Format(ErrorMessages.TaskGetFailed, id));
            throw new Exception(string.Format(ErrorMessages.TaskGetFailed, id), ex);
        }
    }

    public async Task AddTaskAsync(AddTaskDTO task, int userId)
    {
        try
        {
            var concreteTask = _mapper.Map<PlannerTask>(task);
            concreteTask.DateCreated = DateTime.UtcNow;
            concreteTask.DateModified = DateTime.UtcNow;
            concreteTask.IsCompleted = false;
            concreteTask.UserId = userId;

            await _context.PlannerTasks.AddAsync(concreteTask);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ErrorMessages.TaskAddFailed);
            throw new Exception(ErrorMessages.TaskAddFailed, ex);
        }
    }

    public async Task UpdateTaskAsync(TaskDetailDTO task)
    {
        try
        {
            var existingTask = await _context.PlannerTasks.FindAsync(task.Id);
            if (existingTask == null)
            {
                throw new Exception(ErrorMessages.TaskNotFound);
            }

            _mapper.Map(task, existingTask);
            existingTask.DateModified = DateTime.UtcNow;

            _context.PlannerTasks.Update(existingTask);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ErrorMessages.TaskUpdateFailed);
            throw new Exception(ErrorMessages.TaskUpdateFailed, ex);
        }
    }

    public async Task DeleteTaskAsync(int id)
    {
        try
        {
            var task = await _context.PlannerTasks.FindAsync(id);
            if (task != null)
            {
                _context.PlannerTasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Format(ErrorMessages.TaskDeleteFailed, id));
            throw new Exception(string.Format(ErrorMessages.TaskDeleteFailed, id), ex);
        }
    }

    public async Task MarkTaskAsCompletedAsync(int taskId)
    {
        try
        {
            var task = await _context.PlannerTasks.FindAsync(taskId);
            if (task != null)
            {
                var completedTask = new CompletedTask
                {
                    Title = task.Title,
                    Description = task.Description,
                    Priority = task.Priority,
                    DateCreated = task.DateCreated,
                    DateModified = task.DateModified,
                    DueDate = task.DueDate,
                    DateCompleted = DateTime.UtcNow,
                    Category = task.Category,
                    IsCompleted = true,
                    UserId = (int)task.UserId
                };

                await _context.CompletedTasks.AddAsync(completedTask);
                task.IsCompleted = true;
                task.DateModified = DateTime.UtcNow;
                _context.PlannerTasks.Update(task);

                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Format(ErrorMessages.TaskMarkCompleteFailed, taskId));
            throw new Exception(string.Format(ErrorMessages.TaskMarkCompleteFailed, taskId), ex);
        }
    }

    public async Task<IEnumerable<CompletedTaskDTO>> GetCompletedTasksAsync(int userId)
    {
        try
        {
            var completedTasks = await _context.CompletedTasks
                .Where(task => task.UserId == userId)
                .ToListAsync();

            var completedTaskDTOs = completedTasks.Select(task => _mapper.Map<CompletedTaskDTO>(task));

            return completedTaskDTOs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Format(ErrorMessages.CompletedTasksGetFailed, userId));
            throw new Exception(string.Format(ErrorMessages.CompletedTasksGetFailed, userId), ex);
        }
    }

    public async Task<UserTaskAnalyticsDto> GetUserTaskAnalyticsAsync(int userId)
    {
        try
        {
            var storedProcedureExecutor = _storedProcedureExecutorFactory.Create();
            return await storedProcedureExecutor.GetUserTaskAnalyticsAsync(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ErrorMessages.TaskInsightsFailed);
            throw new Exception(ErrorMessages.TaskInsightsFailed, ex);
        }
    }

    public async Task<ValidationResult> TaskAndUserAreValid(int taskId, int userId)
    {
        var isValid = await _context.PlannerTasks.AnyAsync(t => t.Id == taskId && t.UserId == userId);

        if (!isValid)
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = string.Format(ErrorMessages.TaskOwnershipValidationFailed, taskId, userId)
            };
        }

        return new ValidationResult
        {
            IsValid = true
        };
    }
}