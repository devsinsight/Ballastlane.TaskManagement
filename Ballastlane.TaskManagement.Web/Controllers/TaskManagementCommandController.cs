using Ballastlane.TaskManagement.Core.Commands;
using Ballastlane.TaskManagement.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ballastlane.TaskManagement.Web.Controllers
{
    [Authorize("Basic")]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskManagementCommandController : ControllerBase
    {
        private readonly TaskManagementService _taskManagementService;

        public TaskManagementCommandController(TaskManagementService taskManagementService)
        {
            _taskManagementService = taskManagementService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTaskItem([FromBody] AddTaskItemCommand command)
        {
            await _taskManagementService.AddTaskItemAsync(command);
            return Ok();
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateTaskItem([FromBody] UpdateTaskItemCommand command)
        {
            await _taskManagementService.UpdateTaskItemAsync(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveTaskItem(int id)
        {
            var command = new RemoveTaskItemCommand { Id = id };
            await _taskManagementService.RemoveTaskItemAsync(command);
            return Ok();
        }
    }
}