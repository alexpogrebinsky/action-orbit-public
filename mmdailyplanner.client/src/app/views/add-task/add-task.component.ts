import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskService } from '../../services/task-service/task.service';
import { AuthService } from '../../services/auth-service/auth.service';
import { PRIORITY_CONFIG, CAT_CONFIG } from '../../../config';
import { ERROR_MESSAGES, INFO_MESSAGES } from '../../../messages';

@Component({
  selector: 'app-add-task',
  templateUrl: './add-task.component.html',
  styleUrls: ['./add-task.component.css']
})
export class AddTaskComponent implements OnInit {
  taskForm!: FormGroup;
  errorMessage: string = '';
  priorities = PRIORITY_CONFIG.levels;
  categories = CAT_CONFIG.categories;
  subcategories: string[] = [];
  selectedCategory: string = '';
  selectedSubcategory: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private taskService: TaskService,
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.taskForm = this.formBuilder.group({
      title: ['', [Validators.required, Validators.maxLength(100)]],
      category: ['', Validators.required],
      subcategory: [''],
      description: ['', [Validators.required, Validators.maxLength(500)]],
      priority: [1, [Validators.required, Validators.min(1), Validators.max(10)]],
      dueDate: [new Date(), Validators.required]
    });
  }

  onCategoryChange(category: string): void {
    this.selectedCategory = category;
    this.subcategories = this.getSubcategories();
  }

  async addTask(): Promise<void> {
    if (this.taskForm.invalid) {
      return;
    }

    const task = {
      ...this.taskForm.value,
      category: this.selectedCategory && this.selectedSubcategory
        ? `${this.selectedCategory}: ${this.selectedSubcategory}`
        : this.selectedCategory
    };

    try {
      await this.taskService.addTask(task).toPromise();
      console.log(INFO_MESSAGES.taskAdded, task);
      this.router.navigate(['/tasks']);
      this.resetTaskForm();
    } catch (error) {
      this.handleError(error);
    }
  }

  private resetTaskForm(): void {
    this.taskForm.reset({
      title: '',
      category: '',
      subcategory: '',
      description: '',
      priority: 1,
      dueDate: new Date()
    });
    this.selectedCategory = '';
    this.selectedSubcategory = '';
    this.subcategories = [];
  }

  private handleError(error: any): void {
    this.errorMessage = error?.message ?? ERROR_MESSAGES.addTaskError;
    console.error(ERROR_MESSAGES.addTaskError, error);
  }

  getSubcategories(): string[] {
    const category = this.categories.find(cat => cat.name === this.selectedCategory);
    return category ? category.subcategories : [];
  }
}
