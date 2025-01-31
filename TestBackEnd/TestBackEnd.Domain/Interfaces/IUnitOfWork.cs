using ProjectEntity = TestBackEnd.Domain.Entities.Project;
using TaskEntity = TestBackEnd.Domain.Entities.Task;

namespace TestBackEnd.Domain.Interfaces
{

    /// <summary>
    /// Interface que agrupa todas las operaciones de la base de datos
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<ProjectEntity> ProjectRepository { get; }
        IBaseRepository<TaskEntity> TaskRepository { get; }
        void SaveChanges();

    }
}
