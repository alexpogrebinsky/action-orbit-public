import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskService } from '../../services/task-service/task.service';
import { TaskDetailDTO } from '../../dtos/task-detail.dto';
import { PRIORITY_CONFIG, CAT_CONFIG } from '../../../config'; 
import { ERROR_MESSAGES } from '../../../messages'; 

@Component({
  selector: 'app-task-detail',
  templateUrl: './task-detail.component.html',
  styleUrls: ['./task-detail.component.css']
})
export class TaskDetailComponent implements OnInit {
  task: TaskDetailDTO = {} as TaskDetailDTO;
  id: number = 0;
  errorMessage: string = '';
  priorities = PRIORITY_CONFIG.levels;
  categories = CAT_CONFIG.categories;
  selectedCategory: string = '';
  selectedSubcategory: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private taskService: TaskService
  ) { }

  async ngOnInit(): Promise<void> {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = +idParam;
      try {
        const task = await this.taskService.getTaskById(this.id).toPromise();
        if (task) {
          this.task = task;
          const categoryParts = this.task.category.split(': ');
          this.selectedCategory = categoryParts[0];
          this.selectedSubcategory = categoryParts[1] || '';
        } else {
          throw new Error(ERROR_MESSAGES.taskNotFound);
        }
      } catch (error) {
        this.handleError(error);
      }
    } else {
      this.errorMessage = ERROR_MESSAGES.invalidTaskId;
      console.error(ERROR_MESSAGES.invalidTaskId);
    }
  }

  async save(): Promise<void> {
    this.task.category = this.selectedCategory && this.selectedSubcategory ? `${this.selectedCategory}: ${this.selectedSubcategory}` : this.selectedCategory;

    try {
      await this.taskService.updateTask(this.id, this.task).toPromise();
      this.router.navigate(['/tasks']);
    } catch (error) {
      this.handleError(error);
    }
  }

  onCategoryChange(category: string): void {
    this.selectedCategory = category;
    this.selectedSubcategory = '';
  }

  getSubcategories(): string[] {
    const category = this.categories.find(cat => cat.name === this.selectedCategory);
    return category ? category.subcategories : [];
  }

  private handleError(error: unknown): void {
    this.errorMessage = ERROR_MESSAGES.updateTaskError;
    if (error instanceof Error) {
      this.errorMessage = error.message;
    }
    console.error(this.errorMessage, error);
  }
}
