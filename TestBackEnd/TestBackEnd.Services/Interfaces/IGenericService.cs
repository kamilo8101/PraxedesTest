namespace TestBackEnd.Application.Interfaces
{
    /// <summary>
    /// Interface generica para los servicios dde la  clase con la que se defina
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericService<T> where T : class
    {
        public Task<T> Get(Guid id);

        public Task<List<T>> List();

        public Task<T> Create(T data);

        public Task<T> Update(T data);

        public Task<object> Delete(Guid id);
    }
}