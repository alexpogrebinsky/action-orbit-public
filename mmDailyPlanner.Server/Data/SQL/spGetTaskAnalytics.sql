CREATE PROCEDURE [dbo].[spGetTaskAnalytics]
AS
BEGIN
    -- Total number of tasks
    SELECT COUNT(*) AS TotalTasks
    FROM [mmDailyPlanner].[dbo].[PlannerTasks];

    -- Total number of completed tasks
    SELECT COUNT(*) AS TotalCompletedTasks
    FROM [mmDailyPlanner].[dbo].[CompletedTasks];

    -- Total number of outstanding tasks (not completed)
    SELECT COUNT(*) AS OutstandingTasks
    FROM [mmDailyPlanner].[dbo].[PlannerTasks]
    WHERE IsCompleted = 0;

    -- Total number of tasks approaching due date (e.g., due within the next 3 days)
    SELECT COUNT(*) AS ApproachingDueTasks
    FROM [mmDailyPlanner].[dbo].[PlannerTasks]
    WHERE IsCompleted = 0 AND DueDate BETWEEN GETDATE() AND DATEADD(DAY, 3, GETDATE());

    -- Total number of past due tasks
    SELECT COUNT(*) AS PastDueTasks
    FROM [mmDailyPlanner].[dbo].[PlannerTasks]
    WHERE IsCompleted = 0 AND DueDate < GETDATE();

    -- Average priority of outstanding tasks
    SELECT AVG(Priority) AS AveragePriority
    FROM [mmDailyPlanner].[dbo].[PlannerTasks]
    WHERE IsCompleted = 0;

    -- Average time to complete a task
    SELECT AVG(DATEDIFF(DAY, DateCreated, DateCompleted)) AS AverageCompletionTime
    FROM [mmDailyPlanner].[dbo].[CompletedTasks];

    -- Number of tasks per category
    SELECT Category, COUNT(*) AS NumberOfTasks
    FROM [mmDailyPlanner].[dbo].[PlannerTasks]
    GROUP BY Category;

    -- Number of completed tasks per category
    SELECT Category, COUNT(*) AS NumberOfCompletedTasks
    FROM [mmDailyPlanner].[dbo].[CompletedTasks]
    GROUP BY Category;

    -- Number of outstanding tasks per user
    SELECT UserId, COUNT(*) AS NumberOfOutstandingTasks
    FROM [mmDailyPlanner].[dbo].[PlannerTasks]
    WHERE IsCompleted = 0
    GROUP BY UserId;

    -- Number of completed tasks per user
    SELECT UserId, COUNT(*) AS NumberOfCompletedTasks
    FROM [mmDailyPlanner].[dbo].[CompletedTasks]
    GROUP BY UserId;

    -- Average completion time per user
    SELECT UserId, AVG(DATEDIFF(DAY, DateCreated, DateCompleted)) AS AverageCompletionTimePerUser
    FROM [mmDailyPlanner].[dbo].[CompletedTasks]
    GROUP BY UserId;

    -- Number of high-priority tasks (assuming Priority > 5 is high-priority)
    SELECT COUNT(*) AS HighPriorityTasks
    FROM [mmDailyPlanner].[dbo].[PlannerTasks]
    WHERE IsCompleted = 0 AND Priority > 5;

    -- Number of medium-priority tasks (assuming Priority between 3 and 5 is medium-priority)
    SELECT COUNT(*) AS MediumPriorityTasks
    FROM [mmDailyPlanner].[dbo].[PlannerTasks]
    WHERE IsCompleted = 0 AND Priority BETWEEN 3 AND 5;

    -- Number of low-priority tasks (assuming Priority < 3 is low-priority)
    SELECT COUNT(*) AS LowPriorityTasks
    FROM [mmDailyPlanner].[dbo].[PlannerTasks]
    WHERE IsCompleted = 0 AND Priority < 3;

    -- Distribution of tasks by priority level
    SELECT 
        CASE 
            WHEN Priority > 5 THEN 'High'
            WHEN Priority BETWEEN 3 AND 5 THEN 'Medium'
            ELSE 'Low'
        END AS PriorityLevel,
        COUNT(*) AS TaskCount
    FROM [mmDailyPlanner].[dbo].[PlannerTasks]
    WHERE IsCompleted = 0
    GROUP BY 
        CASE 
            WHEN Priority > 5 THEN 'High'
            WHEN Priority BETWEEN 3 AND 5 THEN 'Medium'
            ELSE 'Low'
        END;

    -- Distribution of tasks by completion status
    SELECT 
        CASE 
            WHEN IsCompleted = 1 THEN 'Completed'
            ELSE 'Outstanding'
        END AS CompletionStatus,
        COUNT(*) AS TaskCount
    FROM [mmDailyPlanner].[dbo].[PlannerTasks]
    GROUP BY 
        CASE 
            WHEN IsCompleted = 1 THEN 'Completed'
            ELSE 'Outstanding'
        END;

    -- Distribution of tasks by category
    SELECT Category, COUNT(*) AS TaskCount
    FROM [mmDailyPlanner].[dbo].[PlannerTasks]
    GROUP BY Category;

    -- Distribution of tasks by user
    SELECT UserId, COUNT(*) AS TaskCount
    FROM [mmDailyPlanner].[dbo].[PlannerTasks]
    GROUP BY UserId;

    -- Percentage of tasks completed on time vs. late
    SELECT 
        CASE 
            WHEN DateCompleted <= DueDate THEN 'OnTime'
            ELSE 'Late'
        END AS CompletionTimeliness,
        COUNT(*) * 100.0 / (SELECT COUNT(*) FROM [mmDailyPlanner].[dbo].[CompletedTasks]) AS Percentage
    FROM [mmDailyPlanner].[dbo].[CompletedTasks]
    GROUP BY 
        CASE 
            WHEN DateCompleted <= DueDate THEN 'OnTime'
            ELSE 'Late'
        END;
END
GO
