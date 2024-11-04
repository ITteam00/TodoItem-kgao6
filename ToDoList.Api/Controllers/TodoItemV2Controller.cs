using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Api.Services;
using ToDoList.Api.Models;
using TodoItems.Core.service;
using Swashbuckle.AspNetCore.Annotations;
using MongoDB.Driver;
using TodoItem.Infrastructure;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v2/[controller]")]
    [AllowAnonymous]
    public class TodoItemV2Controller : ControllerBase
    {
        private readonly ITodoItemService _toDoItemService;
        private readonly ILogger<TodoItemV2Controller> _logger;


        public TodoItemV2Controller(ITodoItemService toDoItemService, ILogger<TodoItemV2Controller> logger)
        {
            _toDoItemService = toDoItemService;
            _logger = logger;

        }


        [HttpPost]
        [ProducesResponseType(typeof(ToDoItemDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(
            Summary = "Create New Item",
            Description = "Create a new to-do item"
            )]

        public async Task<ActionResult<ToDoItemDto>> PostAsync([FromBody] ToDoItemCreateRequest toDoItemCreateRequest)
        {
            var toDoItemDto = new TodoItems.Core.Model.TodoItem
            {
                Description = toDoItemCreateRequest.Description,
                IsComplete = toDoItemCreateRequest.Done,
                IsFavorite = toDoItemCreateRequest.Favorite,
                CreateTime = DateTime.UtcNow,
                DueDate = DateTime.Now
            };
            var result=await _toDoItemService.CreateTodoItem(toDoItemDto);
            return Created("", result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ToDoItemDto), 200)]
        [ProducesResponseType(typeof(ToDoItemDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(
            Summary = "Upsert Item",
            Description = "Create or replace a to-do item by id"
            )]
        public async Task<ActionResult<ToDoItemDto>> PutAsync(string id, [FromBody] ToDoItemDto toDoItemDto)
        {
            var updatetoDoItemDto = new TodoItems.Core.Model.TodoItem
            {
                Id = toDoItemDto.Id,
                Description = toDoItemDto.Description,
                IsComplete = toDoItemDto.Done,
                IsFavorite = toDoItemDto.Favorite,
                CreateTime = DateTime.UtcNow,
                DueDate = DateTime.Now,
            };

            _toDoItemService.ModifyTodoItem(updatetoDoItemDto.Id, updatetoDoItemDto);

            return Ok(toDoItemDto);
        }
    }
    }

