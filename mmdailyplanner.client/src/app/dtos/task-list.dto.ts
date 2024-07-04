export interface TaskListDTO {
  id: number;
  title: string;
  description: string;
  priority: number;
  dateCreated: Date;
  dueDate: Date;
  category: string;
  isCompleted: boolean;
}
