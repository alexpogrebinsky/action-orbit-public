using mmDailyPlanner.Server.Repositories;
using Microsoft.EntityFrameworkCore;
using mmDailyPlanner.Server.Data;
using mmDailyPlanner.Server;
using Microsoft.Extensions.Caching.Memory;
using mmDailyPlanner.Server.Services;
using mmDailyPlanner.Server.Services.TaskService;
using System.Reflection;
using mmDailyPlanner.Server.Services.AuthService;
using Serilog.Events;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders().AddSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowTasker",
        builder =>
        {
            builder.WithOrigins("https://localhost:4200") // replace with your Angular app URL
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials(); // Allow credentials

        });
});


// Add DbContext and configure it to use SQL Server
builder.Services.AddDbContext<DailyPlannerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


// Register your repositories
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMemoryCache,  MemoryCache>();
builder.Services.AddScoped<IPasswordService,  PasswordService>();
builder.Services.AddScoped<IStoredProcedureExecutor, StoredProcedureExecutor>();
builder.Services.AddScoped<IStoredProcedureExecutorFactory,  StoredProcedureExecutorFactory>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

//
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/tracker_logs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


// Use CORS policy
app.UseCors("AllowTasker");

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
Log.CloseAndFlush();