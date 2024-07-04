import { Component, OnInit, ViewChild } from '@angular/core';
import { TaskService } from '../../services/task-service/task.service';
import { AuthService } from '../../services/auth-service/auth.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { ChangeDetectorRef } from '@angular/core';
import { TaskListDTO } from '../../dtos/task-list.dto';
import { Observable } from 'rxjs';
import { COLORS_CONFIG, PRIORITY_CONFIG, CAT_CONFIG } from '../../../config';
import { ERROR_MESSAGES } from '../../../messages';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html'
})
export class TaskListComponent implements OnInit {
  tasks!: MatTableDataSource<TaskListDTO>;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  errorMessage: string = '';
  displayedColumns: string[] = ['title', 'category', 'description', 'dueDate', 'priority', 'dateCreated', 'isCompleted', 'actions'];
  task: TaskListDTO = {
    id: 0,
    title: '',
    description: '',
    priority: 0,
    dateCreated: new Date(),
    dueDate: new Date(),
    category: '',
    isCompleted: false
  };

  constructor(
    private taskService: TaskService,
    private authService: AuthService,
    private changeDetectorRefs: ChangeDetectorRef
  ) { }

  async ngOnInit(): Promise<void> {
    await this.getTasks();
  }

  private getTasks(): void {
    this.taskService.getTasks().subscribe(
      (data: TaskListDTO[]) => {
        if (Array.isArray(data)) { 
          this.tasks = new MatTableDataSource(data);
          this.tasks.sort = this.sort;
          this.tasks.paginator = this.paginator;
        } else {
          this.errorMessage = ERROR_MESSAGES.dataFormatError;
          console.error(ERROR_MESSAGES.dataFormatError, data);
        }
      },
      error => {
        this.errorMessage = ERROR_MESSAGES.fetchError;
        console.error(ERROR_MESSAGES.fetchError, error);
      }
    );
  }

  completeOrDeleteTask(operation: (taskId: number) => Observable<any>, taskId: number): void {
    operation(taskId).subscribe(
      () => {
        const data = this.tasks.data;
        this.refreshTable(data.filter((task) => task.id !== taskId));
      },
      error => {
        this.errorMessage = ERROR_MESSAGES.completeOrDeleteError;
        console.error(ERROR_MESSAGES.completeOrDeleteError, error);
      }
    );
  }

  completeTask(taskId: number): void {
    this.completeOrDeleteTask(this.taskService.markTaskAsCompleted.bind(this.taskService), taskId);
  }

  deleteTask(taskId: number): void {
    this.completeOrDeleteTask(this.taskService.deleteTask.bind(this.taskService), taskId);
  }

  getRowBackgroundColor(task: any): string {
    const priority = parseInt(task.priority, 10);
    return COLORS_CONFIG.priorityColors[priority] || 'transparent';
  }

  private refreshTable(data: TaskListDTO[]): void {
    this.tasks = new MatTableDataSource(data);
    this.tasks.sort = this.sort;
    this.tasks.paginator = this.paginator;
    this.changeDetectorRefs.detectChanges();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.tasks.filter = filterValue.trim().toLowerCase();

    if (this.tasks.paginator) {
      this.tasks.paginator.firstPage();
    }
  }
}
