using Ballastlane.TaskManagement.Core.Entities;
using Ballastlane.TaskManagement.Core.Repositories;
using System.Data.SqlClient;

namespace Ballastlane.TaskManagement.Infrastructure.Data
{
    public class TaskManagementRepository : ITaskManagementRepository
    {
        private readonly string connectionString;

        public TaskManagementRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<TaskItem> GetByIdAsync(int taskId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM TaskItems WHERE Id = @TaskId", connection))
                {
                    command.Parameters.AddWithValue("@TaskId", taskId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new TaskItem
                            {
                                Id = (int)reader["Id"],
                                Title = reader["Title"].ToString(),
                                Description = reader["Description"].ToString(),
                                DueDate = (DateTime)reader["DueDate"]
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<List<TaskItem>> GetAllAsync()
        {
            List<TaskItem> taskItems = new List<TaskItem>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM TaskItems", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            TaskItem taskItem = new TaskItem
                            {
                                Id = (int)reader["Id"],
                                Title = reader["Title"].ToString(),
                                Description = reader["Description"].ToString(),
                                DueDate = (DateTime)reader["DueDate"]
                            };

                            taskItems.Add(taskItem);
                        }
                    }
                }
            }

            return taskItems;
        }

        public async Task AddAsync(TaskItem taskItem)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("INSERT INTO TaskItems (Title, Description, DueDate) VALUES (@Title, @Description, @DueDate)", connection))
                {
                    command.Parameters.AddWithValue("@Title", taskItem.Title);
                    command.Parameters.AddWithValue("@Description", taskItem.Description);
                    command.Parameters.AddWithValue("@DueDate", taskItem.DueDate);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(TaskItem taskItem)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("UPDATE TaskItems SET Title = @Title, Description = @Description, DueDate = @DueDate WHERE Id = @TaskId", connection))
                {
                    command.Parameters.AddWithValue("@TaskId", taskItem.Id);
                    command.Parameters.AddWithValue("@Title", taskItem.Title);
                    command.Parameters.AddWithValue("@Description", taskItem.Description);
                    command.Parameters.AddWithValue("@DueDate", taskItem.DueDate);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task RemoveAsync(int taskId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("DELETE FROM TaskItems WHERE Id = @TaskId", connection))
                {
                    command.Parameters.AddWithValue("@TaskId", taskId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}