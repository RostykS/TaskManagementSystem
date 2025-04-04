using AutoMapper;
using TaskManagementSystem.API.Models;

namespace TaskManagementSystem.API.Profiles
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<TaskEntity, TaskDto>();
            CreateMap<TaskDto, TaskEntity>();
        }
    }
}