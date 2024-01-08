using Ballastlane.TaskManagement.Core.Commands;
using Ballastlane.TaskManagement.Core.Entities;
using Ballastlane.TaskManagement.Core.Mappers;
using Ballastlane.TaskManagement.Core.Queries;
using Ballastlane.TaskManagement.Core.Repositories;

namespace Ballastlane.TaskManagement.Core.Services
{
    public class TaskManagementService
    {
        private readonly ITaskManagementRepository _taskManagementRepository;
        private readonly ITaskItemMapper _taskItemMapper;

        public TaskManagementService(ITaskManagementRepository taskManagementRepository, ITaskItemMapper taskItemMapper)
        {
            _taskManagementRepository = taskManagementRepository ?? throw new ArgumentNullException(nameof(taskManagementRepository));
            _taskItemMapper = taskItemMapper ?? throw new ArgumentNullException(nameof(taskItemMapper));
        }

        public async Task<TaskItem> GetTaskItemByIdAsync(GetTaskItemByIdQuery query)
        {
            return await _taskManagementRepository.GetByIdAsync(query.Id);
        }

        public async Task<List<TaskItem>> GetAllTaskItemsAsync(GetAllTaskItemsQuery query)
        {
            return await _taskManagementRepository.GetAllAsync();
        }

        public async Task AddTaskItemAsync(AddTaskItemCommand command)
        {
            var entity = _taskItemMapper.MapToAddEntity(command);
            await _taskManagementRepository.AddAsync(entity);
        }

        public async Task UpdateTaskItemAsync(UpdateTaskItemCommand command)
        {
            var entity = _taskItemMapper.MapToUpdateEntity(command);
            await _taskManagementRepository.UpdateAsync(entity);
        }

        public async Task RemoveTaskItemAsync(RemoveTaskItemCommand command)
        {
            await _taskManagementRepository.RemoveAsync(command.Id);
        }
    }

}
