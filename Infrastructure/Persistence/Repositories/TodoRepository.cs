using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Todo.DTOs;
using TodoApp.Application.Todo.Interfaces;
using TodoApp.Domain.Todo.Entities;
using TodoApp.Infrastructure.Persistence.DbContexts;

namespace TodoApp.Infrastructure.Repositories
{
    public class TodoRepository : ITodoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TodoRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto)
        {
            // Map DTO to entity
            var todoEntity = _mapper.Map<Todo>(createTodoDto);

            // Save to the database
            await _context.Todos.AddAsync(todoEntity);
            await _context.SaveChangesAsync();

            // Map entity to DTO and return
            return _mapper.Map<TodoDto>(todoEntity);
        }

        public async Task<TodoDto> GetTodoByIdAsync(int id)
        {
            var todoEntity = await _context.Todos.FindAsync(id);
            if (todoEntity == null)
            {
                throw new KeyNotFoundException("Todo not found."); // Or throw a custom exception
            }

            return _mapper.Map<TodoDto>(todoEntity);
        }

        public async Task<List<Todo>> GetTodosAsync(TodoQueryDto queryDto)
        {
            IQueryable<Todo> query = _context.Todos;

            // Apply Title filter if provided
            if (!string.IsNullOrEmpty(queryDto.Title))
            {
                query = query.Where(todo => todo.Title.Contains(queryDto.Title));
            }

            // Apply Description filter if provided
            if (!string.IsNullOrEmpty(queryDto.Description))
            {
                query = query.Where(todo => todo.Description.Contains(queryDto.Description));
            }

            // Apply IsCompleted filter if provided
            if (queryDto.IsCompleted.HasValue)
            {
                query = query.Where(todo => todo.IsCompleted == queryDto.IsCompleted.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<TodoDto> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto)
        {
            var todoEntity = await _context.Todos.FindAsync(id);
            if (todoEntity == null)
            {
                throw new KeyNotFoundException("Todo not found."); // Or throw a custom exception
            }

            // Map updated fields from DTO to entity
            _mapper.Map(updateTodoDto, todoEntity);

            _context.Todos.Update(todoEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TodoDto>(todoEntity);
        }

        public async Task DeleteTodoAsync(int id)
        {
            var todoEntity = await _context.Todos.FindAsync(id);
            if (todoEntity != null)
            {
                _context.Todos.Remove(todoEntity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
