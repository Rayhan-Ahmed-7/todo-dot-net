using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Todo.Interfaces;
using TodoApp.Application.Todo.DTOs;

namespace TodoApp.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpPost]
        public async Task<ActionResult<TodoDto>> Create(CreateTodoDto createTodoDto)
        {
            var todo = await _todoService.CreateTodoAsync(createTodoDto);
            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoDto>> GetById(int id)
        {
            var todo = await _todoService.GetTodoByIdAsync(id);
            if (todo == null) return NotFound();
            return todo;
        }

        [HttpGet]
        public async Task<ActionResult<List<TodoDto>>> GetAll()
        {
            var todos = await _todoService.GetAllTodosAsync();
            return Ok(todos);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TodoDto>> Update(int id, UpdateTodoDto updateTodoDto)
        {
            var updatedTodo = await _todoService.UpdateTodoAsync(id, updateTodoDto);
            return Ok(updatedTodo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _todoService.DeleteTodoAsync(id);
            return NoContent();
        }
    }
}
