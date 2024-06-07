using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Data.DTO
{
    public class ProjectsDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool isCompleted { get; set; }

        public int EmployeeProjectId { get; set; }
        public DateTime? EmployeeAddedDate { get; set; }
    }
}
