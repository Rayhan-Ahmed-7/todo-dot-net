using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.Persistence.DbContexts;
using TodoApp.Application.Todo.Mappings;
using TodoApp.Infrastructure.Middleware;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Infrastructure.Interfaces;
using TodoApp.Infrastructure.Repositories;
using TodoApp.Application.Todo.Interfaces;
using TodoApp.Application.Todo.Services;
using TodoApp.Application.Employees.Interfaces;
using TodoApp.Application.Employees.Services;
using TodoApp.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Configure the DbContext to use MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlConnectionString"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySqlConnectionString"))
    )
);
// Register DinkToPdf services
builder.Services.AddPdfService();
// Register application services
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IGenerateEmployeeIdCardService, GenerateEmployeeIdCardService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
// Add Authorization
builder.Services.AddAuthorization();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(TodoMappingProfile)); // Register a single profile
// builder.Services.AddAutoMapper(typeof(TodoMappingProfile), typeof(GoalMappingProfile));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
// Add controllers with structured JSON response
builder.Services.AddControllers();
// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "TodoApp API",
        Version = "v1",
        Description = "API documentation for the TodoApp."
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<CustomResponseMiddleware>();

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Use Authorization Middleware
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();
