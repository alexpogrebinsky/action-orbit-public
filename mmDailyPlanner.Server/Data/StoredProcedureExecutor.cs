using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using mmDailyPlanner.Server.DTO;
using mmDailyPlanner.Server.Models;
using mmDailyPlanner.Server.Constants;
using mmDailyPlanner.Server.Exceptions;  
using Newtonsoft.Json;
using System.Data;
using System.Text;

namespace mmDailyPlanner.Server.Data
{
    public class StoredProcedureExecutor : IStoredProcedureExecutor
    {
        private readonly DailyPlannerContext _context;
        private readonly ILogger<StoredProcedureExecutor> _logger;

        public StoredProcedureExecutor(DailyPlannerContext context, ILogger<StoredProcedureExecutor> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> ExecuteAddUserStoredProcedureAsync(User user, byte[] salt)
        {
            try
            {
                var usernameParam = new SqlParameter("@Username", user.Username);
                var emailParam = new SqlParameter("@Email", user.Email);
                var passwordParam = new SqlParameter("@Password", user.Password);
                var addressIpParam = new SqlParameter("@AddressIP", user.AddressIP);
                var firstNameParam = new SqlParameter("@FirstName", user.FirstName);
                var isAuthenticatedParam = new SqlParameter("@IsAuthenticated", user.IsAuthenticated);
                var lastNameParam = new SqlParameter("@LastName", user.LastName);
                var phoneNumberParam = new SqlParameter("@PhoneNumber", user.PhoneNumber);
                var roleParam = new SqlParameter("@Role", user.Role);
                var profileImageParam = new SqlParameter("@ProfileImage", user.ProfileImage);
                var saltParam = new SqlParameter("@Salt", salt);

                var users = await _context.Users.FromSqlRaw(
                    $"EXECUTE {StoredProcedureNames.AddUser} @Username, @Email, @Password, @AddressIP, @FirstName, @IsAuthenticated, @LastName, @PhoneNumber, @Role, @ProfileImage, @Salt",
                    parameters: new[] { usernameParam, emailParam, passwordParam, addressIpParam, firstNameParam, isAuthenticatedParam, lastNameParam, phoneNumberParam, roleParam, profileImageParam, saltParam }
                ).ToListAsync();

                return users.FirstOrDefault();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, StoredProcedureMessages.SqlErrorAddingUser);
                throw new DatabaseOperationException(StoredProcedureMessages.SqlErrorAddingUser, ex);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, StoredProcedureMessages.DatabaseUpdateErrorAddingUser);
                throw new DatabaseOperationException(StoredProcedureMessages.DatabaseUpdateErrorAddingUser, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, StoredProcedureMessages.UnexpectedErrorAddingUser);
                throw;
            }
        }

