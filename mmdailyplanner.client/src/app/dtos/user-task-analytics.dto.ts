export interface UserTaskAnalyticsDto {
  totalTasks: number;
  totalCompletedTasks: number;
  outstandingTasks: number;
  approachingDueTasks: number;
  pastDueTasks: number;
  averagePriority: number;
  averageCompletionTime: number;
  tasksPerCategoryJson: string;
  completedTasksPerCategoryJson: string;
  tasksByPriorityLevelJson: string;
  tasksByCompletionStatusJson: string;
  taskCompletionTimelinessJson: string;
  tasksPerCategory?: { [key: string]: number };
  completedTasksPerCategory?: { [key: string]: number };
  tasksByPriorityLevel?: { [key: string]: number };
  tasksByCompletionStatus?: { [key: string]: number };
  taskCompletionTimeliness?: { [key: string]: number };
}
