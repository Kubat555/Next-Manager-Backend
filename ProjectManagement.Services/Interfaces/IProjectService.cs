using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Data.DTO;
using ProjectManagement.Data.Models;
using ProjectManagement.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Services.Interfaces
{
    public interface IProjectService
    {
        //Создание проекта
        Task<Project> CreateProject(ProjectCreateDTO projectCreateDTO);
        //Изменение проекта
        Task<ApiResponse<string>> EditProject(Project project);
        //Удаление проекта
        Task<ApiResponse<string>> DeleteProject(int id);
        //Добавление сотрудника в проект
        Task<bool> AddEmployeeToProject(string userId, int projectId);

        void SaveChanges();
    }
}
