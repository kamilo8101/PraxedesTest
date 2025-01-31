using TestBackEnd.Domain.DTOs.Task;

namespace TestBackEnd.Application.Interfaces
{
    /// <summary>
    /// Interface con los metodos para el servicio de tareas
    /// </summary>
    public interface ITaskService
    {
        public Task<TaskDTO> Get(Guid id);

        public Task<List<TaskDTO>> List();

        public Task<TaskDTO> Create(TaskDTO data);

        public Task<TaskDTO> Update(TaskDTO data);

        public Task<object> Delete(Guid id);
    }
}
