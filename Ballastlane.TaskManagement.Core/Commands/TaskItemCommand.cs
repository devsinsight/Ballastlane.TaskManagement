using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ballastlane.TaskManagement.Core.Commands
{
    public class AddTaskItemCommand
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class UpdateTaskItemCommand
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class RemoveTaskItemCommand
    {
        public int Id { get; set; }
    }

}
