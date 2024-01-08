using Ballastlane.TaskManagement.Core.Entities;

namespace Ballastlane.TaskManagement.Core.Repositories
{
    public interface ITaskManagementRepository
    {
        Task<TaskItem> GetByIdAsync(int taskId);
        Task<List<TaskItem>> GetAllAsync();
        Task AddAsync(TaskItem taskItem);
        Task UpdateAsync(TaskItem taskItem);
        Task RemoveAsync(int taskId);
    }
}