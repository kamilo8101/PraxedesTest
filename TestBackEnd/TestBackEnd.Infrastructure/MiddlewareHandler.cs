using System.Globalization;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using TestBackEnd.Domain.DTOs;

namespace TestBackEnd.Infrastructure
{
    /// <summary>
    /// Clase encargada controlar las peticiones de la aplicacion
    /// </summary>
    public class MiddlewareHandler
    {
        private readonly RequestDelegate _next;
        public List<(Type type, int statusCode)> _errorTypes = new List<(Type type, int statusCode)>() {
            (typeof(AppException), (int)HttpStatusCode.BadRequest),
            (typeof(KeyNotFoundException), (int)HttpStatusCode.NotFound)
        };

        public MiddlewareHandler(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = this._errorTypes.Any(x => x.type == error.GetType())
                    ? this._errorTypes.First(x => x.type == error.GetType()).statusCode
                    : (int)HttpStatusCode.InternalServerError;

                var resModel = new ResponseDTO(response.StatusCode, message: error.InnerException.ToString() ?? error.Message);
                var result = JsonSerializer.Serialize(resModel);
                await response.WriteAsync(result);
            }
        }
    }

    public class AppException : Exception
    {
        public AppException() : base() { }

        public AppException(string message) : base(message) { }

        public AppException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }

}
