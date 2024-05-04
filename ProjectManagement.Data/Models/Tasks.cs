using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Data.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Deadline { get; set; }
        public int ExecutorId { get; set; }
        public int AuthorId { get; set; }
        public int ProjectId { get; set; }

        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public ProjectEmployee Executor { get; set; }
        public ProjectEmployee Author { get; set; }
        public Project Project { get; set; }
    }
}
