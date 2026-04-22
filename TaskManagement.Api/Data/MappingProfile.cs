using AutoMapper;
using TaskManagement.Api.DTOs;
using TaskManagement.Api.Models;

namespace TaskManagement.Api.Data;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TaskItem, TaskSummaryDto>();
        CreateMap<TaskItem, TaskDto>()
            .ForMember(dest => dest.CreatorUsername, opt => opt.MapFrom(src => src.Creator != null ? src.Creator.UserName : string.Empty));
        CreateMap<CreateTaskDto, TaskItem>();
        CreateMap<UpdateTaskDto, TaskItem>();
    }
}
