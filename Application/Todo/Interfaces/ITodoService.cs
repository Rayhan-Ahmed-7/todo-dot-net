using TodoApp.Application.Todo.DTOs;

namespace TodoApp.Application.Todo.Interfaces
{
    public interface ITodoService
    {
        Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto);
        Task<TodoDto> GetTodoByIdAsync(int id);
        Task<List<TodoDto>> GetAllTodosAsync(TodoQueryDto queryDto);
        Task<TodoDto> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto);
        Task DeleteTodoAsync(int id);
    }
}
