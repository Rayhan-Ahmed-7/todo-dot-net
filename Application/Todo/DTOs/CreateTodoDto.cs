namespace TodoApp.Application.Todo.DTOs
{
    public class CreateTodoDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
    public CreateTodoDto(string title, string description)
    {
        Title = title;
        Description = description;
    }
    }
}
