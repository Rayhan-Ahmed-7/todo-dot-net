using TodoApp.Application.Todo.DTOs;

namespace TodoApp.Application.Todo.Interfaces
{
    public interface ITodoRepository
    {
        Task<TodoDto> GetTodoByIdAsync(int id);
        Task<List<TodoDto>> GetTodosAsync(TodoQueryDto queryDto);
        Task CreateTodoAsync(CreateTodoDto todo);
        Task UpdateTodoAsync(TodoDto todo);
        Task DeleteTodoAsync(int id);
    }
}
