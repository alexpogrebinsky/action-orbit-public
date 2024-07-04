import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './views/login/login.component';
import { RegisterComponent } from './views/register/register.component';
import { TaskListComponent } from './views/task-list/task-list.component';
import { TaskDetailComponent } from './views/task-detail/task-detail.component';
import { AuthGuard } from './guards/auth.guard';
import {AuthService } from './services/auth-service/auth.service';
import { TaskService } from './services/task-service/task.service';

// Angular Material Components
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { AddTaskComponent } from './views/add-task/add-task.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { ProfileComponent } from './views/profile/profile.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CompletedTaskListComponent } from './views/completed-task-list/completed-task-list.component';
import { MatSelectModule } from '@angular/material/select';
import { HeaderComponent } from './views/header/header.component';
import { FooterComponent } from './views/footer/footer.component';
import { UserAnalyticsComponent } from './views/user-analytics/user-analytics.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatCheckboxModule } from '@angular/material/checkbox';

//charts
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { ProfileDropdownWidgetComponent } from './shared/profile-dropdown-widget/profile-dropdown-widget.component';
import { AboutComponent } from './views/about/about.component';
import { PrivacyPolicyComponent } from './views/pages/privacy-policy/privacy-policy.component';
import { TermsOfServiceComponent } from './views/pages/terms-of-service/terms-of-service.component';
import { ContactComponent } from './views/pages/contact/contact.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    TaskListComponent,
    TaskDetailComponent,
    AddTaskComponent,
    ProfileComponent,
    CompletedTaskListComponent,
    HeaderComponent,
    FooterComponent,
    UserAnalyticsComponent,
    ProfileDropdownWidgetComponent,
    AboutComponent,
    PrivacyPolicyComponent,
    TermsOfServiceComponent,
    ContactComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    MatInputModule,
    MatCardModule,
    MatFormFieldModule,
    MatButtonModule,
    MatToolbarModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatIconModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    ReactiveFormsModule,
    NgxChartsModule,
    MatSidenavModule,
    MatCheckboxModule
  ],
  providers: [AuthGuard, AuthService, TaskService],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
