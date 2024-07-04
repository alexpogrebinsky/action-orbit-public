namespace mmDailyPlanner.Server.Constants
{
    public static class ErrorMessages
    {
        public const string SessionTokenMissing = "Session token is missing.";
        public const string UserNotFound = "User not found.";
        public const string NoValidSession = "No valid session found.";
        public const string GeneralError = "An error occurred while {0}.";
        public const string UserRegistrationFailed = "User registration failed: {Message}";
        public const string UserLoginFailed = "User login failed: {Message}";
        public const string ErrorOccuredWhileActionDescription = "Error occurred while {ActionDescription}.";
        public const string ErrorOccured = "Error occurred while {ActionDescription}.";

        // Task-specific error messages
        public const string TaskNotFound = "Task not found.";
        public const string TaskAddFailed = "Task addition failed.";
        public const string TaskUpdateFailed = "Task update failed.";
        public const string TaskDeleteFailed = "Task deletion failed.";
        public const string TaskMarkCompleteFailed = "Task marking as complete failed.";
        public const string TaskGetFailed = "Failed to retrieve task.";
        public const string TasksGetFailed = "Failed to retrieve tasks.";
        public const string CompletedTasksGetFailed = "Failed to retrieve completed tasks.";
        public const string TaskInsightsFailed = "Failed to retrieve task insights.";
        public const string TaskOwnershipValidationFailed = "Task validation failed or does not belong to the user.";

        // Database-specific error messages
        public const string DatabaseUpdateError = "Database update error.";
    }
}
