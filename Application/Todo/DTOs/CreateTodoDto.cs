using System.ComponentModel.DataAnnotations;

namespace TodoApp.Application.Todo.DTOs
{
    public class CreateTodoDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public required string Description { get; set; }

        public CreateTodoDto(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}
