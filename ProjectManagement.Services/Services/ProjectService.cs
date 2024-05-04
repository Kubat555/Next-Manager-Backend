using Microsoft.AspNetCore.Identity;
using ProjectManagement.Data;
using ProjectManagement.Data.DTO;
using ProjectManagement.Data.Models;
using ProjectManagement.Services.Interfaces;
using ProjectManagement.Services.Models;

namespace ProjectManagement.Services.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProjectService(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<bool> AddEmployeeToProject(string userId, int projectId)
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
                return true;
            }
            return false;
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

        public Task<ApiResponse<string>> DeleteProject(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<string>> EditProject(Project project)
        {
            throw new NotImplementedException();
        }

        public async void SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
