﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Data.DTO
{
    public class ProjectCreateDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string UserId { get; set; }
        public bool isCompleted { get; set; } = false;
    }
}
