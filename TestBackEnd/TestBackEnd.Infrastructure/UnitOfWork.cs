using TestBackEnd.Domain.Interfaces;
using TestBackEnd.Domain.Entities;

namespace TestBackEnd.Infrastructure
{
    /// <summary>
    /// Clase que agrupa las unidades de trabajo para acceder a la base de datos
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        public readonly TestDBContext _context;
        
        public UnitOfWork(TestDBContext context)
        {
            _context = context;
        }
       
        private readonly IBaseRepository<Project>? _projectRepository;
        public IBaseRepository<Project> ProjectRepository => _projectRepository ?? new BaseRepository<Project>(_context);

        private readonly IBaseRepository<Domain.Entities.Task>? _cityRepository;
        public IBaseRepository<Domain.Entities.Task> TaskRepository => _cityRepository ?? new BaseRepository<Domain.Entities.Task>(_context);

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public void SaveChanges() => _context.SaveChanges();
    }
}