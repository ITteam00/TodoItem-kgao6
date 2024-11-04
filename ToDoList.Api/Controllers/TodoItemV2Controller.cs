using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Api.Services;
using ToDoList.Api.Models;
using TodoItems.Core.service;
using Swashbuckle.AspNetCore.Annotations;
using MongoDB.Driver;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v2/[controller]")]
    [AllowAnonymous]
    public class TodoItemV2Controller : ControllerBase
    {
        private readonly IToDoItemService _toDoItemService;
        private readonly ILogger<TodoItemV2Controller> _logger;


        public TodoItemV2Controller(IToDoItemService toDoItemService, ILogger<TodoItemV2Controller> logger)
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
            var toDoItemDto = new ToDoItemDto
            {
                Description = toDoItemCreateRequest.Description,
                Done = toDoItemCreateRequest.Done,
                Favorite = toDoItemCreateRequest.Favorite,
                CreatedTime = DateTimeOffset.UtcNow
            };
            return toDoItemDto;
        }

    }
    }

