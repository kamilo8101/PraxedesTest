using System.Linq.Expressions;

namespace TestBackEnd.Domain.Interfaces
{
    /// <summary>
    /// Interface generica con los metodos para acceder a la base de datos de cualquier entidad
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// Consulta todas las entidades
        /// </summary>
        public Task<IQueryable<TEntity>> GetAll(params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Consulta una entidad por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<TEntity> FindById(object id);


        /// <summary>
        /// Consulta la entidad segun una condicion
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public Task<List<TEntity>> GetByFunction(Expression<Func<TEntity, bool>> function);


        /// <summary>
        /// Consulta la entidad segun una condicion
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public Task<TEntity> GetSingleByFunction(Expression<Func<TEntity, bool>> function);

        /// <summary>
        /// Crea un entidad (Guarda)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<TEntity> Create(TEntity entity);

        /// <summary>
        /// Actualiza la entidad (GUARDA)
        /// </summary>
        /// <param name="editedEntity">Entidad editada</param>
        /// <returns></returns>
        public Task<TEntity> Update(TEntity editedEntity);

        /// <summary>
        /// Elimina una entidad (Guarda)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<TEntity> Delete(TEntity entity);

        /// <summary>
        /// Guardar cambios
        /// </summary>
        public Task SaveChanges();
    }
}
