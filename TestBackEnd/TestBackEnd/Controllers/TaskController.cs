using Microsoft.AspNetCore.Mvc;
using System.Net;
using TestBackEnd.Domain.DTOs;
using TestBackEnd.Application.Interfaces;
using TestBackEnd.Domain.DTOs.Task;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace TestBackEnd.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IGenericService<TaskDTO> _taskService;
        public TaskController(IGenericService<TaskDTO> taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Obtiene un listado de tareas
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(Summary = "Obtiene un listado de tareas", Description = "Devuelve una lista de tareas")]
        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            return Ok(new ResponseDTO((int)HttpStatusCode.OK, "", await _taskService.List()));
        }

        /// <summary>
        /// Busca un tarea por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SwaggerOperation(Summary = "Busca un tarea por su id", Description = "Devuelve una tarea")]
        [HttpGet("Get")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(new ResponseDTO((int)HttpStatusCode.OK, "", await _taskService.Get(id)));
        }

        /// <summary>
        /// Crea un proyecto
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [SwaggerOperation(Summary = "Crea un proyecto", Description = "Devuelve la tarea creada")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(TaskDTO data)
        {
            return Ok(new ResponseDTO((int)HttpStatusCode.OK, "", await _taskService.Create(data)));
        }
    }
}