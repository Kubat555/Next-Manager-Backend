using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Data.DTO;
using ProjectManagement.Data.Models;
using ProjectManagement.Services.Interfaces;
using ProjectManagement.Services.Models;

namespace ProjectManagement.Services.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public TaskService
            (
                ApplicationDbContext context, 
                UserManager<User> userManager, 
                RoleManager<IdentityRole> roleManager,
                IMapper mapper
            )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<ApiResponse> CreateTask(TaskCreateDTO taskCreateDTO)
        {
            if(taskCreateDTO != null)
            {
                var task = _mapper.Map<Tasks>(taskCreateDTO);
                await _context.AddAsync(task);
                await _context.SaveChangesAsync();
                return new ApiResponse()
                {
                    isSuccess = true,
                    Message = "Task created successfully!",
                    StatusCode = 200
                };
            }
            return new ApiResponse()
            {
                isSuccess = false,
                Message = "Failed to create task. Check the submitted data!",
                StatusCode = 400
            };
        }

        public async Task<ApiResponse> DeleteTask(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return new ApiResponse() { isSuccess = true, Message = "Task deleted succesfully!", StatusCode = 200 };
            }
            return new ApiResponse() { isSuccess = false, Message = "Task is not found!", StatusCode = 400 };
        }

        public async Task<ApiResponse> EditTask(Tasks task)
        {
            if (task == null)
            {
                return new ApiResponse() { isSuccess = false, Message = "Task object is null!", StatusCode = 400 };
            }

            var existingTask = await _context.Tasks.FindAsync(task.Id);

            if (existingTask == null)
            {
                return new ApiResponse() { isSuccess = false, Message = "Task not found!", StatusCode = 404 };
            }
            existingTask.Name = task.Name;
            existingTask.PriorityId = task.PriorityId;
            existingTask.StatusId = task.StatusId;
            existingTask.Deadline = task.Deadline;
            existingTask.ExecutorId = task.ExecutorId;

            try
            {
                await _context.SaveChangesAsync();
                return new ApiResponse() { isSuccess = true, Message = "Task edited successfully!", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new ApiResponse() { isSuccess = false, Message = $"An error occurred while editing the task: {ex.Message}", StatusCode = 500 };
            }
        }

        public async Task<ApiResponse<ICollection<TasksDTO>>> GetAllTasks(int projectId)
        {
            var tasks = await _context.Tasks
                .Where(t => t.ProjectId == projectId)
                .Select(t => new TasksDTO
                {
                    Id = t.Id,
                    Name = t.Name,
                    PriorityId = t.PriorityId,
                    PriorityName = t.Priority.Name,
                    StatusId = t.StatusId,
                    StatusName = t.Status.Name,
                    CreatedDate = t.CreatedDate,
                    Deadline = t.Deadline,
                    ExecutorId = t.ExecutorId,
                    ExecutorName = t.Executor.FirstName + " " + t.Executor.LastName,
                    ProjectId = t.ProjectId
                })
                .ToListAsync();
            if (tasks.Count == 0)
            {
                return new ApiResponse<ICollection<TasksDTO>>()
                {
                    isSuccess = true,
                    Message = "Tasks is empty!",
                    StatusCode = 200,
                    Response = null
                };
            }
            return new ApiResponse<ICollection<TasksDTO>>()
            {
                isSuccess = true,
                Message = "All tasks received!",
                StatusCode = 200,
                Response = tasks
            };
        }

        public async Task<ApiResponse<ICollection<TasksDTO>>> GetEmployeeTasks(int projectId, string employeeId)
        {
            var tasks = await _context.Tasks
                .Where(t => t.ProjectId == projectId && t.ExecutorId == employeeId)
                .Select(t => new TasksDTO
                {
                    Id = t.Id,
                    Name = t.Name,
                    PriorityId = t.PriorityId,
                    PriorityName = t.Priority.Name,
                    StatusId = t.StatusId,
                    StatusName = t.Status.Name,
                    CreatedDate = t.CreatedDate,
                    Deadline = t.Deadline,
                    ExecutorId = t.ExecutorId,
                    ExecutorName = t.Executor.FirstName + " " + t.Executor.LastName,
                    ProjectId = t.ProjectId
                })
                .ToListAsync();
            if (tasks.Count == 0)
            {
                return new ApiResponse<ICollection<TasksDTO>>()
                {
                    isSuccess = true,
                    Message = "Tasks is empty!",
                    StatusCode = 200,
                    Response = null
                };
            }
            return new ApiResponse<ICollection<TasksDTO>>()
            {
                isSuccess = true,
                Message = "All tasks received!",
                StatusCode = 200,
                Response = tasks
            };
        }

        public async Task<ApiResponse<ICollection<Status>>> GetAllStatuses()
        {
            var statuses = await _context.Statuses.ToListAsync();
            if(statuses.Count == 0)
            {
                return new ApiResponse<ICollection<Status>>()
                {
                    isSuccess = true,
                    Message = "Statuses is empty",
                    StatusCode = 200
                };
            }
            return new ApiResponse<ICollection<Status>>()
            {
                isSuccess = true,
                Message = "All tasks statuses received",
                StatusCode = 200,
                Response = statuses
            };
        }

        public async Task<ApiResponse<ICollection<Priority>>> GetAllPriorities()
        {
            var priorities = await _context.Priorities.ToListAsync();
            if (priorities.Count == 0)
            {
                return new ApiResponse<ICollection<Priority>>()
                {
                    isSuccess = true,
                    Message = "Priorities is empty",
                    StatusCode = 200
                };
            }
            return new ApiResponse<ICollection<Priority>>()
            {
                isSuccess = true,
                Message = "All tasks priorities received",
                StatusCode = 200,
                Response = priorities
            };
        }
    }
}
