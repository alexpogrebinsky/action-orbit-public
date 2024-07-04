import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { TaskService } from '../../services/task-service/task.service';
import { AuthService } from '../../services/auth-service/auth.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { CompletedTaskDTO } from '../../dtos/completed-task.dto';
import { ERROR_MESSAGES, INFO_MESSAGES } from '../../../messages';

@Component({
  selector: 'app-completed-task-list',
  templateUrl: './completed-task-list.component.html',
  styleUrls: ['./completed-task-list.component.css']
})
export class CompletedTaskListComponent implements OnInit, OnDestroy {
  tasks!: MatTableDataSource<CompletedTaskDTO>;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  errorMessage: string = '';
  displayedColumns: string[] = ['title', 'category', 'description', 'dueDate', 'priority', 'dateCreated', 'dateModified', 'isCompleted'];
  task: any = {};
  private unsubscribe$ = new Subject<void>();

  constructor(
    private taskService: TaskService,
    private authService: AuthService
  ) { }

  async ngOnInit(): Promise<void> {
    this.taskService.getCompletedTasks().pipe(takeUntil(this.unsubscribe$)).subscribe(
      (response: any) => {
        let data = response.data;
        console.log(INFO_MESSAGES.dataReceived, data);

        if (!Array.isArray(data)) {
          data = [];
          console.error(ERROR_MESSAGES.notArrayError, data);
          this.errorMessage = ERROR_MESSAGES.notArrayError;
        }

        this.tasks = new MatTableDataSource(data);
        this.tasks.sort = this.sort;
        this.tasks.paginator = this.paginator;
      },
      error => {
        this.errorMessage = ERROR_MESSAGES.fetchError;
        console.error(ERROR_MESSAGES.fetchError, error);
      },
      () => {
        console.log(INFO_MESSAGES.getCompletedTasksCompleted);
      }
    );
  }

  completeTask(taskId: number): void {
    this.taskService.markTaskAsCompleted(taskId).pipe(takeUntil(this.unsubscribe$)).subscribe(
      () => {
        const data = this.tasks.data;
        const task = data.find((task) => task.id === taskId);
        if (task) {
          task.isCompleted = true;
        }
        this.updateTableData(data);
      },
      error => {
        this.errorMessage = ERROR_MESSAGES.completeOrDeleteError;
        console.error(ERROR_MESSAGES.completeOrDeleteError, error);
      }
    );
  }

  deleteTask(taskId: number): void {
    this.taskService.deleteTask(taskId).pipe(takeUntil(this.unsubscribe$)).subscribe(
      () => {
        const data = this.tasks.data.filter((task) => task.id !== taskId);
        this.updateTableData(data);
      },
      error => {
        this.errorMessage = ERROR_MESSAGES.completeOrDeleteError;
        console.error(ERROR_MESSAGES.completeOrDeleteError, error);
      }
    );
  }

  updateTableData(data: CompletedTaskDTO[]): void {
    if (Array.isArray(data)) {
      this.tasks = new MatTableDataSource(data);
      this.tasks.sort = this.sort;
      this.tasks.paginator = this.paginator;
    } else {
      console.error(ERROR_MESSAGES.updateDataFormatError, data);
      this.errorMessage = ERROR_MESSAGES.updateDataFormatError;
    }
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