        public async Task<UserTaskAnalyticsDto> GetUserTaskAnalyticsAsync(int userId)
        {
            try
            {
                var userTaskAnalyticsDto = new UserTaskAnalyticsDto();

                using (var connection = _context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "spGetUserTaskAnalytics";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@UserId", userId));

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync() && !reader.IsDBNull(0))
                            {
                                userTaskAnalyticsDto.TotalTasks = reader.GetInt32(0);
                            }

                            await reader.NextResultAsync();

                            if (await reader.ReadAsync() && !reader.IsDBNull(0))
                            {
                                userTaskAnalyticsDto.TotalCompletedTasks = reader.GetInt32(0);
                            }

                            await reader.NextResultAsync();

                            if (await reader.ReadAsync() && !reader.IsDBNull(0))
                            {
                                userTaskAnalyticsDto.OutstandingTasks = reader.GetInt32(0);
                            }

                            await reader.NextResultAsync();

                            if (await reader.ReadAsync() && !reader.IsDBNull(0))
                            {
                                userTaskAnalyticsDto.ApproachingDueTasks = reader.GetInt32(0);
                            }

                            await reader.NextResultAsync();

                            if (await reader.ReadAsync() && !reader.IsDBNull(0))
                            {
                                userTaskAnalyticsDto.PastDueTasks = reader.GetInt32(0);
                            }

                            await reader.NextResultAsync();

                            if (await reader.ReadAsync() && !reader.IsDBNull(0))
                            {
                                userTaskAnalyticsDto.AveragePriority = Convert.ToDecimal(reader.GetInt32(0));
                            }

                            await reader.NextResultAsync();

                            if (await reader.ReadAsync() && !reader.IsDBNull(0))
                            {
                                userTaskAnalyticsDto.AverageCompletionTime = reader.GetInt32(0);
                            }

                            await reader.NextResultAsync();

                            var tasksPerCategoryJson = new StringBuilder();
                            while (await reader.ReadAsync() && !reader.IsDBNull(0) && !reader.IsDBNull(1))
                            {
                                tasksPerCategoryJson.Append(reader.GetString(0));
                                tasksPerCategoryJson.Append(":");
                                tasksPerCategoryJson.Append(reader.GetInt32(1));
                                tasksPerCategoryJson.Append(",");
                            }
                            userTaskAnalyticsDto.TasksPerCategoryJson = tasksPerCategoryJson.ToString();

                            await reader.NextResultAsync();

                            var completedTasksPerCategoryJson = new StringBuilder();
                            while (await reader.ReadAsync() && !reader.IsDBNull(0) && !reader.IsDBNull(1))
                            {
                                completedTasksPerCategoryJson.Append(reader.GetString(0));
                                completedTasksPerCategoryJson.Append(":");
                                completedTasksPerCategoryJson.Append(reader.GetInt32(1));
                                completedTasksPerCategoryJson.Append(",");
                            }
                            userTaskAnalyticsDto.CompletedTasksPerCategoryJson = completedTasksPerCategoryJson.ToString();

                            await reader.NextResultAsync();

                            var tasksByPriorityLevelJson = new StringBuilder();
                            while (await reader.ReadAsync() && !reader.IsDBNull(0) && !reader.IsDBNull(1))
                            {
                                tasksByPriorityLevelJson.Append(reader.GetString(0));
                                tasksByPriorityLevelJson.Append(":");
                                tasksByPriorityLevelJson.Append(reader.GetInt32(1));
                                tasksByPriorityLevelJson.Append(",");
                            }
                            userTaskAnalyticsDto.TasksByPriorityLevelJson = tasksByPriorityLevelJson.ToString();

                            await reader.NextResultAsync();

                            var tasksByCompletionStatusJson = new StringBuilder();
                            while (await reader.ReadAsync() && !reader.IsDBNull(0) && !reader.IsDBNull(1))
                            {
                                tasksByCompletionStatusJson.Append(reader.GetString(0));
                                tasksByCompletionStatusJson.Append(":");
                                tasksByCompletionStatusJson.Append(reader.GetInt32(1));
                                tasksByCompletionStatusJson.Append(",");
                            }
                            userTaskAnalyticsDto.TasksByCompletionStatusJson = tasksByCompletionStatusJson.ToString();

                            await reader.NextResultAsync();

                            var taskCompletionTimelinessJson = new StringBuilder();
                            while (await reader.ReadAsync() && !reader.IsDBNull(0) && !reader.IsDBNull(1))
                            {
                                taskCompletionTimelinessJson.Append(reader.GetString(0));
                                taskCompletionTimelinessJson.Append(":");
                                taskCompletionTimelinessJson.Append(reader.GetDecimal(1));
                                taskCompletionTimelinessJson.Append(",");
                            }
                            userTaskAnalyticsDto.TaskCompletionTimelinessJson = taskCompletionTimelinessJson.ToString();
                        }
                    }
                }

                return userTaskAnalyticsDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, StoredProcedureMessages.UnexpectedErrorAddingUser);
                throw;
            }
        }
    }
}
