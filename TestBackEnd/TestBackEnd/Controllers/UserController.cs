using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TestBackEnd.Application.Interfaces;
using TestBackEnd.Domain.DTOs;

namespace TestBackEnd.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("SelectList")]
        public async Task<IActionResult> List()
        {
            return Ok(new ResponseDTO((int)HttpStatusCode.OK, "", await _userService.SelectList()));
        }


        [HttpGet("GetUserTask/{taskId}")]
        public async Task<IActionResult> GetUserTask([FromRoute]int taskId)
        {
            return Ok(new ResponseDTO((int)HttpStatusCode.OK, "", await _userService.SelectListTask(taskId)));
        }

    }
}