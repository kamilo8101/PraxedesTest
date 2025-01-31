using AutoMapper;
using TestBackEnd.Domain.DTOs.Project;
using TestBackEnd.Domain.DTOs.Task;
using ProjectEntity = TestBackEnd.Domain.Entities.Project;
using TaskEntity = TestBackEnd.Domain.Entities.Task;

namespace TestBackEnd.Infrastructure
{
    /// <summary>
    /// Clase para mapear automaticamente las entidades en DTOS o los DTOS en entidades
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProjectDTO, ProjectEntity>();
            CreateMap<ProjectEntity, ProjectDTO>();

            CreateMap<TaskDTO, TaskEntity>();
            CreateMap<TaskEntity, TaskDTO>();
        }
    }
}
