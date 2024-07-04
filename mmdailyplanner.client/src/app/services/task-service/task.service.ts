import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_CONFIG } from '../../../config';
import { TaskListDTO } from '../../dtos/task-list.dto';
import { TaskDetailDTO } from '../../dtos/task-detail.dto';
import { AddTaskDTO } from '../../dtos/add-task.dto';
import { CompletedTaskDTO } from '../../dtos/completed-task.dto';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  constructor(private http: HttpClient) { }

  getTasks(): Observable<TaskListDTO[]> {
    return this.http.get<TaskListDTO[]>(API_CONFIG.getTasks, { withCredentials: true });
  }

  getTaskById(id: number): Observable<TaskDetailDTO> {
    return this.http.get<TaskDetailDTO>(API_CONFIG.getTaskByTaskId + `${id}`, { withCredentials: true });
  }

  addTask(task: any): Observable<AddTaskDTO> {
    return this.http.post<AddTaskDTO>(API_CONFIG.addTask, task, { withCredentials: true });
  }

  updateTask(id: number, task: TaskDetailDTO): Observable<any> {
    return this.http.put(API_CONFIG.updateTask + `${id}`, task, { withCredentials: true });
  }

  deleteTask(taskId: number): Observable<any> {
    return this.http.delete(API_CONFIG.deleteTask + `${taskId}`, { withCredentials: true });
  }

  markTaskAsCompleted(taskId: number): Observable<any> {
    return this.http.put(API_CONFIG.markTaskCompleted + `${taskId}`, { withCredentials: true });
  }

  getCompletedTasks(): Observable<CompletedTaskDTO[]>{
    return this.http.get<CompletedTaskDTO[]>(API_CONFIG.getCompletedTasks, { withCredentials: true });
  }

  getUserTaskAnalytics(): Observable<any> {
    return this.http.get(API_CONFIG.userInsights, { withCredentials: true });
  }
}
