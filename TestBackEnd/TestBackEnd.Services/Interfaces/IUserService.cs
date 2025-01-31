using TestBackEnd.Domain.DTOs;

namespace TestBackEnd.Application.Interfaces
{
    /// <summary>
    /// Interface con los metodos para el servicios de usuarios
    /// </summary>
    public interface IUserService
    {
        public Task<List<SelectListDTO>> SelectList();
        public Task<List<SelectListDTO>> SelectListTask(int type);
    }
}
