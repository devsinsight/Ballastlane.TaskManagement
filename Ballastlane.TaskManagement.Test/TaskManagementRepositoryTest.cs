using Ballastlane.TaskManagement.Core.Commands;
using Ballastlane.TaskManagement.Core.Entities;
using Ballastlane.TaskManagement.Core.Mappers;
using Ballastlane.TaskManagement.Core.Queries;
using Ballastlane.TaskManagement.Core.Repositories;
using Ballastlane.TaskManagement.Core.Services;
using Moq;

namespace Ballastlane.TaskManagement.Test
{

    public class TaskManagementServiceTests
    {
        private readonly Mock<ITaskManagementRepository> repositoryMock;
        private readonly ITaskItemMapper taskItemMapper;
        private readonly TaskManagementService taskManagementService;

        public TaskManagementServiceTests()
        {
            repositoryMock = new Mock<ITaskManagementRepository>();
            taskItemMapper = new TaskItemMapper();
            taskManagementService = new TaskManagementService(repositoryMock.Object, taskItemMapper);
        }

        [Fact]
        public async Task AddTask_ShouldCallRepositoryAddAsync()
        {
            // Arrange
            var newTask = new AddTaskItemCommand { Title = "New Task", Description = "", DueDate = DateTime.Now.AddDays(-7) };

            repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<TaskItem>()));

            // Act
            await taskManagementService.AddTaskItemAsync(newTask);

            // Assert
            repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<TaskItem>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldUpdateTaskSuccessfully()
        {
            var updatedTask = new UpdateTaskItemCommand { Id = 1, Title = "Updated Task", Description = "", DueDate = DateTime.Now.AddDays(-7) };
            repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<TaskItem>()));

            // Act
            await taskManagementService.UpdateTaskItemAsync(updatedTask);

            // Assert
            repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<TaskItem>()), Times.Once);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldDeleteTaskSuccessfully()
        {
            // Arrange
            var taskIdToDelete = new RemoveTaskItemCommand { Id = 1 };
            repositoryMock.Setup(repo => repo.RemoveAsync(taskIdToDelete.Id));

            // Act
            await taskManagementService.RemoveTaskItemAsync(taskIdToDelete);

            // Assert
            repositoryMock.Verify(repo => repo.RemoveAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnTaskSuccessfully()
        {
            // Arrange
            var query = new GetTaskItemByIdQuery { Id = 1 };
            var existingTask = new TaskItem { Id = query.Id, Title = "Existing Task", Description = "", DueDate = DateTime.Today.AddDays(-7) };
            repositoryMock.Setup(repo => repo.GetByIdAsync(query.Id)).ReturnsAsync(existingTask);

            // Act
            var result = await taskManagementService.GetTaskItemByIdAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingTask, result);
            repositoryMock.Verify(repo => repo.GetByIdAsync(query.Id), Times.Once);
        }

        [Fact]
        public async Task GetAllTasksAsync_ShouldReturnAllTasksSuccessfully()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem { Id = 1, Title = "Task 1", Description = "Desc 1", DueDate = DateTime.Today.AddDays(-7) },
                new TaskItem { Id = 2, Title = "Task 2", Description = "Desc 2", DueDate = DateTime.Today.AddDays(-7) },
                new TaskItem { Id = 3, Title = "Task 3", Description = "Desc 3", DueDate = DateTime.Today.AddDays(-7) }
            };

            repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tasks);
            var query = new GetAllTaskItemsQuery();

            // Act
            var result = await taskManagementService.GetAllTaskItemsAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tasks.Count, result.Count);
            Assert.Equal(tasks, result);
            repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }


    }


}