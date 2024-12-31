// Infrastructure/Persistence/DbContexts/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Todo.Entities;

namespace TodoApp.Infrastructure.Persistence.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
