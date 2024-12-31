using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Domain.Todo.Entities;

namespace TodoApp.Infrastructure.Persistence.Configurations
{
    public class TodoConfiguration : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(t => t.Description)
                .IsRequired();
            builder.Property(t => t.IsCompleted)
                .HasDefaultValue(false);
            builder.Property(t => t.CreatedAt)
                .ValueGeneratedOnAdd(); // Let EF handle the value generation
        }
    }
}
