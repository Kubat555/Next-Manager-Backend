using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Data.DTO
{
    public class TasksDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PriorityId { get; set; }
        public string PriorityName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string? Desciption { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Deadline { get; set; }
        public string? ExecutorId { get; set; }
        public string? ExecutorName { get; set; }
        public int ProjectId { get; set; }
    }
}
