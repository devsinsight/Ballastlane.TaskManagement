using Ballastlane.TaskManagement.Core.Commands;
using Ballastlane.TaskManagement.Core.Entities;
using Ballastlane.TaskManagement.Core.Queries;

namespace Ballastlane.TaskManagement.Core.Mappers
{
    public interface ITaskItemMapper
    {
        TaskItem MapToAddEntity(AddTaskItemCommand command);
        TaskItem MapToUpdateEntity(UpdateTaskItemCommand command);
    }
}
