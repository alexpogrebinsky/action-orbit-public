using AutoMapper;
using Microsoft.AspNetCore.Identity;
using mmDailyPlanner.Server.DTO;
using mmDailyPlanner.Server.Models;

namespace mmDailyPlanner.Server.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<IPlannerTask, PlannerTask>().ReverseMap();
            CreateMap<ICompletedTask, CompletedTask>().ReverseMap();
            CreateMap<IPlannerTask, TaskListDTO>().ReverseMap();
            CreateMap<PlannerTask, TaskDetailDTO>().ReverseMap();
            CreateMap<PlannerTask, AddTaskDTO>().ReverseMap();
            CreateMap<ICompletedTask, CompletedTaskDTO>().ReverseMap();
            CreateMap<User,  UserProfileDTO>().ReverseMap();
        }
    }
}
