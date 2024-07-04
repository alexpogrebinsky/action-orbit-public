CREATE INDEX IDX_UserId ON [mmDailyPlanner].[dbo].[PlannerTasks] (UserId);
CREATE INDEX IDX_IsCompleted ON [mmDailyPlanner].[dbo].[PlannerTasks] (IsCompleted);
CREATE INDEX IDX_DueDate ON [mmDailyPlanner].[dbo].[PlannerTasks] (DueDate);
CREATE INDEX IDX_Category ON [mmDailyPlanner].[dbo].[PlannerTasks] (Category);
CREATE INDEX IDX_Priority ON [mmDailyPlanner].[dbo].[PlannerTasks] (Priority);
