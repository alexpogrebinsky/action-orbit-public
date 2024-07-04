using AutoMapper;
using Microsoft.Extensions.Logging;
using mmDailyPlanner.Server.Constants;
using mmDailyPlanner.Server.Data;
using mmDailyPlanner.Server.DTO;
using mmDailyPlanner.Server.Models;
using mmDailyPlanner.Server.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mmDailyPlanner.Server.Services.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISessionService _sessionService;
        private readonly DailyPlannerContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskService> _logger;

        public TaskService(
            ITaskRepository taskRepository,
            IUserRepository userRepository,
            ISessionService sessionService,
            DailyPlannerContext context,
            IMapper mapper,
            ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _sessionService = sessionService;
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<TaskListDTO>> GetTasksAsync()
        {
            try
            {
                var userId = await _sessionService.GetCurrentUserId();
                var tasks = await _taskRepository.GetTasksAsync(userId);
                var taskDtos = _mapper.Map<IEnumerable<TaskListDTO>>(tasks);
                _logger.LogInformation(TaskMessages.TasksRetrieved);
                return taskDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(ErrorMessages.ErrorOccuredWhileActionDescription, "fetching tasks"));
                throw new Exception(string.Format(ErrorMessages.ErrorOccuredWhileActionDescription, "fetching tasks"), ex);
            }
        }

        public async Task<TaskDetailDTO> GetTaskAsync(int taskId)
        {
            try
            {
                var userId = await _sessionService.GetCurrentUserId();
                var validationResult = await _taskRepository.TaskAndUserAreValid(taskId, userId);
                if (validationResult.IsValid)
                {
                    var task = await _taskRepository.GetTaskByIdAsync(taskId);
                    var taskDTO = _mapper.Map<TaskDetailDTO>(task);
                    _logger.LogInformation(TaskMessages.TaskRetrieved);
                    return taskDTO ?? throw new Exception(ErrorMessages.TaskNotFound);
                }
                else
                {
                    throw new Exception(validationResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(ErrorMessages.ErrorOccuredWhileActionDescription, "fetching task"));
                throw new Exception(string.Format(ErrorMessages.ErrorOccuredWhileActionDescription, "fetching task"), ex);
            }
        }

        public async Task AddTaskAsync(AddTaskDTO task)
        {
            try
            {
                var userId = await _sessionService.GetCurrentUserId();
                await _taskRepository.AddTaskAsync(task, userId);
                _logger.LogInformation(TaskMessages.TaskAdded);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.TaskAddFailed);
                throw new Exception(ErrorMessages.TaskAddFailed, ex);
            }
        }

        public async Task UpdateTaskAsync(int taskId, TaskDetailDTO task)
        {
            try
            {
                if (taskId != task.Id)
                {
                    throw new Exception(ErrorMessages.TaskUpdateFailed);
                }
                var userId = await _sessionService.GetCurrentUserId();
                var validationResult = await _taskRepository.TaskAndUserAreValid(taskId, userId);
                if (validationResult.IsValid)
                {
                    await _taskRepository.UpdateTaskAsync(task);
                    _logger.LogInformation(TaskMessages.TaskUpdated);
                }
                else
                {
                    throw new Exception(ErrorMessages.TaskOwnershipValidationFailed);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.TaskUpdateFailed);
                throw new Exception(ErrorMessages.TaskUpdateFailed, ex);
            }
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            try
            {
                var userId = await _sessionService.GetCurrentUserId();
                var validationResult = await _taskRepository.TaskAndUserAreValid(taskId, userId);
                if (validationResult.IsValid)
                {
                    await _taskRepository.DeleteTaskAsync(taskId);
                    _logger.LogInformation(TaskMessages.TaskDeleted);
                }
                else
                {
                    throw new Exception(ErrorMessages.TaskOwnershipValidationFailed);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.TaskDeleteFailed);
                throw new Exception(ErrorMessages.TaskDeleteFailed, ex);
            }
        }

        public async Task<IEnumerable<CompletedTaskDTO>> GetCompleteTasks()
        {
            try
            {
                var userId = await _sessionService.GetCurrentUserId();
                var completedTasks = await _taskRepository.GetCompletedTasksAsync(userId);
                _logger.LogInformation(TaskMessages.CompletedTasksRetrieved);
                return completedTasks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.CompletedTasksGetFailed);
                throw new Exception(ErrorMessages.CompletedTasksGetFailed, ex);
            }
        }

        public async Task MarkTaskAsCompleted(int taskId)
        {
            try
            {
                var userId = await _sessionService.GetCurrentUserId();
                var validationResult = await _taskRepository.TaskAndUserAreValid(taskId, userId);
                if (validationResult.IsValid)
                {
                    await _taskRepository.MarkTaskAsCompletedAsync(taskId);
                    _logger.LogInformation(TaskMessages.TaskCompleted);
                }
                else
                {
                    throw new Exception(ErrorMessages.TaskOwnershipValidationFailed);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.TaskMarkCompleteFailed);
                throw new Exception(ErrorMessages.TaskMarkCompleteFailed, ex);
            }
        }

        public async Task<UserTaskAnalyticsDto> GetUserTaskAnalyticsAsync()
        {
            try
            {
                var userId = await _sessionService.GetCurrentUserId();
                var analytics = await _taskRepository.GetUserTaskAnalyticsAsync(userId);
                _logger.LogInformation(TaskMessages.TaskInsightsRetrieved);
                return analytics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.TaskInsightsFailed);
                throw new Exception(ErrorMessages.TaskInsightsFailed, ex);
            }
        }
    }
}
