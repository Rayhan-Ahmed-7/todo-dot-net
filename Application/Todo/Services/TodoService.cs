using TodoApp.Application.Todo.Interfaces;
using TodoApp.Application.Todo.DTOs;
using AutoMapper;
using TodoApp.Infrastructure.Interfaces;

namespace TodoApp.Application.Todo.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IMapper _mapper;

        public TodoService(ITodoRepository todoRepository, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _mapper = mapper;
        }

        public async Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto)
        {
            if (string.IsNullOrEmpty(createTodoDto.Title))
                throw new ArgumentException("Title is required");

            return await _todoRepository.CreateTodoAsync(createTodoDto);
        }

        public async Task<TodoDto> GetTodoByIdAsync(int id)
        {
            return await _todoRepository.GetTodoByIdAsync(id);
        }

        public async Task<List<TodoDto>> GetAllTodosAsync(TodoQueryDto queryDto)
        {
            var todos = await _todoRepository.GetTodosAsync(queryDto);
            return _mapper.Map<List<TodoDto>>(todos);
        }

        public async Task<TodoDto> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto)
        {
            return await _todoRepository.UpdateTodoAsync(id, updateTodoDto);
        }

        public async Task DeleteTodoAsync(int id)
        {
            await _todoRepository.DeleteTodoAsync(id);
        }
    }
}
