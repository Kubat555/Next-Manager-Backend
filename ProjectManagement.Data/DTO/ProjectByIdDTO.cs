using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Data.DTO
{
    public class ProjectByIdDTO
    {
        public ProjectsDTO Project { get; set; }
        public ICollection<UserDTO> Users { get; set; }
    }
}
