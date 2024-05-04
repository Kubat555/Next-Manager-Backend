using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Data.Models
{
    public class ProjectEmployee
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProjectId { get; set; }
        public DateTime? AddedDate { get; set; }

        public User User { get; set; }
        public Project Project { get; set; }
    }
}
