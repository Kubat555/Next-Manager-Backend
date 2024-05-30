using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Data.DTO
{
    public class TaskCreateDTO
    {
        public string Name { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Deadline { get; set; }
        public string? ExecutorId { get; set; }
        public int ProjectId { get; set; }
        public string? Description { get; set; }
    }
}
