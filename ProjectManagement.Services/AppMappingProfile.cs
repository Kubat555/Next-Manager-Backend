
using AutoMapper;
using ProjectManagement.Data.DTO;
using ProjectManagement.Data.Models;

namespace ProjectManagement.Services
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<TaskCreateDTO, Tasks>();
            CreateMap<Project, ProjectsDTO>();
            CreateMap<User, UserDTO>();

        }
    }
}
