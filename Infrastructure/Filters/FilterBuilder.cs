using TodoApp.Domain.Todo.Entities;

namespace TodoApp.Infrastructure.Filters
{
    public static class FilterBuilder
    {
        public static IQueryable<Todo> BuildFilter(IQueryable<Todo> query, string? title, string? description, bool? isCompleted)
        {
            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(todo => todo.Title.Contains(title));
            }

            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(todo => todo.Description.Contains(description));
            }

            if (isCompleted.HasValue)
            {
                query = query.Where(todo => todo.IsCompleted == isCompleted.Value);
            }

            return query;
        }
    }
}
