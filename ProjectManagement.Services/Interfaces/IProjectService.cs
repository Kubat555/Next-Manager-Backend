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
        Task<ApiResponse> EditProject(Project project);
        //Удаление проекта
        Task<ApiResponse> DeleteProject(int id);
        //Добавление сотрудника в проект
        Task<ApiResponse> AddEmployeeToProject(string userId, int projectId);
        //Удаление сотрудника из проекта
        Task<ApiResponse> DeleteEmployeeInProject(string userId, int projectId);
        //Получение списка проектов сотрудника
        Task<ApiResponse<ICollection<ProjectsDTO>>> GetEmployeeProjects(string userId);
        void SaveChanges();
    }
}
