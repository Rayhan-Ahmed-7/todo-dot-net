using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Todo.Interfaces;
using TodoApp.Application.Todo.DTOs;
using TodoApp.Application.Models;

namespace TodoApp.Presentation.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<TodoDto>> Create(CreateTodoDto createTodoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = await _todoService.CreateTodoAsync(createTodoDto);
            var successResponse = new ApiResponse<TodoDto>("Todo created successfully", StatusCodes.Status201Created, todo, null);
            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, successResponse);
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult<TodoDto>> GetById(int id)
        {
            var todo = await _todoService.GetTodoByIdAsync(id);
            if (todo == null) return NotFound();
            return todo;
        }

        [HttpGet("")]
        public async Task<ActionResult<List<TodoDto>>> GetAll([FromQuery] TodoQueryDto queryDto)
        {
            var todos = await _todoService.GetAllTodosAsync(queryDto);
            return Ok(todos);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<TodoDto>> Update(int id, UpdateTodoDto updateTodoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedTodo = await _todoService.UpdateTodoAsync(id, updateTodoDto);
            return Ok(updatedTodo);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _todoService.DeleteTodoAsync(id);
            return NoContent();
        }
    }
}
