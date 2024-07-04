export interface CompletedTaskDTO {
  id: number;
  title: string;
  description: string;
  priority: number;
  dateCreated: Date;
  dateModified: Date;
  dueDate: Date;
  dateCompleted: Date;
  category: string;
  isCompleted: boolean;
}
