using AutoMapper;
using TodoApp.Application.Todo.DTOs;          // Your DTOs namespace
using TodoApp.Domain.Todo.Entities;           // Your entities namespace

namespace TodoApp.Application.Todo.Mappings
{
    public class TodoMappingProfile : Profile
    {
        public TodoMappingProfile()
        {
            // Create map between CreateTodoDto and Todo (Entity)
            CreateMap<CreateTodoDto, TodoApp.Domain.Todo.Entities.Todo>();

            // Create map between Todo (Entity) and TodoDto
            CreateMap<TodoApp.Domain.Todo.Entities.Todo, TodoDto>();

            // Create map between UpdateTodoDto and Todo (Entity)
            CreateMap<UpdateTodoDto, TodoApp.Domain.Todo.Entities.Todo>();
        }
    }
}
