using TestBackEnd.Domain.DTOs;
using TestBackEnd.Domain.DTOs.Project;

namespace TestBackEnd.Application.Interfaces
{
    /// <summary>
    /// Interface con los metodos para el servicio de proyectos
    /// </summary>
    public interface IProjectService 
    {
        public Task<ProjectDTO> Get(Guid id);

        public Task<List<ProjectDTO>> List();

        public Task<ProjectDTO> Create(ProjectDTO data);

        public Task<ProjectDTO> Update(ProjectDTO data);

        public Task<object> Delete(Guid id);

        public Task<FileDTO> ExcelReport();
    }
}
