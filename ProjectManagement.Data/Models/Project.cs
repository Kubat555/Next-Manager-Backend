using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Data.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        private DateTime _createdDate;
        public DateTime CreatedDate
        {
            get => _createdDate;
            set => _createdDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        public bool isCompleted { get; set; } = false;

        public ICollection<ProjectEmployee>? ProjectEmployees { get; set; }
    }
}
