CREATE PROCEDURE [dbo].[spGetUserTaskAnalytics]
    @UserId INT
AS
BEGIN
    BEGIN TRY
        -- Create a temporary table to store PlannerTasks for the user
        SELECT *
        INTO #UserTasks
        FROM [mmDailyPlanner].[dbo].[PlannerTasks]
        WHERE UserId = @UserId;

        -- Create a temporary table to store CompletedTasks for the user
        SELECT *
        INTO #UserCompletedTasks
        FROM [mmDailyPlanner].[dbo].[CompletedTasks]
        WHERE UserId = @UserId;

        -- Total number of tasks for the user
        SELECT COUNT(*) AS TotalTasks
        FROM #UserTasks;

        -- Total number of completed tasks for the user
        SELECT COUNT(*) AS TotalCompletedTasks
        FROM #UserCompletedTasks;

        -- Total number of outstanding tasks (not completed) for the user
        SELECT COUNT(*) AS OutstandingTasks
        FROM #UserTasks
        WHERE IsCompleted = 0;

        -- Total number of tasks approaching due date (e.g., due within the next 3 days) for the user
        SELECT COUNT(*) AS ApproachingDueTasks
        FROM #UserTasks
        WHERE IsCompleted = 0 AND DueDate BETWEEN GETDATE() AND DATEADD(DAY, 3, GETDATE());

        -- Total number of past due tasks for the user
        SELECT COUNT(*) AS PastDueTasks
        FROM #UserTasks
        WHERE IsCompleted = 0 AND DueDate < GETDATE();

        -- Average priority of outstanding tasks for the user
        SELECT AVG(Priority) AS AveragePriority
        FROM #UserTasks
        WHERE IsCompleted = 0;

        -- Average time to complete a task for the user
        SELECT AVG(DATEDIFF(HOUR, DateCreated, DateCompleted)) AS AverageCompletionTime
		FROM #UserCompletedTasks
		WHERE DateCompleted IS NOT NULL;


        -- Number of tasks per category for the user
        SELECT Category, COUNT(*) AS NumberOfTasks
        FROM #UserTasks
        GROUP BY Category;

        -- Number of completed tasks per category for the user
        SELECT Category, COUNT(*) AS NumberOfCompletedTasks
        FROM #UserCompletedTasks
        GROUP BY Category;

        -- Distribution of tasks by priority level for the user
        SELECT 
            CASE 
                WHEN Priority > 5 THEN 'High'
                WHEN Priority BETWEEN 3 AND 5 THEN 'Medium'
                ELSE 'Low'
            END AS PriorityLevel,
            COUNT(*) AS TaskCount
        FROM #UserTasks
        WHERE IsCompleted = 0
        GROUP BY 
            CASE 
                WHEN Priority > 5 THEN 'High'
                WHEN Priority BETWEEN 3 AND 5 THEN 'Medium'
                ELSE 'Low'
            END;

        -- Distribution of tasks by completion status for the user
        SELECT 
            CASE 
                WHEN IsCompleted = 1 THEN 'Completed'
                ELSE 'Outstanding'
            END AS CompletionStatus,
            COUNT(*) AS TaskCount
        FROM #UserTasks
        GROUP BY 
            CASE 
                WHEN IsCompleted = 1 THEN 'Completed'
                ELSE 'Outstanding'
            END;

        -- Percentage of tasks completed on time vs. late for the user
        SELECT 
            CASE 
                WHEN DateCompleted <= DueDate THEN 'OnTime'
                ELSE 'Late'
            END AS CompletionTimeliness,
            COUNT(*) * 100.0 / NULLIF((SELECT COUNT(*) FROM #UserCompletedTasks), 0) AS Percentage
        FROM #UserCompletedTasks
        GROUP BY 
            CASE 
                WHEN DateCompleted <= DueDate THEN 'OnTime'
                ELSE 'Late'
            END;

        -- Drop the temporary tables
        DROP TABLE #UserTasks;
        DROP TABLE #UserCompletedTasks;
    END TRY
    BEGIN CATCH
        -- Error handling: Log or handle the error appropriately
        PRINT 'Error occurred: ' + ERROR_MESSAGE();
    END CATCH;
END
GO
