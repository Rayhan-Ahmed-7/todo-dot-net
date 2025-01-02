using TodoApp.Application.Todo.Interfaces;
using TodoApp.Application.Todo.DTOs;
using AutoMapper;

namespace TodoApp.Application.Todo.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoService _todoRepository;
        private readonly IMapper _mapper;

        public TodoService(ITodoService todoRepository, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _mapper = mapper;
        }

        public async Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto)
        {
            // Validate the input DTO before passing to the repository
            if (string.IsNullOrEmpty(createTodoDto.Title))
                throw new ArgumentException("Title is required");

            // Pass to repository for creation
            return await _todoRepository.CreateTodoAsync(createTodoDto);
        }

        public async Task<TodoDto> GetTodoByIdAsync(int id)
        {
            // Fetch Todo from repository
            return await _todoRepository.GetTodoByIdAsync(id);
        }

        public async Task<List<TodoDto>> GetAllTodosAsync(TodoQueryDto queryDto)
        {
            var todos = await _todoRepository.GetAllTodosAsync(queryDto);
            return _mapper.Map<List<TodoDto>>(todos);
        }

        public async Task<TodoDto> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto)
        {
            // Validate and update Todo
            return await _todoRepository.UpdateTodoAsync(id, updateTodoDto);
        }

        public async Task DeleteTodoAsync(int id)
        {
            // Delete Todo
            await _todoRepository.DeleteTodoAsync(id);
        }
    }
}
