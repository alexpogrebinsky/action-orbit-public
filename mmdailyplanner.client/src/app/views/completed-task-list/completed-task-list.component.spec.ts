import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompletedTaskListComponent } from './completed-task-list.component';

describe('CompletedTaskListComponent', () => {
  let component: CompletedTaskListComponent;
  let fixture: ComponentFixture<CompletedTaskListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompletedTaskListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompletedTaskListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
