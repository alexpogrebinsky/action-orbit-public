using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using mmDailyPlanner.Server.Data;
using mmDailyPlanner.Server.DTO;
using mmDailyPlanner.Server.Models;
using mmDailyPlanner.Server.Repositories;
using mmDailyPlanner.Server.Services;
using mmDailyPlanner.Server.Services.TaskService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using mmDailyPlanner.Server.Constants;

namespace mmDailyPlanner.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISessionService _sessionService;
        private readonly DailyPlannerContext _context;
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskController> _logger;

        public TaskController(
            ITaskRepository taskRepository,
            IUserRepository userRepository,
            ISessionService sessionService,
            DailyPlannerContext context,
            ITaskService taskService,
            IMapper mapper,
            ILogger<TaskController> logger)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _sessionService = sessionService;
            _context = context;
            _taskService = taskService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("get-tasks")]
        public async Task<IActionResult> GetTasks()
        {
            return await HandleRequestAsync(async () =>
            {
                var tasks = await _taskService.GetTasksAsync();
                _logger.LogInformation(TaskMessages.TaskRetrieved);
                return Ok(tasks);
            }, "getting tasks", ErrorMessages.TaskGetFailed);
        }

        [HttpGet("get-task-by-taskId/{taskId}")]
        public async Task<IActionResult> GetTask(int taskId)
        {
            return await HandleRequestAsync(async () =>
            {
                var task = await _taskService.GetTaskAsync(taskId);
                if (task == null)
                {
                    return NotFound(new { message = ErrorMessages.TaskNotFound });
                }
                _logger.LogInformation(TaskMessages.TaskRetrieved);
                return Ok(task);
            }, "getting task by ID", ErrorMessages.TaskGetFailed);
        }

        [HttpPost("add-task")]
        public async Task<IActionResult> AddTask([FromBody] AddTaskDTO task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await HandleRequestAsync(async () =>
            {
                await _taskService.AddTaskAsync(task);
                _logger.LogInformation(TaskMessages.TaskAdded);
                return Ok(new { message = TaskMessages.TaskAdded });
            }, "adding task", ErrorMessages.TaskAddFailed);
        }


        [HttpPut("update-task/{taskId}")]
        public async Task<IActionResult> UpdateTask(int taskId, [FromBody] TaskDetailDTO task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await HandleRequestAsync(async () =>
            {
                await _taskService.UpdateTaskAsync(taskId, task);
                _logger.LogInformation(TaskMessages.TaskUpdated);
                return Ok(new { message = TaskMessages.TaskUpdated });
            }, "updating task", ErrorMessages.TaskUpdateFailed);
        }

        [HttpDelete("delete-task/{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            return await HandleRequestAsync(async () =>
            {
                await _taskService.DeleteTaskAsync(taskId);
                _logger.LogInformation(TaskMessages.TaskDeleted);
                return Ok(new { message = TaskMessages.TaskDeleted });
            }, "deleting task", ErrorMessages.TaskDeleteFailed);
        }

        [HttpPut("mark-task-completed/{taskId}")]
        public async Task<IActionResult> MarkTaskAsCompleted(int taskId)
        {
            return await HandleRequestAsync(async () =>
            {
                await _taskService.MarkTaskAsCompleted(taskId);
                _logger.LogInformation(TaskMessages.TaskCompleted);
                return NoContent();
            }, "marking task as completed", ErrorMessages.TaskMarkCompleteFailed);
        }

        [HttpGet("get-completed-tasks")]
        public async Task<IActionResult> GetCompletedTasks()
        {
            return await HandleRequestAsync(async () =>
            {
                var completedTasks = await _taskService.GetCompleteTasks();
                _logger.LogInformation(TaskMessages.TaskRetrieved);
                return Ok(new { message = TaskMessages.TaskRetrieved, data = completedTasks });
            }, "getting completed tasks", ErrorMessages.TaskGetFailed);
        }

        [HttpGet("task-insights")]
        public async Task<ActionResult<List<UserTaskAnalyticsDto>>> GetUserTaskAnalyticsAsync()
        {
            try
            {
                var analytics = await _taskService.GetUserTaskAnalyticsAsync();
                _logger.LogInformation(TaskMessages.TaskInsightsRetrieved);
                return Ok(analytics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting user task analytics");
                return StatusCode(500, new { message = ErrorMessages.TaskInsightsFailed });
            }
        }

        private async Task<IActionResult> HandleRequestAsync(Func<Task<IActionResult>> func, string actionDescription, string errorMessage)
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while {actionDescription}");
                return StatusCode(500, new { message = string.Format(ErrorMessages.GeneralError, actionDescription) });
            }
        }
    }
}
