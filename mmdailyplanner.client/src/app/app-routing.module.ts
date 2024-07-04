import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from '../app/views/login/login.component';
import { RegisterComponent } from '../app/views/register/register.component';
import { TaskListComponent } from '../app/views/task-list/task-list.component';
import { TaskDetailComponent } from '../app/views/task-detail/task-detail.component';
import { AuthService } from './services/auth-service/auth.service';
import { AppComponent } from './app.component';
import { AuthGuard } from './guards/auth.guard';
import { AddTaskComponent } from './views/add-task/add-task.component';
import { ProfileComponent } from './views/profile/profile.component';
import { CompletedTaskListComponent } from './views/completed-task-list/completed-task-list.component';
import {UserAnalyticsComponent} from './views/user-analytics/user-analytics.component'; 
import { ProfileDropdownWidgetComponent } from './shared/profile-dropdown-widget/profile-dropdown-widget.component';
import { AboutComponent } from './views/about/about.component';
import { TermsOfServiceComponent } from './views/pages/terms-of-service/terms-of-service.component';
import { PrivacyPolicyComponent } from './views/pages/privacy-policy/privacy-policy.component';
import { ContactComponent } from './views/pages/contact/contact.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'tasks', component: TaskListComponent },
  { path: 'tasks/:id', component: TaskDetailComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'add-task', component: AddTaskComponent },
  { path: 'profile/:username', component: ProfileComponent },
  { path: 'completed-tasks', component: CompletedTaskListComponent },
  { path: 'user-analytics', component: UserAnalyticsComponent },
  { path: 'app', component: AppComponent },
  { path: 'profile-dropdown-widget', component: ProfileDropdownWidgetComponent },
  { path: 'about', component: AboutComponent },
  { path: 'terms-of-service', component: TermsOfServiceComponent },
  { path: 'privacy-policy', component: PrivacyPolicyComponent },
  { path: 'contact', component: ContactComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
