using Ballastlane.TaskManagement.Core.Entities;
using Ballastlane.TaskManagement.Core.Queries;
using Ballastlane.TaskManagement.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ballastlane.TaskManagement.Web.Controllers
{
    [Authorize("Basic")]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskManagementQueryController : ControllerBase
    {
        private readonly TaskManagementService _taskManagementService;

        public TaskManagementQueryController(TaskManagementService taskManagementService)
        {
            _taskManagementService = taskManagementService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTaskItemById(int id)
        {
            var query = new GetTaskItemByIdQuery { Id = id };
            var taskItem = await _taskManagementService.GetTaskItemByIdAsync(query);
            if (taskItem == null)
            {
                return NotFound();
            }

            return Ok(taskItem);
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskItem>>> GetAllTaskItems()
        {
            var query = new GetAllTaskItemsQuery();
            var taskItems = await _taskManagementService.GetAllTaskItemsAsync(query);
            return Ok(taskItems);
        }
    }
}
