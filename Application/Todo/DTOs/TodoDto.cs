namespace TodoApp.Application.Todo.DTOs
{
    public class TodoDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
    }
}
