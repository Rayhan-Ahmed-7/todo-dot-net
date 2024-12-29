using TodoApp.Domain.Todo.Entities;
using TodoApp.Application.Todo.Interfaces;
using TodoApp.Infrastructure.Persistence.DbContexts;

namespace TodoApp.Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ApplicationDbContext _context;

        public TodoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Todo> CreateAsync(Todo todo)
        {
            await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<Todo> GetByIdAsync(int id)
        {
            return await _context.Todos.FindAsync(id);
        }

        public async Task<List<Todo>> GetAllAsync()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<Todo> UpdateAsync(Todo todo)
        {
            _context.Todos.Update(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task DeleteAsync(int id)
        {
            var todo = await GetByIdAsync(id);
            if (todo != null)
            {
                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
