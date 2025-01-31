using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TestBackEnd.Domain.DTOs.Auth;

namespace TestBackEnd.Application.Services
{
    /// <summary>
    /// Servicio que obtiene el usuario en sesion
    /// </summary>
    public class UserSessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public UserSessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Método para obtener el JWT almacenado en la sesión
        public ClaimsPrincipal GetJwtTokenFromSession()
        {
            return _httpContextAccessor.HttpContext.User;
        }

        // Método para obtener el nombre del usuario desde el JWT en sesión
        public UserSessionDTO GetUserFromSession()
        {
            var token = GetJwtTokenFromSession();

            return token?.Claims.Select(x => new UserSessionDTO
            {
                UserID = token?.Claims.FirstOrDefault(c => c.Type == "Id")?.Value,
                UserName = token?.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value,
                UserRoles = token?.Claims.Where(c => c.Type == "UserRoles")?.Select(x =>x.Value).ToArray()
            }).FirstOrDefault();
        }
    }
}
