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
    public interface ITaskService
    {
        //Получение списка всех задач
        Task<ApiResponse<TasksBoardDTO>> GetAllTasks(int projectId);
        //Получения списка задач определенного пользователя
        Task<ApiResponse<TasksBoardDTO>> GetEmployeeTasks(int projectId, string employeeId);

        //Добавление задача
        Task<ApiResponse> CreateTask(TaskCreateDTO taskCreateDTO);

        //Удаление задачи
        Task<ApiResponse> DeleteTask(int id);

        //Редактирование задачи
        Task<ApiResponse> EditTask(int id, TaskCreateDTO task);

        Task<ApiResponse<ICollection<Status>>> GetAllStatuses();
        Task<ApiResponse<ICollection<Priority>>> GetAllPriorities();
    }
}
