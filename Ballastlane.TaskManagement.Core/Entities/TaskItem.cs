namespace Ballastlane.TaskManagement.Core.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}