export interface AddTaskDTO {
  id: number,
  title: string,
  description: string,
  priority: number,
  dueDate: Date,
  category: string
}
