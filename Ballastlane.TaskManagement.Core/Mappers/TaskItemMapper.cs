using Ballastlane.TaskManagement.Core.Commands;
using Ballastlane.TaskManagement.Core.Entities;
using Ballastlane.TaskManagement.Core.Queries;

namespace Ballastlane.TaskManagement.Core.Mappers
{
    public class TaskItemMapper : ITaskItemMapper
    {
        public TaskItem MapToAddEntity(AddTaskItemCommand command)
        {
            return new TaskItem
            {
                Title = command.Title,
                Description = command.Description,
                DueDate = command.DueDate
            };
        }

        public TaskItem MapToUpdateEntity(UpdateTaskItemCommand command)
        {
            return new TaskItem
            {
                Id = command.Id,
                Title = command.Title ?? throw new ArgumentNullException(nameof(command.Title)),
                Description = command.Description ?? throw new ArgumentNullException(nameof(command.Description)),
                DueDate = command.DueDate
            };
        }
    }
}