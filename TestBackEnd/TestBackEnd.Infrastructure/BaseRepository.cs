using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TestBackEnd.Domain.Interfaces;
using Z.EntityFramework.Plus;

namespace TestBackEnd.Infrastructure
{
    /// <summary>
    /// Clase Base generica encargada de realizar las operaciones de la base de datos
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// Contexto
        /// </summary>
        public TestDBContext _context;
        /// <summary>
        /// Entidad
        /// </summary>
        public DbSet<TEntity> _dbSet;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public BaseRepository(TestDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        /// <summary>
        /// Consulta todas las entidades
        /// </summary>
        public virtual async Task<IQueryable<TEntity>> GetAll(params Expression<Func<TEntity, object?>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var includeExpression in includes)
            {
                query = query.IncludeOptimized(includeExpression);
            }

            return query;
        }

        /// <summary>
        /// Consulta una entidad por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> FindById(object id)
        {
            return await _dbSet.FindAsync(id);
        }


        public virtual async Task<List<TEntity>> GetByFunction(Expression<Func<TEntity, bool>> function)
        {
            return await _dbSet.Where(function).ToListAsync();
        }

        /// <summary>
        /// Crea un entidad (Guarda)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> Create(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Added; // added row

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return (entity);
        }

        /// <summary>
        /// Actualiza la entidad (GUARDA)
        /// </summary>
        /// <param name="editedEntity">Entidad editada</param>
        /// <returns></returns>
        public virtual async Task<TEntity> Update(TEntity editedEntity)
        {

            _context.Entry(editedEntity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return (editedEntity);
        }

        /// <summary>
        /// Elimina una entidad (Guarda)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> Delete(TEntity entity)
        {
            _dbSet.Remove(entity);

            await _context.SaveChangesAsync();

            return (entity);
        }

        /// <summary>
        /// Guardar cambios
        /// </summary>
        public virtual async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public virtual async Task<TEntity> GetSingleByFunction(Expression<Func<TEntity, bool>> function)
        {
            return await _dbSet.FirstOrDefaultAsync(function);
        }
    }
}
