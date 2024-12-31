using System.ComponentModel.DataAnnotations;

namespace TodoApp.Domain.Todo.Entities
{
    public class Todo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public required string Description { get; set; }

        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        public Todo(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}
