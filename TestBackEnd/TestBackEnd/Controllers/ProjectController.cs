using Microsoft.AspNetCore.Mvc;
using System.Net;
using TestBackEnd.Domain.DTOs;
using TestBackEnd.Domain.DTOs.Project;
using TestBackEnd.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace TestBackEnd.Controllers
{
    /// <summary>
    /// Proporciona los endpoints para la gestion de proyecto
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Listado de proyecto
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(Summary = "Obtiene un listado de proyecto", Description = "Devuelve una lista de pryectos")]
        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            return Ok( new ResponseDTO((int)HttpStatusCode.OK, "", await _projectService.List()));
        }


        /// <summary>
        /// Busca un proyecto por su id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SwaggerOperation(Summary = "Busca un proyecto por su id", Description = "Devuelve un projecto")]
        [HttpGet("Get")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(new ResponseDTO((int)HttpStatusCode.OK, "", await _projectService.Get(id)));
        }


        /// <summary>
        /// Crea Un proyecto
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [SwaggerOperation(Summary = "Crea un proyecto", Description = "Devuelve el projecto creado")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ProjectDTO data)
        {
            return Ok(new ResponseDTO((int)HttpStatusCode.OK, "", await _projectService.Create(data)));
        }


        /// <summary>
        /// Edita un proyecto
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [SwaggerOperation(Summary = "Edita un proyecto", Description = "Devuelve el projecto editado")]
        [HttpPost("Update")]
        public async Task<IActionResult> Update(ProjectDTO data)
        {
            return Ok(new ResponseDTO((int)HttpStatusCode.OK, "", await _projectService.Update(data)));
        }

        /// <summary>
        /// Inactiva un proyecto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SwaggerOperation(Summary = "Inactiva un proyecto", Description = "Devuelve el projecto Inactivado")]
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(new ResponseDTO((int)HttpStatusCode.OK, "", await _projectService.Delete(id)));
        }


        /// <summary>
        /// Inactiva un proyecto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SwaggerOperation(Summary = "Inactiva un proyecto", Description = "Devuelve el projecto Inactivado")]
        [HttpGet("ExcelReport")]
        public async Task<IActionResult> ExcelReport()
        {
            var result = await _projectService.ExcelReport();
            return File(result.File, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "reporte.xlsx");
        }
    }
}