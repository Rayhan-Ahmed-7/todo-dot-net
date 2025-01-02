using TodoApp.Application.Todo.DTOs;

namespace TodoApp.Infrastructure.Interfaces
{
    public interface ITodoRepository
    {
        Task<TodoDto> GetTodoByIdAsync(int id);
        Task<List<TodoDto>> GetTodosAsync(TodoQueryDto queryDto);  // Changed to List<Todo> to interact with the database
        Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto);
        Task<TodoDto> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto);
        Task DeleteTodoAsync(int id);
    }
}
