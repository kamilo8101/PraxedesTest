using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using TestBackEnd.Domain.DTOs;
using TestBackEnd.Domain.DTOs.Auth;

namespace TestBackEnd.Controllers
{
    /// <summary>
    /// Proporciona los métodos que responden a las solicitudes HTTP del inicio de sesión.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="loginServices"></param>
        public AccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Registra un usuario nuevo en la base de datos
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SwaggerOperation(Summary = "Registra un usuario nuevo en la base de datos", Description = "Devuelve mensaje de existo")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var user = new IdentityUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new ResponseDTO((int)HttpStatusCode.OK, "Usuario registrado con exito" ));
            }

            return BadRequest(new ResponseDTO((int)HttpStatusCode.BadRequest, "", result.Errors));
        }

        /// <summary>
        /// Inicio de sesión de un usuario.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>JWT con la informacion del usuario</returns>
        [SwaggerOperation(Summary = "Inicio de sesión de un usuario.", Description = "Token JWT")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim("UserName", user.UserName!),
                    new Claim("Id", user.Id)
                };

                authClaims.AddRange(userRoles.Select(role => new Claim("UserRoles", role)));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                    SecurityAlgorithms.HmacSha256));

                return Ok(new ResponseDTO((int)HttpStatusCode.OK, "", new JwtSecurityTokenHandler().WriteToken(token)));
            }

            return Unauthorized();
        }


        /// <summary>
        /// Crea un rol
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("add-role")]
        [SwaggerOperation(Summary = "Adiciona un rol nuevo", Description = "Devuelve mensaje de existo")]
        public async Task<IActionResult> AddRole([FromBody] string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(role));
                if (result.Succeeded)
                {
                    return Ok(new ResponseDTO((int)HttpStatusCode.OK, "Rol creado con exito" ));
                }

                return BadRequest(new ResponseDTO((int)HttpStatusCode.BadRequest, "", result.Errors));
            }

            return BadRequest(new ResponseDTO((int)HttpStatusCode.BadRequest, "El rol ya existe"));
        }

        /// <summary>
        /// ASigna roles a los usuarios
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SwaggerOperation(Summary = "Asigna un rol a un usuario", Description = "Devuelve mensaje de existo")]
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] UserRoleDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return BadRequest(new ResponseDTO((int)HttpStatusCode.BadRequest, "Usuario no encontrado"));
            }

            var result = await _userManager.AddToRoleAsync(user, model.Role);
            if (result.Succeeded)
            {
                return Ok(new ResponseDTO((int)HttpStatusCode.OK, "Rol asignado con exito" ));
            }

            return BadRequest(new ResponseDTO((int)HttpStatusCode.BadRequest, "", result.Errors));
        }
    }
}