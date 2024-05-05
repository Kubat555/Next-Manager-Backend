using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Data.DTO;
using ProjectManagement.Data.Models;
using ProjectManagement.Services.Interfaces;
using ProjectManagement.Services.Models;
using System.Collections.Generic;

namespace ProjectManagement.Services.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public ProjectService(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;

        }
        public async Task<ApiResponse> AddEmployeeToProject(string userId, int projectId)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            var project = _context.Projects.FirstOrDefault(u => u.Id == projectId);
            if (user != null && project != null)
            {
                ProjectEmployee projectEmployee = new()
                {
                    UserId = userId,
                    ProjectId = projectId,
                    AddedDate = DateTime.Now
                };
                await _context.ProjectEmployees.AddAsync(projectEmployee);
                await _context.SaveChangesAsync();
                return new ApiResponse() { isSuccess = true, Message = "User added to project successfully!", StatusCode = 200 };
            }
            return new ApiResponse() { isSuccess = false, Message = "User or Project not found!", StatusCode = 400 };
        }

        public async Task<Project> CreateProject(ProjectCreateDTO projectCreateDTO)
        {
            var project = new Project()
            {
                Name = projectCreateDTO.Name,
                Description = projectCreateDTO.Description,
                CreatedDate = DateTime.Now
            };
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<ApiResponse> DeleteEmployeeInProject(string userId, int projectId)
        {
            var projectEmployee = _context.ProjectEmployees.FirstOrDefault(pe => pe.UserId == userId && pe.ProjectId == projectId);
            if (projectEmployee != null)
            {
                _context.ProjectEmployees.Remove(projectEmployee);
                await _context.SaveChangesAsync();
                return new ApiResponse() { isSuccess = true, Message = "User deleted from project successfully!", StatusCode = 200 };
            }
            return new ApiResponse() { isSuccess = false, Message = "User or Project not found!", StatusCode = 400 };
        }

        public async Task<ApiResponse> DeleteProject(int id)
        {
            var project = _context.Projects.FirstOrDefault(u => u.Id == id);
            if(project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();

                return new ApiResponse() { isSuccess = true, Message = "Project deleted succesfully!", StatusCode = 200 };
            }
            return new ApiResponse() { isSuccess = false, Message = "Project is not found!", StatusCode = 400 };

        }

        public async Task<ApiResponse> EditProject(Project project)
        {
            var existingProject = await _context.Projects.FindAsync(project.Id);

            if (existingProject == null)
            {
                return new ApiResponse() { isSuccess = false, Message = "Project not found!", StatusCode = 404 };
            }

            existingProject.Name = project.Name;
            existingProject.Description = project.Description;

            try
            {
                await _context.SaveChangesAsync();
                return new ApiResponse() { isSuccess = true, Message = "Project edited successfully!", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new ApiResponse() { isSuccess = false, Message = $"An error occurred while editing the project: {ex.Message}", StatusCode = 500 };
            }
        }

        public async Task<ApiResponse<ICollection<ProjectsDTO>>> GetEmployeeProjects(string userId)
        {
            if(userId != null)
            {
                var projectsDTO = await _context.ProjectEmployees
                .Where(pe => pe.UserId == userId)
                .Select(pe => new ProjectsDTO
                {
                    Id = pe.Project.Id,
                    Name = pe.Project.Name,
                    Description = pe.Project.Description,
                    CreatedDate = pe.Project.CreatedDate,
                    EmployeeProjectId = pe.Id,
                    EmployeeAddedDate = pe.AddedDate
                })
                .ToListAsync();
                return new ApiResponse<ICollection<ProjectsDTO>>()
                {
                    isSuccess = true,
                    Message = "Data received successfully",
                    StatusCode = 200,
                    Response = projectsDTO
                };
            }

            return new ApiResponse<ICollection<ProjectsDTO>>()
            {
                isSuccess = false,
                Message = "userId is Null",
                StatusCode = 400
            };
        }

        public async void SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
