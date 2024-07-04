export const API_CONFIG = {
  login: '/api/auth/login',
  register: '/api/auth/register',
  isAuthenticated: '/api/auth/isAuthenticated',
  getUser: '/api/auth/get-user',
  getCurrentUserId: '/api/auth/current-user',
  logout: '/api/auth/logout',
  getTasks: '/api/task/get-tasks',
  getTaskByTaskId: '/api/task/get-task-by-taskId/',
  addTask: '/api/task/add-task',
  updateTask: '/api/task/update-task/',
  deleteTask: '/api/task/delete-task/',
  markTaskCompleted: '/api/task/mark-task-completed/',
  getCompletedTasks: '/api/task/get-completed-tasks',
  userInsights: '/api/task/task-insights'
};

export const NAME_CONFIG = {
  appName: 'TaskPlanner',
  appTitle: 'Task Planner',
  appDescription: 'A well-functioning boilerplate of Angular front-end, C# backend, SQL Server database, and original authentication.'
};

export const COLORS_CONFIG: { priorityColors: { [key: number]: string } } = {
  priorityColors: {
    1: '#6ec7ec',
    2: '#7dcbeb',
    3: '#8dcfeb',
    4: '#b2daeb',
    5: '#cde3ec',
    6: '#dae7ec',
    7: '#d0dee4',
    8: '#bcd3dc',
    9: '#b5d1dd',
    10: '#b1d2e0'
  }
};

export const PRIORITY_CONFIG = {
  levels: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] 
};

export const CAT_CONFIG = {
  categories: [
    { name: 'Work', subcategories: ['Meetings', 'Projects', 'Emails', 'Reports', 'Deadlines'] },
    { name: 'Personal Development', subcategories: ['Courses', 'Workshops', 'Certifications', 'Reading', 'Skill Practice'] },
    { name: 'Health & Fitness', subcategories: ['Exercise', 'Diet', 'Medical Appointments', 'Mental Health', 'Sleep'] },
    { name: 'Home Maintenance', subcategories: ['Cleaning', 'Repairs', 'Gardening', 'Organization', 'Renovations'] },
    { name: 'Family', subcategories: ['Quality Time', 'Activities', 'Birthdays', 'Family Meetings', 'Support'] },
    { name: 'Social', subcategories: ['Friends', 'Networking', 'Parties', 'Social Media', 'Community Events'] },
    { name: 'Finance', subcategories: ['Budgeting', 'Bills', 'Investments', 'Savings', 'Taxes'] },
    { name: 'Travel', subcategories: ['Planning', 'Packing', 'Reservations', 'Sightseeing', 'Itineraries'] },
    { name: 'Hobbies', subcategories: ['Sports', 'Arts & Crafts', 'Music', 'Collecting', 'Gaming'] },
    { name: 'Education', subcategories: ['Classes', 'Homework', 'Research', 'Study Sessions', 'Exams'] },
    { name: 'Errands', subcategories: ['Grocery Shopping', 'Bank', 'Post Office', 'Dry Cleaning', 'Car Maintenance'] },
    { name: 'Shopping', subcategories: ['Groceries', 'Clothes', 'Gifts', 'Online Shopping', 'Household Items'] },
    { name: 'Technology', subcategories: ['Software Updates', 'Device Maintenance', 'Learning New Tools', 'Troubleshooting', 'Backups'] },
    { name: 'Creativity', subcategories: ['Writing', 'Drawing', 'Painting', 'Photography', 'Crafting'] },
    { name: 'Volunteering', subcategories: ['Community Service', 'Fundraising', 'Mentoring', 'Event Planning', 'Donations'] },
    { name: 'Self-Care', subcategories: ['Relaxation', 'Spa Days', 'Meditation', 'Journaling', 'Personal Time'] },
    { name: 'Chores', subcategories: ['Laundry', 'Dishes', 'Trash', 'Vacuuming', 'Dusting'] },
    { name: 'Appointments', subcategories: ['Doctor', 'Dentist', 'Haircut', 'Therapist', 'Consultant'] },
    { name: 'Reading', subcategories: ['Books', 'Articles', 'Research Papers', 'Magazines', 'Blogs'] },
    { name: 'Writing', subcategories: ['Journaling', 'Essays', 'Stories', 'Poems', 'Letters'] },
    { name: 'Meal Planning', subcategories: ['Breakfast', 'Lunch', 'Dinner', 'Snacks', 'Meal Prep'] },
    { name: 'Pet Care', subcategories: ['Feeding', 'Grooming', 'Vet Visits', 'Walks', 'Training'] },
    { name: 'Events', subcategories: ['Birthdays', 'Weddings', 'Anniversaries', 'Conferences', 'Holidays'] },
    { name: 'Goals', subcategories: ['Short-term', 'Long-term', 'Career', 'Personal', 'Financial'] },
    { name: 'Miscellaneous', subcategories: ['To-Do List', 'Reminders', 'Notes', 'Ideas', 'Random Tasks'] }
  ]
};


export const CHART_CONFIG = {
  chartTypeMap: {
    completedTasksPerCategoryJson: 'bar-vertical',
    taskCompletionTimelinessJson: 'pie-chart',
    tasksByCompletionStatusJson: 'pie-chart',
    tasksByPriorityLevelJson: 'bar-vertical',
    tasksPerCategoryJson: 'bar-vertical'
  },
  readableNamesMap: {
    completedTasksPerCategoryJson: 'Completed Tasks per Category',
    taskCompletionTimelinessJson: 'Task Completion Timeliness',
    tasksByCompletionStatusJson: 'Tasks by Completion Status',
    tasksByPriorityLevelJson: 'Tasks by Priority Level',
    tasksPerCategoryJson: 'Tasks per Category',
    totalTasks: 'Total Tasks',
    totalCompletedTasks: 'Total Completed Tasks',
    outstandingTasks: 'Outstanding Tasks',
    approachingDueTasks: 'Approaching Due Tasks',
    pastDueTasks: 'Past Due Tasks',
    averagePriority: 'Average Priority',
    averageCompletionTime: 'Average Completion Time (Hours)'
  },
  colorScheme: {
    domain: ['#5DADE2', '#2874A6', '#5499C7', '#85C1E9', '#2980B9', '#154360'],
    group: 'ordinal',
    selectable: true,
    name: 'Cool Blues'
  }
};
