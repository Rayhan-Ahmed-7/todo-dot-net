namespace TodoApp.Application.Todo.DTOs
{
    public class TodoQueryDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
