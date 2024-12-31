namespace TodoApp.Domain.Todo.Entities
{
    public class Todo
{
    public int Id { get; set; }
    public required string Title { get; set; }
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
